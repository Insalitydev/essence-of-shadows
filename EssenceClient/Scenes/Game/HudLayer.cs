using CocosSharp;
using EssenceShared;

namespace EssenceClient.Scenes.Game {
    internal class HudLayer: CCLayer {
        private float _fps;
        private CCLabelTtf _label;
        public int _step { get; private set; }
        private int _lastGold;

        public HudLayer() {
            _fps = 0;
            _step = 0;
            _lastGold = 0;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            _label = new CCLabelTtf("HUD Text", "kongtesxt", 14) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorMiddleTop,
                Position = new CCPoint(Settings.ScreenWidth/2, Settings.ScreenHeight)
            };

            var helper = new CCLabelTtf("Arrows to move, A - shoot, S - say to chat", "kongtext", 10) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorMiddleTop,
                Position = new CCPoint(Settings.ScreenWidth/2, Settings.ScreenHeight - 20),
            };

            AddChild(_label);
            AddChild(helper);

            Schedule(Update);
        }

        public override void Update(float dt) {
            base.Update(dt);

            _fps = 1/dt;
            _step++;
            int gold = 10;
            var gameScene = Parent as GameScene;
            if (gameScene != null && gameScene.GameLayer.MyAccountState != null) {
                gold = gameScene.GameLayer.MyAccountState.Gold;
                if (_lastGold < gold){
                    _lastGold += 4;
                }
                else{
                    _lastGold = gold;
                }
            }
            _label.Text = "FPS: " + (int) _fps + " Step: " + _step + " Gold: " + _lastGold;
        }
    }
}