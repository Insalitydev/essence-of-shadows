using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared.Entities;
using EssenceShared.Entities.Player;

namespace EssenceShared.Scenes {
    /** В этой сцене обрабатывается вся игровая логика на стороне сервера 
     * и на этой сцене рисуются все данные на стороне клиента */

    public class GameLayer: CCLayer {
        public List<Entity> entities = new List<Entity>();

        public void AddEntity(PlayerState e) {
            var tmp = new Player(e.Id);
            {
                tmp.PositionX = e.PositionX;
                tmp.PositionY = e.PositionY;
            }
            AddEntity(tmp);
        }

        public override void Update(float dt) {
//            Log.Print("Update server logic");
            base.Update(dt);

        }

        public void AddEntity(Entity e) {
            Log.Print("Adding entity " + e.Id);
            entities.Add(e);
            AddChild(e);
        }

        /** Вызывается на стороне клиента. 
         * playerId - ИД игрока, кто вызывает метод, необходим для исключения обновления свого персонажа
         * PlayerState - игрок, информацию которого необходимо обновить */

        public void UpdateEntity(PlayerState e, string playerid) {
            var tmp = new Player(e.Id);
            {
                tmp.PositionX = e.PositionX;
                tmp.PositionY = e.PositionY;
            }
            UpdateEntity(tmp, playerid);
        }

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

            Console.WriteLine(entities);
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