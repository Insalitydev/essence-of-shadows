using CocosSharp;

namespace EssenceShared.Entities.Objects {
    public class Smith : Entity {
        public Smith(string id) : base(Resources.ObjectSmith, id) {
            Tag = Tags.Object;

        }

        public override void OnEnter() {
            base.OnEnter();

            if (Parent.Tag == Tags.Client) {
                var label = new CCLabelTtf("Store", "kongtext", 8) {
                    Color = CCColor3B.Black,
                    PositionY = 30,
                    PositionX = -5,
                };

                AddChild(label, 10);
            }
        }
    }
}