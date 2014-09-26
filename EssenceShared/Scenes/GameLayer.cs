using System.Collections.Generic;
using CocosSharp;
using EssenceShared.Entities;
using EssenceShared.Entities.Player;

namespace EssenceShared.Scenes {
    /** В этой сцене обрабатывается вся игровая логика на стороне сервера 
     * и на этой сцене рисуются все данные на стороне клиента */

    public class GameLayer: CCLayer {
        private readonly List<Entity> entities = new List<Entity>();

        public void AddEntity(PlayerState e) {
            Log.Print("Adding entity " + e.Id);
            var tmp = new Player(e.Id);
            {
                tmp.PositionX = e.PositionX;
                tmp.PositionY = e.PositionY;
            }
            AddChild(tmp);
            entities.Add(tmp);
        }

        /** Вызывается на стороне клиента. 
         * playerId - ИД игрока, кто вызывает метод, необходим для исключения обновления свого персонажа
         * PlayerState - игрок, информацию которого необходимо обновить */
        public void UpdateEntity(PlayerState e, ulong playerid) {
            Log.Print("Update entity");
            var index = entities.FindIndex(x=>x.Id == e.Id);
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

        public Entity FindEntityById(ulong id) {
            int result = entities.FindIndex(x => x.Id == id);
            if (result == -1) {
                return null;
            }
            return entities[result];
        }

    }
}