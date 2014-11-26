using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocosSharp;
using EssenceServer.Scenes;
using EssenceShared;
using EssenceShared.Entities.Players;
using Newtonsoft.Json;

namespace EssenceServer {
    /// <summary>
    ///     Обрабатывает игровую логику на сервере. Содержит Сцену с игровыми объектами
    /// </summary>
    internal class ServerGame : CCApplicationDelegate {
        public List<AccountState> AccountsAll;
        public ServerScene ServerScene { get; private set; }

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow) {
            base.ApplicationDidFinishLaunching(application, mainWindow);

            /** Set up resource folders */
            Resources.LoadContent(application);

            ServerScene = new ServerScene(mainWindow);
            mainWindow.RunWithScene(ServerScene);
            LoadAccounts();

            ServerScene.Schedule(SaveAccounts, 5);
        }

        /// <summary>
        ///     Загружает из файла все аккаунты
        /// </summary>
        private void LoadAccounts() {
            using (FileStream fs = File.Open("Accounts.save", FileMode.OpenOrCreate, FileAccess.Read)) {
                var sr = new StreamReader(fs);

                string data = sr.ReadToEnd();
                AccountsAll = JsonConvert.DeserializeObject<List<AccountState>>(data);

                if (AccountsAll == null) {
                    AccountsAll = new List<AccountState>();
                }
                Log.Print(string.Format("Loaded {0} accounts", AccountsAll.Count));
            }
        }

        /// <summary>
        ///     Сохраняет всю информацию о всех аккаунтах в файл
        /// </summary>
        private void SaveAccounts(float dt) {
            using (var sw = new StreamWriter("Accounts.save", false)) {
                string data = JsonConvert.SerializeObject(AccountsAll);
                sw.Write(data);
            }
        }

        public Player GetPlayer(string id) {
            return ServerScene.GetPlayer(id);
        }

        public void AddNewPlayer(string id, string nickname, int x, int y) {
            Log.Print("Spawn player " + id);

            string type = "Mystic";
            switch (new Random().Next(3)) {
                case 0:
                    type = "Reaper";
                    break;
                case 1:
                    type = "Sniper";
                    break;
            }

            // Если нет аккаунта в базе, то создаем, иначе грузим
            AccountState accState;
            int ind = AccountsAll.FindIndex(acc => acc.nickname == nickname);
            if (ind == -1) {
                Log.Print("Creating new account: " + nickname, LogType.Info);
                accState = new AccountState(id, nickname, ServerScene.LocationsDict);
                AccountsAll.Add(accState);
            }
            else {
                //TODO: Переделать...
                AccountsAll[ind].SetLocationsDict(ServerScene.LocationsDict);
                AccountsAll[ind].HeroId = id;
                accState = AccountsAll[ind];
            }

            var player = new Player(id, type, accState) {
                PositionX = x,
                PositionY = y
            };

            ServerScene.GetGameLayer(accState.Location).AddEntity(player);
            ServerScene.Accounts.Add(accState);
            accState.RecalcStats();
        }

        public void RemovePlayer(string id) {
            try {
                Player pl = GetPlayer(id);
                if (pl != null)
                    pl.Remove();
                if (ServerScene.Accounts.Any(x => x.HeroId == id)) {
                    ServerScene.Accounts.Remove(ServerScene.Accounts.Single(x => x.HeroId == id));
                }
            }
            catch (NullReferenceException e) {
                Log.Print("Player " + id + " not found in the Game", LogType.Error);
            }
            catch (InvalidOperationException) {
                Log.Print("Account " + id + " already not it the Game", LogType.Network);
            }
        }
    }
}