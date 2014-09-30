using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Enemies;
using EssenceShared.Scenes;

namespace EssenceServer.Scenes {
    internal class ServerScene: CCScene {
        public readonly GameLayer GameLayer;
        public List<AccountState> Accounts = new List<AccountState>();

        public ServerScene(CCWindow window): base(window) {
            GameLayer = new GameLayer {Tag = Tags.Server};
            AddChild(GameLayer);

            Log.Print("Game has started, waiting for players");
            Schedule(Update, 0.04f);
            Schedule(UpdateLogic);

            // TODO: У сцены автоматически не вызывается addedToScene?
            AddedToScene();
        }

        /** формируем игровое состояние и возвращаем его */

        public GameState GetGameState() {
            var gs = new GameState();

            foreach (Entity entity in GameLayer.Entities.ToList()) {
                gs.Entities.Add(EntityState.ParseEntity(entity));
            }


            return gs;
        }

        protected override void AddedToScene() {
            base.AddedToScene();
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