using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace EssenceShared {
    public class GameState {
        public List<EntityState> Entities = new List<EntityState>();
        public AccountState Account;

        public string Serialize() {
            // TODO: Вылетает, когда идет одновременно сериализация и изменение состояния
            // TODO: Возможно исправленно из за ToList();
            var gsTemp = (GameState) MemberwiseClone();
            gsTemp.Entities = Entities.ToList();

            return JsonConvert.SerializeObject(gsTemp);
        }

        /** Здесь необходимо перезаписывать все изменяемые данные состояния игрового состояния 
         Метод выполняется на сервере */

        public void Deserialize(string json) {
            throw new NotImplementedException();
        }
    }
}