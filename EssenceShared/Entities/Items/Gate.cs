using CocosSharp;
using EssenceShared.Entities.Players;
using EssenceShared.Game;

namespace EssenceShared.Entities {
    public class Gate: Entity {
        public Locations TeleportTo = Locations.Town;

        public Gate(string id): base(Resources.ItemGate, id) {
            Tag = Tags.Item;
            _maskH = _maskH/4;
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Player){
                (other as Player).AccState.SwitchLocation(TeleportTo);
            }
        }
    }
}