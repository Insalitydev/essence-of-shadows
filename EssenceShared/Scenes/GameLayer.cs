using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared.Entities;
using EssenceShared.Entities.Enemies;
using EssenceShared.Entities.Players;
using EssenceShared.Entities.Players;
using EssenceShared.Entities.Projectiles;

namespace EssenceShared.Scenes {
    /** В этой сцене обрабатывается вся игровая логика на стороне сервера 
     * и на этой сцене рисуются все данные на стороне клиента */

    public class GameLayer: CCLayer {
        public List<Entity> Entities = new List<Entity>();
        private readonly Object lockThis = new Object();

        public void AddEntity(EntityState es) {
            Entity entity = null;

            /* Получаем название объекта по изображению */
            string textureName = es.TextureName;
            textureName = textureName.Replace("\\", "/").Split('/').Last();

            /** TODO: можно ли вынести куда-нибудь? можно ли обойтись без этого? */
            switch (textureName){
                case Resources.ClassMystic:
                case Resources.ClassReaper:
                case Resources.ClassSniper:
                    entity = new Player(es.Id, textureName);
                    break;
                case Resources.ProjectileMystic:
                    entity = new MysticProjectile(es.Id);
                    break;
                case Resources.ItemChest:
                    entity = new Enemy(es.Id);
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

        protected override void AddedToScene() {
            base.AddedToScene();
            Schedule(UpdateDebug, 5);
        }

        public override void Update(float dt) {
            base.Update(dt);

            UpdateCollisions();
        }

        private void UpdateCollisions() {
            foreach (Entity e1 in Entities.ToList()){
                foreach (Entity e2 in Entities.ToList()) {
                    if (e1.Id != e2.Id && e1.Mask != null && e2.Mask != null && e1.Mask.IntersectsRect(e2.Mask)){
                        e1.Collision(e2);
                    }
                }
            }
        }

        private void UpdateDebug(float dt) {
            Log.Print(GetGameState().Serialize());
        }

        /** формируем игровое состояние и возвращаем его */

        public GameState GetGameState() {
            var gs = new GameState();

            foreach (Entity entity in Entities.ToList()){
                gs.Entities.Add(EntityState.ParseEntity(entity));
            }

            return gs;
        }


        /** Вызывается на клиенте для обновления текущего состояния игры */

        public void AppendGameState(GameState gs, string playerId) {
            /** Updating entities */
            foreach (EntityState entity in gs.Entities){
                int index = Entities.FindIndex(x=>x.Id == entity.Id);
                if (index != -1){
                    // Себя не обновляем, мы верим себе!
                    if (entity.Id != playerId){
                        Entities[index].AppendState(entity);
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
                Entities.Remove(child as Entity);
                base.RemoveChild(child, cleanup);
            }
        }
    }
}