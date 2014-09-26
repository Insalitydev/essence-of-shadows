using CocosSharp;

namespace EssenceClient.Scenes.Game {
    internal class BackgroundLayer: CCLayerColor {
        public BackgroundLayer() {
            Color = CCColor3B.Blue;
            Opacity = 50;
        }

        protected override void AddedToScene() {
            base.AddedToScene();
        }
    }
}