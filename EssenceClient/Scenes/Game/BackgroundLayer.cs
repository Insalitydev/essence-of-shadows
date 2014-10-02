using CocosSharp;
using EssenceShared;

namespace EssenceClient.Scenes.Game {
    internal class BackgroundLayer: CCLayerColor {
        public BackgroundLayer() {
            Color = CCColor3B.Blue;
            Opacity = 50;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            AddCardinalImage(0, 0);
            AddCardinalImage(800, 0);
        }

        // TODO: Удалить
        private void AddCardinalImage(int p1, int p2) {
            var tmp = new CCSprite("Cardinal.png");

            tmp.Texture.IsAntialiased = false;
            tmp.PositionX = p1;
            tmp.PositionY = p2;
            tmp.Scale = 4;

            if (tmp.PositionX > Settings.ScreenWidth/2){
                tmp.FlipX = true;
            }

            AddChild(tmp);
        }
    }
}