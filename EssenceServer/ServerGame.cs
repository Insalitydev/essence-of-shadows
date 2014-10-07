using System;
using System.Linq;
using CocosSharp;
using EssenceServer.Scenes;
using EssenceShared;
using EssenceShared.Entities.Players;

namespace EssenceServer {
    /// <summary>
    ///     Обрабатывает игровую логику на сервере. Содержит Сцену с игровыми объектами
    /// </summary>
    internal class ServerGame: CCApplicationDelegate {
        public ServerScene ServerScene { get; private set; }

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow) {
            base.ApplicationDidFinishLaunching(application, mainWindow);

            /** Set up resource folders */
            Resources.LoadContent(application);

            ServerScene = new ServerScene(mainWindow);
            mainWindow.RunWithScene(ServerScene);
        }

        public void AddNewPlayer(string id, int x, int y, string type) {
            Log.Print("Spawn player " + id);

            var accState = new AccountState(id, ServerScene.GameLayer);
            var player = new Player(id, type, accState) {
                PositionX = x,
                PositionY = y
            };
            ServerScene.GameLayer.AddEntity(player);
            ServerScene.Accounts.Add(accState);
        }

        public void RemovePlayer(string id) {
            try{
                var pl = ServerScene.GameLayer.Entities.Find(x=>x.Id == id) as Player;
                if (pl != null)
                    pl.Remove();

                ServerScene.Accounts.Remove(ServerScene.Accounts.Single(x=>x.HeroId == id));
            }
            catch (NullReferenceException e){
                Log.Print("Player " + id + " not found in the Game", LogType.Error);
            }
        }
    }
}