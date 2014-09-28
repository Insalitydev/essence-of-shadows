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

        internal void AppendPlayerState(EntityState es) {
            GameLayer.UpdateEntity(es);
        }
    }
}