using CocosSharp;
using EssenceShared;

namespace EssenceClient.Scenes.Game {
    internal class HudLayer: CCLayer {
        private float _fps;
        private CCLabel _label;
        private int _step;

        public HudLayer() {
            _fps = 0;
            _step = 0;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            _label = new CCLabel("HUD Text", "kongtext", 16);
            _label.Color = CCColor3B.Gray;
            _label.AnchorPoint = CCPoint.AnchorMiddleTop;
            _label.Position = new CCPoint(Settings.SCREEN_WIDTH/2, Settings.SCREEN_HEIGHT);

            AddChild(_label);

            Schedule(Update);
        }

        public override void Update(float dt) {
            base.Update(dt);

            _fps = 1 / dt;
            _step++;

            _label.Text = "FPS: " + (int)_fps + " Step: " + _step;
        }
    }
}