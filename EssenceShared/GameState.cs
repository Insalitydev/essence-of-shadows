using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
namespace EssenceShared {
    public class GameState {
        public int playersCount = 0;
        public List<PlayerState> players = new List<PlayerState>(); 

        public string Serialize() {
            // TODO: Вылетает, когда идет одновременно сериализация и изменение состояния
            var gsTemp = new GameState();

            return JsonConvert.SerializeObject(gsTemp);
        }

        /** Здесь необходимо перезаписывать все изменяемые данные состояния игрового состояния 
         Метод выполняется на сервере */

        public void Deserialize(string json) {
            throw new NotImplementedException();
        }
    }
}