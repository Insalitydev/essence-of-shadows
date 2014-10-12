using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared.Entities;
using EssenceShared.Entities.Enemies;
using EssenceShared.Entities.Map.Tiles;
using EssenceShared.Entities.Players;
using EssenceShared.Entities.Projectiles;
using EssenceShared.Game;
using Newtonsoft.Json;

namespace EssenceShared.Scenes {
    /** В этой сцене обрабатывается вся игровая логика на стороне сервера 
     * и на этой сцене рисуются все данные на стороне клиента */

    public class GameLayer: CCLayer {
        private readonly Object lockThis = new Object();
        public List<Entity> Entities = new List<Entity>();
        /** Состояние игрока на клиенте */
        public AccountState MyAccountState;
        public List<string> currentMap;
        // TODO: сделать в конструкторе
        public Locations Location = Locations.Town;

        /** AccState - если создается игрок, ему передается для связывания... (что плохо :( )*/

        public void AddEntity(EntityState es, AccountState accState = null) {
            Entity entity = null;

            /* Получаем название объекта по изображению */
            string textureName = es.TextureName;
            textureName = textureName.Replace("\\", "/").Split('/').Last();

            /** TODO: можно ли вынести куда-нибудь? можно ли обойтись без этого? */
            switch (textureName){
                case Resources.ClassMystic:
                case Resources.ClassReaper:
                case Resources.ClassSniper:
                    entity = new Player(es.Id, textureName, accState);
                    break;
                case Resources.ItemGold:
                    entity = new GoldStack(es.Id);
                    break;
                case Resources.ItemGate:
                    entity = new Gate(es.Id);
                    break;
                case Resources.ProjectileMystic:
                    entity = new MysticProjectile(es.Id);
                    break;
                case Resources.ProjectileLaser:
                    entity = new EnemyRangeProjectile(es.AttackDamage, textureName, es.Id);
                    break;
                case Resources.ParticleMeleeSweepAttack:
                    entity = new EnemyMeleeProjectile(es.AttackDamage, textureName, es.Id);
                    break;
                case Resources.EnemyMeleeRobot:
                    entity = new MeleeEnemy(textureName, es.Id);
                    break;
                case Resources.EnemyStinger:
                    entity = new RangeEnemy(textureName, es.Id);
                    break;
            }

            if (entity != null){
                EntityState.AppendStateToEntity(entity, es);
                AddEntity(entity);
            }
            else{
                Log.Print("Error! Entity isn't created, New entity will not be added", LogType.Error);
            }
        }

        public void AddEntity(Entity e) {
            Entities.Add(e);
            AddChild(e);
        }

        public override void Update(float dt) {
            base.Update(dt);

            UpdateCollisions();
        }


        public void CreateNewMap(List<string> tileMap) {
            // удаляем предыдущую карту
            RemoveAllChildrenByTag(Tags.MapTile);


            // запоминаем текущую карту
            currentMap = tileMap;

            // создаем карту (на клиенте)
            if (Tag == Tags.Client){
                Log.Print("Creating new map");
                Tile tile;
                // TODO: переделать на словарь
                for (int i = 0; i < tileMap.Count; i++){
                    for (int j = 0; j < tileMap[i].Length; j++){
                        switch (tileMap[i][j]){
                            case '#':
                                tile = new Tile(Resources.MapTileSand);
                                break;
                            case '~':
                                tile = new Tile(Resources.MapTileWater);
                                break;
                            case '+':
                                tile = new Tile(Resources.MapTileTownCell);
                                break;
                            case '-':
                                tile = new Tile(Resources.MapTileCityStone);
                                break;
                            case '|':
                                tile = new Tile(Resources.MapTileRoad);
                                break;
                            case '%':
                                tile = new Tile(Resources.MapTileCaveWall);
                                break;
                            case '.':
                                tile = new Tile(Resources.MapTileDirt);
                                break;
                            default:
                                tile = new Tile(Resources.MapTileError);
                                break;
                        }
                        tile.Scale = Settings.Scale;
                        // Поворачиваем карту на 90 (поэтому поменяны местами i и j)
                        tile.Position = new CCPoint(j*Settings.TileSize*tile.ScaleX, i*Settings.TileSize*tile.ScaleY);
                        AddChild(tile, -100, Tags.MapTile);
                    }
                }
                Log.Print("Map created");
            }
        }

        public string SerializeMap() {
            Log.Print("Serialize map");
            return JsonConvert.SerializeObject(currentMap);
        }

        public void DeserializeMap(string jsonMap) {
            Log.Print("Deserialize map");
            var tileMap = JsonConvert.DeserializeObject<List<string>>(jsonMap);
            CreateNewMap(tileMap);
        }

        private void UpdateCollisions() {
            List<Entity> tmpList = Entities.ToList();
            foreach (Entity e1 in tmpList){
                foreach (Entity e2 in tmpList){
                    if (e1.Id != e2.Id && e1.Mask.IntersectsRect(e2.Mask)){
                        e1.Collision(e2);
                    }
                }
            }
        }


        /** Вызывается на клиенте для обновления текущего состояния игры */

        public void AppendGameState(GameState gs, string playerId) {
            /** Updating entities */
            foreach (EntityState entity in gs.Entities.ToList()){
                int index = Entities.FindIndex(x=>x.Id == entity.Id);
                if (index != -1){
                    // У себя обновляем все, кроме местоположения
                    if (entity.Id != playerId){
                        Entities[index].AppendState(entity);
                    }
                    else{
                        // Наш персонаж
                        CCPoint pos = Entities[index].Position;
                        Entities[index].AppendState(entity);
                        Entities[index].Position = pos;
                    }
                }
                else{
                    AddEntity(entity);
                }
            }
            /** Проверяем, если у нас есть на сцене объект, которого нет в новом состоянии - убираем его*/
            foreach (Entity entity in Entities.ToList()){
                if (gs.Entities.FindIndex(x=>x.Id == entity.Id) == -1){
                    entity.Remove();
                }
            }
            /** обновляем состояние аккаунта */
            if (gs.Account != null && gs.Account.HeroId == playerId){
                MyAccountState = gs.Account;
            }
        }


        /** Вызваем на сервере, обновляем состояние объекта в игре, если нет такого объекта - создаем */

        public void UpdateEntity(EntityState es) {
            int index = Entities.FindIndex(x=>x.Id == es.Id);
            if (index == -1){
                AddEntity(es);
            }
            else{
                EntityState.AppendStateToEntity(Entities[index], es);
            }
        }

        public Entity FindEntityById(string id) {
            int result = Entities.FindIndex(x=>x.Id == id);
            if (result == -1){
                return null;
            }
            return Entities[result];
        }

        /** Удаляет объект, вызывается автоматически при вызове Remove у объекта-сына  */

        public override void RemoveChild(CCNode child, bool cleanup = true) {
            lock (lockThis){
                if (child != null){
                    Entities.Remove(child as Entity);
                    base.RemoveChild(child, cleanup);
                }
            }
        }
    }
}