using System;
using EssenceShared.Entities;
using EssenceShared.Game;
using Newtonsoft.Json;

namespace EssenceShared {
    public class EntityState {
        public float Direction;
        public Stat Hp;
        public float PositionX;
        public float PositionY;
        public float Scale;
        //        public string OwnerId;
        public int Tag;
        public string TextureName;

        public EntityState(string id) {
            Id = id;
        }

        public string Id { get; private set; }

        public static EntityState ParseEntity(Entity entity) {
            var es = new EntityState(entity.Id) {
                PositionX = entity.PositionX,
                PositionY = entity.PositionY,
                Direction = entity.Direction,
                //                OwnerId = entity.OwnerId,
                Tag = entity.Tag,
                Scale = 4,
                TextureName = entity.Texture.Name.ToString(),
                Hp = new Stat(entity.Hp.Maximum) {
                    Current = entity.Hp.Current,
                }
            };
            return es;
        }

        public static Entity CreateEntity(EntityState es) {
            var entity = new Entity(es.TextureName, es.Id);

            AppendStateToEntity(entity, es);

            return entity;
        }

        public static void AppendStateToEntity(Entity entity, EntityState es) {
            // погрешность в ~ 3 пикселя не правим
            if (Math.Abs(entity.PositionX - es.PositionX) > 4)
                entity.PositionX = es.PositionX;
            if (Math.Abs(entity.PositionY - es.PositionY) > 4)
                entity.PositionY = es.PositionY;

            entity.Tag = es.Tag;
            entity.Scale = es.Scale;
            //            entity.OwnerId = es.OwnerId;
            entity.Direction = es.Direction;
            entity.Hp = es.Hp;
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