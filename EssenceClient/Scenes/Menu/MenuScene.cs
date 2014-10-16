using System;
using CocosSharp;

namespace EssenceClient.Scenes.Menu {
    internal class MenuScene: CCScene {
        private readonly MenuLayer _menuLayer;
        private MenuBackgroundLayer _backgroundLayer;   

        public MenuScene(CCWindow window): base(window) {
            SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;

            _backgroundLayer = new MenuBackgroundLayer();
            AddChild(_backgroundLayer);

            _menuLayer = new MenuLayer();
            AddChild(_menuLayer);

            var keyListener = new CCEventListenerKeyboard {OnKeyReleased = OnKeyReleased};

            AddEventListener(keyListener, this);
        }


        private void OnKeyReleased(CCEventKeyboard e) {
            if (e.Keys == CCKeys.Enter || e.Keys == CCKeys.Space){
                _menuLayer.StartGame(new Object());
            }

            if (e.Keys == CCKeys.Escape){
                _menuLayer.ExitGame(new Object());
            }
        }
    }
}