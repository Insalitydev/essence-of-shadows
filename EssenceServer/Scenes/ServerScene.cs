using CocosSharp;
using EssenceShared;
using EssenceShared.Entities.Enemies;
using EssenceShared.Scenes;

namespace EssenceServer.Scenes {
    internal class ServerScene: CCScene {
        public readonly GameLayer GameLayer;

        public ServerScene(CCWindow window): base(window) {
            GameLayer = new GameLayer {Tag = Settings.Server};
            AddChild(GameLayer);

            Log.Print("Game has started, waiting for players");
            Schedule(Update, 0.04f);
            Schedule(UpdateLogic);

            // TODO: У сцены автоматически не вызывается addedToScene?
            AddedToScene();
        }

        protected override void AddedToScene() {
            base.AddedToScene();
            Log.Print("SDSDSSDSDSD");
            GameLayer.AddEntity(new Enemy(Server.GetUniqueId()){PositionX = 600, PositionY = 300});
        }

        private void UpdateLogic(float dt) {
            GameLayer.Update(dt);
        }

        public override void Update(float dt) {
            base.Update(dt);

            Server.SendGameStateToAll();
        }

        internal void AppendPlayerState(EntityState es) {
            GameLayer.UpdateEntity(es);
        }
    }
}