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
            var textureName = es.TextureName;
            Log.Print(textureName);
            textureName = textureName.Replace("\\", "/").Split('/').Last();
            Log.Print(textureName);
            switch (textureName){
                case "Mystic":
                    entity = new Player(es.Id);
                    break;
                case "MysticProjectile":
                    entity = new MysticProjectile(es.Id, new CCPoint(0, 0));
                    break;
            }

            EntityState.AppendStateToEntity(entity, es);
//            var tmp = new MysticProjectile(es.Id, new CCPoint(0, 0));
//            //            var tmp = new Entity(es.TextureName, es.Id);
//            {
//                /** TODO: формировать объекты не тут...*/
//                tmp.PositionX = es.PositionX;
//                tmp.PositionY = es.PositionY;
//                tmp.Scale = es.Scale;
//            }
            AddEntity(entity);
        }

        public void AddEntity(Entity e) {
            entities.Add(e);
            AddChild(e);
        }


        public override void Update(float dt) {
            base.Update(dt);
        }

        public GameState GetGameState() {
            var gs = new GameState();

            foreach (Entity entity in entities){
                gs.entities.Add(EntityState.ParseEntity(entity));
            }
            Log.Print(gs.Serialize());
            return gs;
        }

        /** Клиентское */

        public void AppendGameState(GameState gs, string playerId) {
            /** Updating entities */
            foreach (EntityState entity in gs.entities){
                Log.Print("SHOULD UPDATE" + entity.Id);
                int index = entities.FindIndex(x=>x.Id == entity.Id);
                if (index != -1){
                    if (entity.Id != playerId){
                        entities[index].AppendState(entity);
                    }
                }
                else{
                    AddEntity(entity);
                }
            }
        }


        /** Вызывается на стороне клиента. 
         * playerId - ИД игрока, кто вызывает метод, необходим для исключения обновления свого персонажа
         * PlayerState - игрок, информацию которого необходимо обновить */


        /** серверное */

        public void UpdateEntity(Entity e, string playerid) {
            int index = entities.FindIndex(x=>x.Id == e.Id);
            if (index == -1){
                AddEntity(e);
            }
            else{
                if (playerid == e.Id){
                    return;
                }
                entities[index].PositionX = e.PositionX;
                entities[index].PositionY = e.PositionY;
            }
        }

        public void UpdateEntity(Entity e) {
            UpdateEntity(e, "FALSE_ID");
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