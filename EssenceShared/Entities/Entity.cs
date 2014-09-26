using CocosSharp;

namespace EssenceShared.Entities {
    /** Основной класс для всех игровых объектов */

    public class Entity: CCSprite {
        public string Id { get; private set; }

        public Entity(string url, string id): base(url) {
            this.Id = id;
            this.IsAntialiased = false;
        }
    }
}