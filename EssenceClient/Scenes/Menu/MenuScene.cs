using System;
using CocosSharp;
using EssenceClient.Scenes.Menu;

namespace EssenceClient.Scenes.Game {
    internal class MenuScene: CCScene {
        private MenuBackgroundLayer _backgroundLayer;
        private MenuLayer _menuLayer;

        public MenuScene(CCWindow window): base(window) {
            SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;

            _backgroundLayer = new MenuBackgroundLayer();
            AddChild(_backgroundLayer);

            _menuLayer = new MenuLayer();
            AddChild(_menuLayer);

            var keyListener = new CCEventListenerKeyboard();
            keyListener.OnKeyReleased = OnKeyReleased;

            AddEventListener(keyListener, this);
        }


        private void OnKeyReleased(CCEventKeyboard e) {
            if (e.Keys == CCKeys.Enter || e.Keys == CCKeys.Space ) {
                _menuLayer.StartGame(new Object());
            }

            if (e.Keys == CCKeys.Escape) {
                _menuLayer.ExitGame(new Object());
            }
        }
    }
}