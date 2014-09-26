using CocosSharp;

namespace EssenceClient.Scenes.Game {
    internal class GameScene: CCScene {
        private BackgroundLayer _backgroundLayer;
        private HudLayer _hudLayer;

        public GameScene(CCWindow window): base(window) {
            _backgroundLayer = new BackgroundLayer();
            AddChild(_backgroundLayer);

            _hudLayer = new HudLayer();
            AddChild(_hudLayer);
        }
    }
}