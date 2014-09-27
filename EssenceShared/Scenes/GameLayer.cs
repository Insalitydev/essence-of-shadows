using System.Collections.Generic;
using CocosSharp;
using EssenceShared.Entities;
using EssenceShared.Entities.Player;

namespace EssenceShared.Scenes {
    /** В этой сцене обрабатывается вся игровая логика на стороне сервера 
     * и на этой сцене рисуются все данные на стороне клиента */

    public class GameLayer: CCLayer {
        public List<Entity> entities = new List<Entity>();
        public List<Player> players = new List<Player>();

        public void AddPlayer(PlayerState e) {
            var tmp = new Player(e.Id);
            {
                tmp.PositionX = e.PositionX;
                tmp.PositionY = e.PositionY;
            }
            players.Add(tmp);
            AddChild(tmp);
        }

        public void AddEntity(EntityState e) {
            var tmp = new Entity(e.TextureName, e.Id);
            {
                /** TODO: формировать объекты не тут...*/
                tmp.PositionX = e.PositionX;
                tmp.PositionY = e.PositionY;
                tmp.Scale = e.Scale;
            }
            AddEntity(tmp);
        }

        public override void Update(float dt) {
            base.Update(dt);
        }

        public GameState GetGameState() {
            var gs = new GameState();
            foreach (Player player in players){
                gs.players.Add(PlayerState.ParsePlayer(player));
            }
            foreach (Entity entity in entities){
                gs.entities.Add(EntityState.ParseEntity(entity));
            }
            Log.Print(gs.Serialize());
            return gs;
        }

        /** Клиентское */
        public void AppendGameState(GameState gs, string playerId) {
            /** Updating players */
            foreach (PlayerState player in gs.players){
                
                int index = players.FindIndex(x=>x.Id == player.Id);
                if (index != -1){
                    if (player.Id != playerId) {
                        players[index].AppendState(player);
                    }
                    
                }
                else{
                    AddPlayer(player);
                }
            }

            /** Updating entities */
            foreach (EntityState entity in gs.entities){
                Log.Print("SHOULD UPDATE" + entity.Id);
                int index = entities.FindIndex(x=>x.Id == entity.Id);
                if (index != -1){
                    entities[index].AppendState(entity);
                }
                else{
                    AddEntity(entity);
                }
            }
        }

        public void AddEntity(Entity e) {
            entities.Add(e);
            AddChild(e);
        }

        /** Вызывается на стороне клиента. 
         * playerId - ИД игрока, кто вызывает метод, необходим для исключения обновления свого персонажа
         * PlayerState - игрок, информацию которого необходимо обновить */



        /** серверное */
        public void UpdateEntity(PlayerState e, string playerid) {
            int index = players.FindIndex(x=>x.Id == e.Id);

            if (index != -1){
                players[index].PositionX = e.PositionX;
                players[index].PositionY = e.PositionY;
            }
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

        public Player FindPlayerById(string id) {
            Log.Print("SEARCH " + id);
            int result = players.FindIndex(x => x.Id == id);
            if (result == -1) {
                return null;
            }
            return players[result];
        }
    }
}