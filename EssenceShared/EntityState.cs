﻿using System;
using EssenceShared.Entities;
using EssenceShared.Game;
using Newtonsoft.Json;

namespace EssenceShared {
    public class EntityState {
        // Ниже все параметры, которые мы передаем для сущностей
        // TODO: Обновлять каждый раз не все параметры
        // TODO: по возможности перейти на messagePack (бинарный json)
        // TODO: сократить имена переменных для меньшего объема пакетов
        [JsonProperty(PropertyName = "A")] public ActionState ActionState;
        [JsonProperty(PropertyName = "D")] public int AttackDamage;
        [JsonProperty(PropertyName = "Di")] public float Direction;
        [JsonProperty(PropertyName = "F")] public bool FlipX;
        [JsonProperty(PropertyName = "H")] public Stat Hp;
        [JsonProperty(PropertyName = "X")] public float PositionX;
        [JsonProperty(PropertyName = "Y")] public float PositionY;
        [JsonProperty(PropertyName = "S")] public float Scale;
        [JsonProperty(PropertyName = "V")] public float Speed;
        [JsonProperty(PropertyName = "T")] public int Tag;
        [JsonProperty(PropertyName = "Tx")] public string TextureName;

        public EntityState(string id) {
            Id = id;
        }

        [JsonProperty(PropertyName = "I")]
        public string Id { get; private set; }

        public static EntityState ParseEntity(Entity entity) {
            var es = new EntityState(entity.Id) {
                PositionX = entity.PositionX,
                PositionY = entity.PositionY,
                FlipX = entity.FlipX,
                Speed = entity.Speed,
                Direction = entity.Direction,
                AttackDamage = entity.AttackDamage,
                ActionState = entity.ActionState,
                Tag = entity.Tag,
                Scale = entity.ScaleX,
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
            if (Math.Abs(entity.PositionX - es.PositionX) > 4) {
                entity.PositionX = es.PositionX;
            }
            if (Math.Abs(entity.PositionY - es.PositionY) > 4) {
                entity.PositionY = es.PositionY;
            }
            entity.FlipX = es.FlipX;
            entity.Tag = es.Tag;
            entity.Scale = es.Scale;
            entity.Speed = es.Speed;
            entity.AttackDamage = es.AttackDamage;
            entity.Direction = es.Direction;
            entity.Hp = es.Hp;
            entity.ActionState = es.ActionState;
        }

        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }
    }
}