using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared.Entities;
using EssenceShared.Entities.Player;
using EssenceShared.Entities.Projectiles;

namespace EssenceShared.Scenes {
    /** В этой сцене обрабатывается вся игровая логика на стороне сервера 
     * и на этой сцене рисуются все данные на стороне клиента */

    public class GameLayer: CCLayer {
        public List<Entity> entities = new List<Entity>();

        private Entity entity;

        public void AddEntity(EntityState es) {
            /* Получаем название объекта по изображению */
            string textureName = es.TextureName;
            textureName = textureName.Replace("\\", "/").Split('/').Last();

            switch (textureName){
                case "Mystic":
                case "Reaper":
                case "Sniper":
                    entity = new Player(es.Id, textureName);
                    break;
                case "MysticProjectile":
                    entity = new MysticProjectile(es.Id, new CCPoint(0, 0));
                    break;
            }

            if (entity != null){
                EntityState.AppendStateToEntity(entity, es);
                AddEntity(entity);
            }
            else{
                Log.Print("Error! Entity isn't created, New entity will not be added", LogType.ERROR);
            }
        }

        public void AddEntity(Entity e) {
            Log.Print("Spawn entity: " + EntityState.ParseEntity(e).Serialize());
            entities.Add(e);
            AddChild(e);
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            Schedule(UpdateDebug, 2);
        }

        private void UpdateDebug(float dt) {
            Log.Print(GetGameState().Serialize());
        }

        public override void Update(float dt) {
            base.Update(dt);
        }

        /** формируем игровое состояние и возвращаем его */

        public GameState GetGameState() {
            var gs = new GameState();

            foreach (Entity entity in entities){
                gs.entities.Add(EntityState.ParseEntity(entity));
            }

            return gs;
        }


        /** Вызывается на клиенте для обновления текущего состояния игры */

        public void AppendGameState(GameState gs, string playerId) {
            /** Updating entities */
            foreach (EntityState entity in gs.entities){
                int index = entities.FindIndex(x=>x.Id == entity.Id);
                if (index != -1){
                    // Себя не обновляем, мы верим себе!
                    if (entity.Id != playerId){
                        entities[index].AppendState(entity);
                    }
                }
                else{
                    AddEntity(entity);
                }
            }
        }


        /** Вызваем на сервере, обновляем состояние объекта в игре, если нет такого объекта - создаем */

        public void UpdateEntity(EntityState es) {
            int index = entities.FindIndex(x=>x.Id == es.Id);
            if (index == -1){
                AddEntity(es);
            }
            else{
                EntityState.AppendStateToEntity(entities[index], es);
            }
        }

        public Entity FindEntityById(string id) {
            int result = entities.FindIndex(x=>x.Id == id);
            if (result == -1){
                return null;
            }
            return entities[result];
        }
    }
}