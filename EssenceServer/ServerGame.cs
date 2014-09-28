using CocosSharp;
using EssenceServer.Scenes;
using EssenceShared;

namespace EssenceServer {
    /** Обрабатывает всю игровую логику */

    internal class ServerGame: CCApplicationDelegate {
        public ServerScene GameScene { get; private set; }

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow) {
            base.ApplicationDidFinishLaunching(application, mainWindow);


            /** Set up resource folders */
            Log.Print("Loading Resources");
            Resources.LoadContent(application);

            GameScene = new ServerScene(mainWindow);
            mainWindow.RunWithScene(GameScene);
        }

        public void AddNewPlayer(string id, int x, int y, string type) {
            Log.Print("Spawn player " + id);

            var es = new EntityState(id) {PositionX = x, PositionY = y, TextureName = type};

            GameScene.GameLayer.AddEntity(es);
        }
    }
}