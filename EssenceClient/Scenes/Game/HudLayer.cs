using CocosSharp;
using EssenceShared;
using EssenceShared.Entities.Players;

namespace EssenceClient.Scenes.Game {
    internal class HudLayer: CCLayer {
        private const int BarWidth = 500;
        private const int BarHeight = 18;
        private float _fps;
        private CCLabelTtf _goldLabel;
        private CCLabelTtf _hpLabel;
        private CCLabelTtf _label;
        private int _lastGold;

        private CCLabelTtf _levelLabel;

        public HudLayer() {
            _fps = 0;
            _step = 0;
            _lastGold = 0;
        }

        public int _step { get; private set; }

        protected override void AddedToScene() {
            base.AddedToScene();

            _label = new CCLabelTtf("HUD Text", "kongtext", 14) {
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

            AddHpLabel();
            AddLevelLabel();
            AddGoldLabel();

            Schedule(Update);
        }


        private void AddGoldLabel() {
            _goldLabel = new CCLabelTtf("Gold:", "kongtext", 10) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorLowerLeft,
                Position = new CCPoint(Settings.ScreenWidth/2 + BarWidth/5, BarHeight)
            };

            AddChild(_goldLabel);
        }

        private void AddLevelLabel() {
            _levelLabel = new CCLabelTtf("LVL", "kongtext", 12) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorMiddleBottom,
                Position = new CCPoint(Settings.ScreenWidth/2, BarHeight)
            };

            AddChild(_levelLabel);
        }

        private void AddHpLabel() {
            _hpLabel = new CCLabelTtf("HP", "kongtext", 8) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorMiddleBottom,
                Position = new CCPoint(Settings.ScreenWidth/2, BarHeight*0.25f)
            };
            AddChild(_hpLabel);
        }

        protected override void Draw() {
            base.Draw();
            var gameScene = Parent as GameScene;
            Player player = gameScene.MyPlayer;
            if (player != null){
                DrawUnderHud();
                DrawHpBar(player.Hp.Perc);
            }

            if (gameScene.GameLayer.MyAccountState != null)
                DrawExpBar(gameScene.GameLayer.MyAccountState.Exp.Perc);
        }

        private void DrawUnderHud() {
            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawSolidRect(new CCPoint(Settings.ScreenWidth/2 - BarWidth/2, BarHeight),
                new CCPoint(Settings.ScreenWidth/2 + BarWidth/2, BarHeight*2f), new CCColor4B(0, 0, 0, 0.4f));


            // Draw a circle about level label

            CCDrawingPrimitives.DrawSolidCircle(new CCPoint(Settings.ScreenWidth / 2, BarHeight), 25, 0, 12, new CCColor4B(0.2f, 0.2f, 0.2f, 1f));

            CCDrawingPrimitives.End();
        }

        private void DrawExpBar(float perc) {
            CCDrawingPrimitives.Begin();

            CCDrawingPrimitives.DrawSolidRect(new CCPoint(Settings.ScreenWidth/2 - BarWidth/2, 0),
                new CCPoint(Settings.ScreenWidth/2 + BarWidth/2, BarHeight*0.25f), CCColor4B.Black);
            CCDrawingPrimitives.DrawSolidRect(
                new CCPoint(Settings.ScreenWidth/2 - BarWidth/2 + (BarWidth*(1 - perc)/2), 0),
                new CCPoint(Settings.ScreenWidth/2 + BarWidth/2*perc, BarHeight*0.25f), CCColor4B.Green);

            CCDrawingPrimitives.End();
        }

        private void DrawHpBar(float perc) {
            CCDrawingPrimitives.Begin();

            CCDrawingPrimitives.DrawSolidRect(new CCPoint(Settings.ScreenWidth/2 - BarWidth/2, BarHeight*0.25f),
                new CCPoint(Settings.ScreenWidth/2 + BarWidth/2, BarHeight), CCColor4B.Black);
            CCDrawingPrimitives.DrawSolidRect(
                new CCPoint(Settings.ScreenWidth/2 - BarWidth/2 + (BarWidth*(1 - perc)/2), BarHeight*0.25f),
                new CCPoint(Settings.ScreenWidth/2 + BarWidth/2*perc, BarHeight), CCColor4B.Red);

            CCDrawingPrimitives.End();
        }

        public override void Update(float dt) {
            base.Update(dt);

            _fps = 1/dt;
            _step++;
            var gameScene = Parent as GameScene;
            if (gameScene != null && gameScene.GameLayer.MyAccountState != null){
                int gold = gameScene.GameLayer.MyAccountState.Gold;
                if (_lastGold < gold){
                    _lastGold += 4;
                }
                else{
                    _lastGold = gold;
                }
            }

            _label.Text = "FPS: " + (int) _fps + " Step: " + _step + " Gold: " + _lastGold;

            /** Updating HUD Labels */
            _goldLabel.Text = "Gold: " + _lastGold;

            if (gameScene.MyPlayer != null){
                _hpLabel.Text = string.Format("{0}/{1}", gameScene.MyPlayer.Hp.Current, gameScene.MyPlayer.Hp.Maximum);
            }

            if (gameScene.GameLayer.MyAccountState != null){
                _levelLabel.Text = gameScene.GameLayer.MyAccountState.Level.ToString();
            }
        }
    }
}