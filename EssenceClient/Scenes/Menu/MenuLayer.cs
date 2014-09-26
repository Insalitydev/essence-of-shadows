using System;
using CocosSharp;
using EssenceClient.Scenes.Game;
using EssenceShared;
using IniParser;
using IniParser.Model;

namespace EssenceClient.Scenes.Menu {
    internal class MenuLayer: CCLayer {
        private CCMenu MainMenu;

        protected override void AddedToScene() {
            base.AddedToScene();

            var _menuPos = new CCPoint(Settings.SCREEN_SIZE.Center);
            // TODO: ItemLabel не работает :(
//            CCMenuItem itemStart = new CCMenuItemLabel(GetMenuLabel("STARTGAME"), StartGame);

            CCMenuItem itemStart = new CCMenuItemImage("StartGame.png", "StartGameOver.png", StartGame);
            CCMenuItem itemExit = new CCMenuItemImage("ExitGame.png", "ExitGameOver.png", ExitGame);


            MainMenu = new CCMenu(itemStart, itemExit);
            MainMenu.Position = _menuPos;
            MainMenu.AlignItemsVertically(10);

            Log.Print(MainMenu.ToString());

            AddChild(MainMenu);
//            AddChild(GetMenuLabel("TOOOSTOR"));
        }

        private void ExitGame(object obj) {
            Environment.Exit(0);
        }

        private void StartGame(object obj) {
            Log.Print("Starting game");
            Window.DefaultDirector.PushScene(new GameScene(Window));
        }

        private CCLabel GetMenuLabel(string text) {
            var label = new CCLabel(text, "kongtext", 20) {
                Color = CCColor3B.Green,
                Dimensions = ContentSize,
            };

            return label;
        }
    }
}