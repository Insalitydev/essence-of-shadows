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

            var helper = new CCLabel("Arrows to move, A - shoot, S - say to chat", "kongtext", 10) {
                Color = CCColor3B.Gray,
                AnchorPoint = CCPoint.AnchorMiddleTop,
                Position = new CCPoint(Settings.SCREEN_WIDTH/2, Settings.SCREEN_HEIGHT - 20),
            };

            AddChild(_label);
            AddChild(helper);

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