using CocosSharp;
using EssenceServer.Scenes;
using EssenceShared;
using EssenceShared.Entities.Player;

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

        public void AddNewPlayer(string id, int x, int y) {
            Log.Print("Spawn player " + id);

            var es = new EntityState(id);
            es.PositionX = x;
            es.PositionY = y;
            es.TextureName = "Mystic";

//            var pl = new Player(id);
//            EntityState.AppendStateToEntity(pl, es);

            GameScene._gameLayer.AddEntity(es);
        }
    }
}