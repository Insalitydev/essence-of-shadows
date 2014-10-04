using CocosSharp;

namespace EssenceClient.Scenes.Game {
    internal class BackgroundLayer: CCLayerColor {
        public BackgroundLayer() {
            Color = CCColor3B.Blue;
            Opacity = 50;
        }
    }
}