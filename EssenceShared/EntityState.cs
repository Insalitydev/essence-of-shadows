using System;
using EssenceShared.Entities;
using Newtonsoft.Json;

namespace EssenceShared {
    public class EntityState {

        public string Id { get; private set; }

        public float PositionX;
        public float PositionY;
        public float Scale;
        public string TextureName;

        public EntityState(string id) {
            Id = id;
        }

        public static EntityState ParseEntity(Entity entity) {
            var es = new EntityState(entity.Id) {
                PositionX = entity.PositionX,
                PositionY = entity.PositionY,
                Scale = 4,
                TextureName = entity.Texture.Name.ToString()
            };
            return es;
        }

        public static Entity CreateEntity(EntityState es) {
            var entity = new Entity(es.TextureName, es.Id);

            AppendStateToEntity(entity, es);

            return entity;
        }

        public static void AppendStateToEntity(Entity entity, EntityState es) {
            entity.PositionX = es.PositionX;
            entity.PositionY = es.PositionY;
            entity.Scale = es.Scale;
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