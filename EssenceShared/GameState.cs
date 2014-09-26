using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace EssenceShared {
    public class GameState {
        public List<PlayerState> players = new List<PlayerState>();
        public int playersCount = 0;

        public string Serialize() {
            // TODO: Вылетает, когда идет одновременно сериализация и изменение состояния
            var gsTemp = (GameState) MemberwiseClone();
            gsTemp.players = players.ToList();

            return JsonConvert.SerializeObject(gsTemp);
        }

        /** Здесь необходимо перезаписывать все изменяемые данные состояния игрового состояния 
         Метод выполняется на сервере */

        public void Deserialize(string json) {
            throw new NotImplementedException();
        }
    }
}