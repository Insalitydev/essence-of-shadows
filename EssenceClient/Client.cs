using CocosSharp;
using EssenceClient.Scenes.Game;
using EssenceShared;

namespace EssenceClient {
    /** Инициализирует игру и запускает её */

    internal class Client: CCApplicationDelegate {
        public static CCSize DefaultResoultion;
        private CCScene _startScene;

        /** Вызывается после загрузки приложения */

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow) {
            base.ApplicationDidFinishLaunching(application, mainWindow);
            mainWindow.SetDesignResolutionSize(Settings.SCREEN_WIDTH, Settings.SCREEN_HEIGHT,
                CCSceneResolutionPolicy.ShowAll);

            Resources.LoadContent(application);

            // TODO: нужен ли defaultResolution?
            _startScene = new MenuScene(mainWindow);
            mainWindow.RunWithScene(_startScene);
        }
    }
}