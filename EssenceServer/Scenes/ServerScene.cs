using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Projectiles;
using EssenceShared.Scenes;

namespace EssenceServer.Scenes {
    internal class ServerScene: CCScene {
        public readonly GameLayer _gameLayer;

        public ServerScene(CCWindow window): base(window) {
            _gameLayer = new GameLayer();
            AddChild(_gameLayer);
            GameState = new GameState();

            Log.Print("Game has started, waiting for players");
            Schedule(Update, 0.04f);
            Schedule(UpdateLogic);
        }

        public GameState GameState { get; private set; }

        private void UpdateLogic(float dt) {
            _gameLayer.Update(dt);
        }


        public override void Update(float dt) {
            base.Update(dt);

            Server.SendGameStateToAll();
        }

        internal void AddNewPlayer(string id, int x, int y) {
            GameState.playersCount++;
            var newPlayer = new PlayerState(id);
            newPlayer.PositionX = x;
            newPlayer.PositionY = y;

            _gameLayer.AddEntity(newPlayer);

            GameState.players.Add(newPlayer);
            Log.Print("New player spawned");
        }

        internal void AddNewEntity(string id, float x, float y) {
            Entity newEntity = new MysticProjectile(id, new CCPoint(0, 0));
            newEntity.PositionX = x;
            newEntity.PositionY = y;

            _gameLayer.AddEntity(newEntity);

            var es = new EntityState(id);
            es.PositionX = x;
            es.PositionY = y;
            GameState.entities.Add(es);
            Log.Print("New entitity spawned");
        }

        internal void UpdateGameState(PlayerState ps) {
            int id = GameState.players.FindIndex(x=>x.Id == ps.Id);
            GameState.players[id] = ps;

            _gameLayer.UpdateEntity(ps, ps.Id);

            foreach (Entity ent in _gameLayer.entities){
                var es = new EntityState(ent.Id);
                es.PositionX = ent.PositionX;
                es.PositionY = ent.PositionY;

                int ind = GameState.entities.FindIndex(x=>x.Id == es.Id);
                if (ind != -1)
                    GameState.entities[ind] = es;
            }
        }
    }
}