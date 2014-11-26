using System;
using CocosSharp;
using EssenceClient.Scenes.Game;
using EssenceShared;

namespace EssenceClient.Scenes.Menu {
    internal class MenuLayer : CCLayer {
        private CCMenu MainMenu;

        protected override void AddedToScene() {
            base.AddedToScene();

            var _menuPos = new CCPoint(Settings.ScreenSize.Center);

            CCLabelTtf labelStartGame = GetMenuLabel("Start Game");
            CCMenuItem itemStart = new CCMenuItemLabelTTF(labelStartGame, StartGame);

            CCLabelTtf labelExit = GetMenuLabel("Exit Game");
            CCMenuItem itemExit = new CCMenuItemLabelTTF(labelExit, ExitGame);


            MainMenu = new CCMenu(itemStart, itemExit) {Position = _menuPos};
            MainMenu.AlignItemsVertically(15);

            AddChild(MainMenu);
        }

        public void StartGame(object obj) {
            Log.Print("Starting game");
            Window.DefaultDirector.PushScene(new GameScene(Window));
        }

        public void ExitGame(object obj) {
            Environment.Exit(0);
        }

        private CCLabelTtf GetMenuLabel(string text) {
            var label = new CCLabelTtf(text, "kongtext", 18) {
                Color = new CCColor3B(180, 180, 250)
            };

            return label;
        }
    }
}