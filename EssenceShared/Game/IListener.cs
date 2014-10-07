using EssenceShared.Entities;

namespace EssenceShared.Game {
    public interface IListener {
        void Fire(EventType type, Entity from);
    }
}