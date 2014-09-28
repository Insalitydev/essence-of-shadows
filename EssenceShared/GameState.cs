using System;
using System.Collections.Generic;
using System.Linq;
using EssenceShared.Entities;
using Newtonsoft.Json;

namespace EssenceShared {
    public class GameState {
        public List<EntityState> entities = new List<EntityState>(); 

        public string Serialize() {
            // TODO: Вылетает, когда идет одновременно сериализация и изменение состояния
            // TODO: Возможно исправленно из за ToList();
            var gsTemp = (GameState) MemberwiseClone();
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