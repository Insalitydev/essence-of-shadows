using CocosSharp;

namespace EssenceShared.Entities {
    /** Основной класс для всех игровых объектов */

    public class Entity: CCSprite {
        public string Id { get; private set; }

        public Entity(string url, string id): base(url) {
            Id = id;
            IsAntialiased = false;
        }

        internal void AppendState(EntityState es) {
            EntityState.AppendStateToEntity(this, es);
        }
    }
}