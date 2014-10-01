using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared.Entities;
using EssenceShared.Entities.Enemies;
using EssenceShared.Entities.Players;
using EssenceShared.Entities.Projectiles;

namespace EssenceShared.Scenes {
    /** В этой сцене обрабатывается вся игровая логика на стороне сервера 
     * и на этой сцене рисуются все данные на стороне клиента */

    public class GameLayer: CCLayer {
        private readonly Object lockThis = new Object();
        public List<Entity> Entities = new List<Entity>();
        /** Состояние игрока на клиенте */
        public AccountState MyAccountState;

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

        public override void Update(float dt) {
            base.Update(dt);

            UpdateCollisions();
        }

        private void UpdateCollisions() {
            List<Entity> tmpList = Entities.ToList();
            foreach (Entity e1 in tmpList){
                foreach (Entity e2 in tmpList){
                    if (e1.Id != e2.Id && e1.Mask != null && e2.Mask != null && e1.Mask.IntersectsRect(e2.Mask)){
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
                if  (child != null){
                    Entities.Remove(child as Entity);
                    base.RemoveChild(child, cleanup);
                }
            }
        }
    }
}