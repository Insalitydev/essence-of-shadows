using CocosSharp;

namespace EssenceShared.Entities.Items {
    public class HealPot: Entity {
        private float HealPerc = 0.1f;
        public HealPot(string id): base(Resources.ItemHealpot, id) {
            Tag = Tags.Item;
            Scale = 3;
        }

        public override void OnEnter() {
            base.OnEnter();

            var move = new CCActionEase(new CCMoveBy(1, new CCPoint(0, 20)));
            
            var moveSeq = new CCSequence(move, move.Reverse());
            AddAction(new CCRepeatForever(moveSeq));
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Player){
                other.Hp.Perc += HealPerc;
                Remove();
            }

            
        }
    }
}