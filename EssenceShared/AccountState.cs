using System;
using CocosSharp;
using EssenceShared.Entities.Players;
using EssenceShared.Game;
using EssenceShared.Scenes;
using Newtonsoft.Json;

namespace EssenceShared {
    public class AccountState {
        public Stat Exp;
        public int Gold;
        public string HeroId;
        public int Level;

        [JsonIgnore] public GameLayer gameLayer;

        public AccountState(string id, GameLayer gameLayer) {
            HeroId = id;
            this.gameLayer = gameLayer;
            Gold = 0;
            Exp = new Stat(Settings.StartExp) {Current = 0};
            Level = 1;
        }

        /** Call by player */

        public void Update() {
            if (Exp.Perc == 1){
                LevelUp();
            }
        }

        private void LevelUp() {
            Level++;
            Exp.Current = 0;
            Exp.Maximum = (int) (Settings.ExpMultiplier*Exp.Maximum);

            // Inc stats:
            var player = GetPlayer();
            player.Hp.Maximum += 30;
            player.AttackDamage += 3;
            player.Hp.Current = GetPlayer().Hp.Maximum;
        }

        private Player GetPlayer() {
            var pl = gameLayer.FindEntityById(HeroId) as Player;
            return pl;
        }

        public static AccountState LoadAccountState(string AccountId) {
            //TODO: грузить из базы
            throw new NotImplementedException();
        }

        public AccountState SaveAccountState() {
            //TODO: записывать в базу
            throw new NotImplementedException();
        }

        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }
    }
}