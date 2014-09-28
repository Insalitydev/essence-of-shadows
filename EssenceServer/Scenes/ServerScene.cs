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

            Log.Print("Game has started, waiting for players");
            Schedule(Update, 0.04f);
            Schedule(UpdateLogic);
        }


        private void UpdateLogic(float dt) {
            _gameLayer.Update(dt);
        }


        public override void Update(float dt) {
            base.Update(dt);

            Server.SendGameStateToAll();
        }

//        internal void AddNewPlayer(string id, int x, int y) {
//            var newPlayer = new PlayerState(id);
//            newPlayer.PositionX = x;
//            newPlayer.PositionY = y;
//
//            _gameLayer.AddPlayer(newPlayer);
//
//            GameState.players.Add(newPlayer);
//            Log.Print("New player spawned");
//        }


        internal void AppendPlayerState(EntityState es) {
//            int id = _gameLayer.entities.FindIndex(x=>x.Id == es.Id);

            _gameLayer.UpdateEntity(es);
        }
    }
}