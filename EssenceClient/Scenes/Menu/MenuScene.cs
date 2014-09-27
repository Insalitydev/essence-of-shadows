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
        }
    }
}