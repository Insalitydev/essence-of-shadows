using System;
using Newtonsoft.Json;

namespace EssenceShared {
    public class EntityState {

        public string Id { get; private set; }

        public float PositionX;
        public float PositionY;
        public string Name;

        public EntityState(string id) {
            Id = id;
        }

        /** Пакует все необходимые данные в строку json
         Метод выполняется на стороне клиента */

        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }

        /** Здесь необходимо перезаписывать все изменяемые данные состояния игрока 
         Метод выполняется на сервере */

        public void Deserialize(string json) {
            throw new NotImplementedException();
        }
    }
}