using CocosSharp;

namespace EssenceShared.Entities.Projectiles {
    public class MysticProjectile: Entity {
        private const float _speed = 450;
        private float _angle;
        private CCPoint _target;


        public MysticProjectile(string id): base(Resources.ProjectileMystic, id) {
            Scale = 4;
            Tag = Tags.Projectile;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            //            AddAction(new CCRepeat( new CCRotateBy(2, 170f) ));
            Schedule(Update);
            Schedule(Delete, 2);

            if (Parent.Tag == Tags.Client){
               
            }
        }

        public override void OnEnter() {
            base.OnEnter();


            if (Parent != null && Parent.Tag == Tags.Client){
                var Emmiter = new CCParticleMeteor(new CCPoint(6, 6)) {
                    Gravity = GetNormalPointByDirection(Direction)*-2000,
                    Scale = 0.08f,
                    SpeedVar = 200,
                    Texture = CCTextureCache.SharedTextureCache.AddImage(Resources.ParticleMysticProjectile)
                };
                AddChild(Emmiter, -10);
            }

            
        }

        public override void Update(float dt) {
            base.Update(dt);

            MoveByAngle(Direction, _speed*dt);

            if (Parent != null && Parent.Tag == Tags.Client){
                UpdateAnimation(dt);
            }
        }

        public void Delete(float dt) {
            RemoveAllChildren(true);
            Remove();
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Enemy){

                Remove();
            }
        }

        public void UpdateAnimation(float dt) {
            Color = new CCColor3B((byte) ((PositionX + PositionY)*2), (byte) ((PositionX + PositionY)*2),
                (byte) ((PositionX + PositionY)*2));
        }
    }
}