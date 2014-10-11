using EssenceShared.Entities.Players;
using EssenceShared.Game;

namespace EssenceShared.Entities {
    public class Gate: Entity {
        public Locations teleportTo = Locations.Desert;
        public Gate(string id): base(Resources.ItemGate, id) {
            Tag = Tags.Item;

        }

        public override void Collision(Entity other) {
            base.Collision(other);

            // TODO: реализовать нормально...
            if (other.Tag == Tags.Player){
                (other as Player).accState.SwitchLocation(teleportTo);

            }
        }
    }
}
