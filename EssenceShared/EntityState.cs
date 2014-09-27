using System;
using EssenceShared.Entities;
using Newtonsoft.Json;

namespace EssenceShared {
    public class EntityState {

        public string Id { get; private set; }

        public float PositionX;
        public float PositionY;
        public string TextureName;

        public EntityState(string id) {
            Id = id;
        }

        public static EntityState ParseEntity(Entity entity) {
            var es = new EntityState(entity.Id);
            es.PositionX = entity.PositionX;
            es.PositionY = entity.PositionY;
            es.TextureName = entity.Texture.Name.ToString();
            return es;
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