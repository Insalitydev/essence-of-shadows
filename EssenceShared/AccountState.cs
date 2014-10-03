using System;
using EssenceShared.Game;
using Newtonsoft.Json;

namespace EssenceShared {
    public class AccountState {
        public Stat Exp;
        public int Gold;
        public int Level;
        public string HeroId;

        public AccountState(string id) {
            HeroId = id;
            Gold = 0;
            Exp = new Stat(1000);
            Exp.Current = 0;
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
            Exp.Maximum = (int)(1.5f * Exp.Maximum);
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