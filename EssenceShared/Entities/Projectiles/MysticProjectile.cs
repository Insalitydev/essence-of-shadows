using System;
using CocosSharp;

namespace EssenceShared.Entities.Projectiles {
    public class MysticProjectile: Entity {
        private const float _speed = 350;
        private CCPoint _target;
        private float _angle;
        CCParticleMeteor Emmiter;
        

        public MysticProjectile(string id) : base(Resources.ProjectileMystic, id){
            Scale = 4;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

//            AddAction(new CCRepeat( new CCRotateBy(2, 170f) ));
            Schedule(Update);
            Schedule(Delete, 2);

            if (Parent.Tag == Settings.Client){
                
                Emmiter = new CCParticleMeteor(new CCPoint(6, 6));

                Emmiter.Gravity = GetNormalPointByDirection(Direction) * -2000;
                Emmiter.Scale = 0.08f;
                Emmiter.Texture = CCTextureCache.SharedTextureCache.AddImage(Resources.ParticleMysticProjectile);

                AddChild(Emmiter, -1);
            }
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
            if (Parent != null && Parent.Tag == Settings.Client){
                Color = new CCColor3B((byte)((PositionX + PositionY) * 2), (byte)((PositionX + PositionY) * 2), (byte)((PositionX+PositionY) * 2));

//                Rotation = PositionX*3;
            }
        }
    }
}