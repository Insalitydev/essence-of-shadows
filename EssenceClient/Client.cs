using CocosSharp;
using EssenceClient.Scenes.Menu;
using EssenceShared;

namespace EssenceClient {
    /** Инициализирует игру и запускает её */

    internal class Client: CCApplicationDelegate {
        private CCScene _startScene;

        /** Вызывается после загрузки приложения */

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow) {
            base.ApplicationDidFinishLaunching(application, mainWindow);

            mainWindow.SetDesignResolutionSize(Settings.ScreenWidth, Settings.ScreenHeight, CCSceneResolutionPolicy.ShowAll);

            Resources.LoadContent(application);
            mainWindow.DisplayStats = true;
            mainWindow.AllowUserResizing = false;

            _startScene = new MenuScene(mainWindow);
            mainWindow.RunWithScene(_startScene);
        }
    }
}