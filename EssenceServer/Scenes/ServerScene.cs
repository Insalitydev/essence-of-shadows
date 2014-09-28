using CocosSharp;
using EssenceShared;
using EssenceShared.Scenes;

namespace EssenceServer.Scenes {
    internal class ServerScene: CCScene {
        public readonly GameLayer GameLayer;

        public ServerScene(CCWindow window): base(window) {
            GameLayer = new GameLayer();
            AddChild(GameLayer);

            Log.Print("Game has started, waiting for players");
            Schedule(Update, 0.04f);
            Schedule(UpdateLogic);
        }


        private void UpdateLogic(float dt) {
            GameLayer.Update(dt);
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

            GameLayer.UpdateEntity(es);
        }
    }
}