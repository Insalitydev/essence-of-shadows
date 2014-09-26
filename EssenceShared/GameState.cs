using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace EssenceShared {
    public class GameState {
        public int playersCount = 0;
        public List<PlayerState> players = new List<PlayerState>(); 

        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }

        /** Здесь необходимо перезаписывать все изменяемые данные состояния игрового состояния 
         Метод выполняется на сервере */

        public void Deserialize(string json) {
            throw new NotImplementedException();
        }
    }
}