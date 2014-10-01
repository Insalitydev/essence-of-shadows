using CocosSharp;
using EssenceShared;

namespace EssenceClient.Scenes.Menu {
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
            var tmp = new CCSprite(Resources.BossCardinal) {
                Texture = {IsAntialiased = false},
                PositionX = p1,
                PositionY = p2,
                Scale = 4
            };

            if (tmp.PositionX > Settings.ScreenWidth / 2) {
                tmp.FlipX = true;
            }

            AddChild(tmp);
        }

        private void AddTitle() {
            var title = new CCLabelTtf(Settings.GameName, "kongtext", 24) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorMiddleTop,
                PositionX = 400,
                PositionY = 400,
            };

            var helper = new CCLabelTtf("Enter/Space to start, Esc to exit", "kongtext", 10) {
                Color = CCColor3B.Gray,
                AnchorPoint = CCPoint.AnchorMiddleBottom,
                PositionX = Settings.ScreenWidth/2,
                PositionY = 0
            };

            AddChild(title);
            AddChild(helper);
        }
    }
}