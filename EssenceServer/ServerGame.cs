using CocosSharp;
using EssenceServer.Scenes;
using EssenceShared;

namespace EssenceServer {
    /** Обрабатывает всю игровую логику */

    internal class ServerGame: CCApplicationDelegate {

        private ServerScene _gameScene;

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow) {
            base.ApplicationDidFinishLaunching(application, mainWindow);


            /** Set up resource folders */
            // TODO: убрать дублирование кода тут и на стороне клиента
            Log.Print("Loading Resources");
            Resources.LoadContent(application);

            _gameScene = new ServerScene(mainWindow);
            mainWindow.RunWithScene(_gameScene);
        }

        public void AddNewPlayer(ulong id, int x, int y) {
            Log.Print("Spawn player " + id);
            _gameScene.AddNewPlayer(id, x, y);
        }

        internal string GetGameState() {
            return _gameScene.GameState.Serialize();
        }

        internal void UpdateGameState(PlayerState ps) {
            _gameScene.UpdateGameState(ps);
        }
    }
}