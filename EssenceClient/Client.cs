using CocosSharp;
using EssenceClient.Scenes.Menu;
using EssenceShared;

namespace EssenceClient {
    /// <summary>
    ///   /  Основной класс клиента. Запускает игру
    /// </summary>
    internal class Client: CCApplicationDelegate {
        private static CCWindow sharedWindow;
        private CCScene _startScene;


        /// <summary>
        ///     Метод вызывается после загрузки приложения
        /// </summary>
        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow) {
            base.ApplicationDidFinishLaunching(application, mainWindow);
            sharedWindow = mainWindow;

            mainWindow.SetDesignResolutionSize(Settings.ScreenWidth, Settings.ScreenHeight,
                CCSceneResolutionPolicy.ShowAll);

            Resources.LoadContent(application);
            mainWindow.DisplayStats = true;
            mainWindow.AllowUserResizing = false;


            _startScene = new MenuScene(mainWindow);
            mainWindow.RunWithScene(_startScene);
        }
    }
}
