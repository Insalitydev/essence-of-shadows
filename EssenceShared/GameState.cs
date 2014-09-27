using System;
using System.Collections.Generic;
using System.Linq;
using EssenceShared.Entities;
using Newtonsoft.Json;

namespace EssenceShared {
    public class GameState {
        public List<PlayerState> players = new List<PlayerState>();
        public List<EntityState> entities = new List<EntityState>(); 
        public int playersCount = 0;

        public string Serialize() {
            // TODO: Вылетает, когда идет одновременно сериализация и изменение состояния
            var gsTemp = (GameState) MemberwiseClone();
            gsTemp.players = players.ToList();
            gsTemp.entities = entities.ToList();

            return JsonConvert.SerializeObject(gsTemp);
        }

        /** Здесь необходимо перезаписывать все изменяемые данные состояния игрового состояния 
         Метод выполняется на сервере */

        public void Deserialize(string json) {
            throw new NotImplementedException();
        }
    }
}