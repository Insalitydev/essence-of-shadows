using CocosSharp;

namespace EssenceShared.Entities.Projectiles {
    public class MysticProjectile: Entity {
        private const float _speed = 350;
        private CCPoint _target;
        private float _angle;

        public MysticProjectile(string id) : base(Resources.ProjectileMystic, id){
            Scale = 4;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

//            AddAction(new CCRepeat( new CCRotateBy(2, 170f) ));
            Schedule(Update);
            Schedule(Delete, 2);
        }

        public override void Update(float dt) {
            base.Update(dt);

            UpdateAnimation(dt);
            MoveByAngle(Direction, _speed*dt);
            
        }

        public void Delete(float dt) {
            this.Remove();
        }


        public void UpdateAnimation(float dt) {
            if (Parent.Tag == Settings.Client){
                Color = new CCColor3B((byte)((PositionX + PositionY) * 2), (byte)((PositionX + PositionY) * 2), (byte)((PositionX+PositionY) * 2));

                Rotation = PositionX*3;
            }
        }
    }
}