using CocosSharp;

namespace EssenceShared.Entities {
    /** Основной класс для всех игровых объектов */

    public class Entity: CCSprite {
        public int ID { get; private set; }

        public Entity(string url, int ID): base(url) {
            this.ID = ID;
        }
    }
}