using System;
using Newtonsoft.Json;

namespace EssenceShared {
    public class AccountState {
        public int Exp;
        public int Gold;
        public string HeroId;

        public AccountState(string id) {
            HeroId = id;
            Gold = 0;
            Exp = 0;
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