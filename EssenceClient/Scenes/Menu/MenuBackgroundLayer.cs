using CocosSharp;
using EssenceShared;

namespace EssenceClient.Scenes.Game {
    internal class MenuBackgroundLayer: CCLayerColor {
        public MenuBackgroundLayer() {
            Color = CCColor3B.Yellow;
            Opacity = 20;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            AddTitle();
            AddCardinalImage(0, 0);
            AddCardinalImage(800, 0);
            AddCardinalImage(800, 600);
            AddCardinalImage(0, 600);
        }

        private void AddCardinalImage(int p1, int p2) {
            var tmp = new CCSprite("Cardinal.png");

            tmp.Texture.IsAntialiased = false;
            tmp.PositionX = p1;
            tmp.PositionY = p2;
            tmp.Scale = 4;

            AddChild(tmp);
        }

        private void AddTitle() {
            var title = new CCLabel(Settings.GAME_NAME, "kongtext", 24) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorMiddleBottom,
                PositionX = 400,
                PositionY = 400,
            };

            var helper = new CCLabel("Press space to continue", "kongtext", 12) {
                Color = CCColor3B.Gray,
                AnchorPoint = CCPoint.AnchorMiddle,
                PositionX = 400,
                PositionY = 300,
            };

            AddChild(title);
        }
    }
}