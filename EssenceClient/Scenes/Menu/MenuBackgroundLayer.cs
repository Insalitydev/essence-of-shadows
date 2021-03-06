﻿using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;

namespace EssenceClient.Scenes.Menu {
    internal class MenuBackgroundLayer : CCLayerColor {
        public MenuBackgroundLayer() {
            Color = CCColor3B.Yellow;
            Opacity = 20;
        }

        protected override void AddedToScene() {
            base.AddedToScene();


            for (int i = 0; i < Settings.ScreenWidth; i += 10) {
                AddChild(new FlyingSquare(i));
                AddChild(new FlyingSquare(i));
            }

            AddTitle();
            AddCardinalImage(0, 0);
            AddCardinalImage((int) Settings.ScreenWidth, 0);
            AddCardinalImage((int) Settings.ScreenWidth, (int) Settings.ScreenHeight);
            AddCardinalImage(0, (int) Settings.ScreenHeight);
        }

        private void AddCardinalImage(int p1, int p2) {
            var tmp = new CCSprite(Resources.BossCardinal) {
                Texture = {IsAntialiased = false},
                PositionX = p1,
                PositionY = p2,
                Scale = 6
            };

            var blades = new CCSprite(Resources.BossCardinalBlades) {
                Scale = 1,
                IsAntialiased = false,
                PositionX = tmp.Texture.PixelsWide/2,
                PositionY = tmp.Texture.PixelsHigh/2
            };

            blades.AddAction(new CCRepeatForever(new CCRotateBy(0.7f, -120)));
            tmp.AddChild(blades, -1);

            if (tmp.PositionX > Settings.ScreenWidth/2) {
                tmp.FlipX = true;
            }

            AddChild(tmp);
        }

        private void AddTitle() {
            var title = new CCLabelTtf(Settings.GameName, "kongtext", 28) {
                Color = CCColor3B.White,
                AnchorPoint = CCPoint.AnchorMiddleTop,
                PositionX = Settings.ScreenWidth/2,
                PositionY = 550,
            };

            var titleShadow = new CCLabelTtf(Settings.GameName, "kongtext", 28) {
                Color = new CCColor3B(100, 0, 220),
                AnchorPoint = CCPoint.AnchorMiddleTop,
                PositionX = Settings.ScreenWidth/2,
                PositionY = 550,
            };

            // Движение тени у текста с названием игры
            const int moveStreak = 30;
            var moves = new CCFiniteTimeAction[moveStreak];
            for (int i = 0; i < moveStreak; i++) {
                moves[i] = MoveAround();
            }
            titleShadow.RepeatForever(new CCSequence(moves));


            var helper = new CCLabelTtf("Enter/Space to start, Esc to exit", "kongtext", 10) {
                Color = CCColor3B.Gray,
                AnchorPoint = CCPoint.AnchorMiddleBottom,
                PositionX = Settings.ScreenWidth/2,
                PositionY = 0
            };

            AddChild(titleShadow);

            AddChild(title);
            AddChild(helper);
        }

        private CCMoveBy MoveAround() {
            return new CCMoveTo(0.1f,
                new CCPoint(Settings.ScreenWidth/2, 550) + Entity.GetNormalPointByDirection(CCRandom.Next(360))*8);
        }
    }

    internal class FlyingSquare : CCNode {
        private readonly int h;
        private readonly int speed;
        private readonly int w;

        public FlyingSquare(int x) {
            PositionX = x;
            PositionY = CCRandom.Next(-20, (int) Settings.ScreenHeight + 20);

            w = CCRandom.Next(10, 45);
            h = (int) (w*(CCRandom.NextDouble() + 1));
            speed = w*4;
        }

        public override void OnEnter() {
            base.OnEnter();

            Schedule(Update);
        }

        public override void Update(float dt) {
            base.Update(dt);

            PositionY += speed*dt;
            if (PositionY > Settings.ScreenHeight) {
                PositionY = -h;
            }
        }

        protected override void Draw() {
            base.Draw();

            CCDrawingPrimitives.Begin();

            CCDrawingPrimitives.DrawSolidRect(Position, new CCPoint(PositionX + w, PositionY + h),
                new CCColor4B(255, 255, 0, 0.07f));
            CCDrawingPrimitives.End();
        }
    }
}