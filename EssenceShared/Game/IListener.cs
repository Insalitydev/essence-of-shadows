using EssenceShared.Entities;

namespace EssenceShared.Game {
    public interface IListener {
        void Fire(EosEvent type, Entity from);
    }
}