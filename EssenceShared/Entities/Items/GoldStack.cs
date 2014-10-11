using EssenceShared.Entities.Players;

namespace EssenceShared.Entities {
    public class GoldStack: Entity {
        public GoldStack(string id): base(Resources.ItemGold, id) {
            Tag = Tags.Item;

        }

        public override void Collision(Entity other) {
            base.Collision(other);
            Log.Print("colliti");

            if (other.Tag == Tags.Player){
                (other as Player).accState.Gold += 100;
                Remove();
            }
        }
    }
}
