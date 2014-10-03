using CocosSharp;
using EssenceShared;
using Microsoft.Xna.Framework.Content;

namespace EssenceClient.Scenes.Game {
    internal class HudLayer: CCLayer {
        private float _fps;
        private CCLabelTtf _label;
        private int _lastGold;

        private CCLabelTtf _hpLabel;
        private CCLabelTtf _expLabel;
        private CCLabelTtf _levelLabel;
        private CCLabelTtf _goldLabel;

        private const int BarWidth = 400;
        private const int BarHeight = 24;

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
//            AddExpLabel();
            AddLevelLabel();
            AddGoldLabel();

            Schedule(Update);
        }

        
        private void AddGoldLabel() {
            _goldLabel = new CCLabelTtf("Gold:", "kongtext", 10) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorLowerLeft,
                Position = new CCPoint(Settings.ScreenWidth / 2 + BarWidth/4, BarHeight)
            };

            AddChild(_goldLabel);
        }

        private void AddLevelLabel() {
            _levelLabel = new CCLabelTtf("LVL", "kongtext", 12) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorMiddleBottom,
                Position = new CCPoint(Settings.ScreenWidth / 2, BarHeight)
            };

            AddChild(_levelLabel);
        }

        private void AddExpLabel() {
            _expLabel = new CCLabelTtf("exp", "kongtext", 14) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorMiddleBottom,
                Position = new CCPoint(Settings.ScreenWidth / 2, 0)
            };
            AddChild(_expLabel);
        }

        private void AddHpLabel() {
            _hpLabel = new CCLabelTtf("HP", "kongtext", 10) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorMiddleBottom,
                Position = new CCPoint(Settings.ScreenWidth / 2, BarHeight*0.25f)
            };
            AddChild(_hpLabel);
        }

        protected override void Draw() {
            base.Draw();
            var gameScene = Parent as GameScene;
            var player = gameScene.MyPlayer;
            if (player != null) {
                DrawHpBar(player.Hp.GetPerc());
                DrawExpBar();
                DrawUnderHud();
            }
        }

        private void DrawUnderHud() {
            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawSolidRect(new CCPoint(Settings.ScreenWidth / 2 - BarWidth / 2, BarHeight), new CCPoint(Settings.ScreenWidth / 2 + BarWidth / 2, BarHeight * 2f), new CCColor4B(0, 0, 0, 0.4f));

            CCDrawingPrimitives.End();
        }

        private void DrawExpBar() {
            CCDrawingPrimitives.Begin();

            CCDrawingPrimitives.DrawSolidRect(new CCPoint(Settings.ScreenWidth / 2 - BarWidth / 2, 0), new CCPoint(Settings.ScreenWidth / 2 + BarWidth / 2, BarHeight * 0.25f), CCColor4B.Black);
            CCDrawingPrimitives.DrawSolidRect(new CCPoint(Settings.ScreenWidth / 2 - BarWidth / 2, 0), new CCPoint(Settings.ScreenWidth / 2 + BarWidth / 2 - 30, BarHeight * 0.25f), CCColor4B.Green);

            CCDrawingPrimitives.End();
        }

        private void DrawHpBar(float perc) {
            CCDrawingPrimitives.Begin();

            CCDrawingPrimitives.DrawSolidRect(new CCPoint(Settings.ScreenWidth / 2 - BarWidth / 2, BarHeight * 0.25f), new CCPoint(Settings.ScreenWidth / 2 + BarWidth / 2, BarHeight), CCColor4B.Black);
            CCDrawingPrimitives.DrawSolidRect(new CCPoint(Settings.ScreenWidth / 2 - BarWidth / 2 + (BarWidth * (1-perc)/2), BarHeight * 0.25f), new CCPoint(Settings.ScreenWidth / 2 + BarWidth / 2 * perc, BarHeight), CCColor4B.Red);


            CCDrawingPrimitives.End();
        }

        public override void Update(float dt) {
            base.Update(dt);

            _fps = 1/dt;
            _step++;
            int gold = 10;
            var gameScene = Parent as GameScene;
            if (gameScene != null && gameScene.GameLayer.MyAccountState != null){
                gold = gameScene.GameLayer.MyAccountState.Gold;
                if (_lastGold < gold){
                    _lastGold += 4;
                }
                else{
                    _lastGold = gold;
                }
            }
            _label.Text = "FPS: " + (int) _fps + " Step: " + _step + " Gold: " + _lastGold;

            _goldLabel.Text = "Gold: " + _lastGold.ToString();
            if (gameScene.MyPlayer != null) {
                _hpLabel.Text = string.Format("{0}/{1}", gameScene.MyPlayer.Hp.Current, gameScene.MyPlayer.Hp.Maximum);
            }

            if (gameScene.GameLayer.MyAccountState != null) {
//                _expLabel.Text = string.Format("{0}/{1}",gameScene.GameLayer.MyAccountState.Exp, "unknown");
                _levelLabel.Text = gameScene.GameLayer.MyAccountState.Level.ToString();
            }
        }
    }
}