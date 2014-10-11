using System;
using CocosSharp;
using EssenceShared.Game;
using EssenceShared.Scenes;

namespace EssenceShared.Entities {
    public enum ActionState {
        Idle,
        MoveToAttack,
        Attack,
        Died
    }

    /// <summary>
    ///     Базовый класс для всех игровых объектов
    /// </summary>
    public class Entity: CCSprite {
        public ActionState ActionState;
        public int AttackDamage;
        public float Direction;
        public Stat Hp;
        public string OwnerId = null;
        public float Speed;
        private int _imageH;
        private int _imageW;

        public Entity(string url, string id): base(url) {
            Id = id;
            IsAntialiased = false;
            Direction = 0;
            Speed = 0;
            Tag = Tags.Unknown;
            Hp = new Stat(0);
            AttackDamage = 0;
            ActionState = ActionState.Idle;
        }


        /// <summary>
        ///     По этой маске проверяется столкновение между объектами
        /// </summary>
        public CCRect Mask { get; private set; }

        public string Id { get; private set; }

        protected override void AddedToScene() {
            base.AddedToScene();

            Schedule(Update);
        }

        public override void Update(float dt) {
            base.Update(dt);

            UpdateMask();
        }


        /// <summary>
        ///     Пытается предугадать движение
        ///     Вызывается на клиенте для сглаживания движения
        ///     TODO: не работает
        /// </summary>
        protected void PredictMove() {
            //            var deltaPos = Position - _lastPos;
            //            if (deltaPos.Length > 4){
            //                Position += deltaPos;
            //                Log.Print("Predicted");
            //            }
        }

        public Entity GetOwner() {
            if (Parent != null && (Parent as GameLayer) != null)
                return (Parent as GameLayer).FindEntityById(OwnerId);
            return null;
        }

        /// <summary>
        ///     Метод вызывается при столкновении двух объектов.
        /// </summary>
        /// <param name="other"> Объект с которым произошло столкновение </param>
        public virtual void Collision(Entity other) {
        }

        protected void UpdateMask() {
            // TODO: можно ли без пересоздавааний?
            // lesser - немного уменьшаем маску столкновения
            const int lesser = 12;
            _imageW = (int) (Texture.PixelsWide*ScaleX) - lesser;
            _imageH = (int) (Texture.PixelsHigh*ScaleY) - lesser;
            Mask = new CCRect(PositionX - _imageW/2 + lesser/2, PositionY - _imageH/2 + lesser/2, _imageW, _imageH);
        }

        internal void AppendState(EntityState es) {
            EntityState.AppendStateToEntity(this, es);
        }


        protected void MoveToTarget(CCPoint target, float speed) {
            MoveByAngle(AngleTo(target), speed);
        }

        protected void MoveFromTarget(CCPoint target, float speed) {
            MoveByAngle(AngleTo(target) + 180, speed);
        }

        protected void MoveByAngle(float angle, float speed) {
            PositionX += speed*(float) Math.Cos(ToRadians(angle));
            PositionY += speed*(float) Math.Sin(ToRadians(angle));
        }

        public float AngleTo(CCPoint p) {
            return AngleBetweenPoints(Position, p);
        }

        public float DistanceTo(CCPoint p) {
            return DistanceBetweenPoints(Position, p);
        }

        public float DistanceTo(Entity ent) {
            return DistanceBetweenPoints(Position, ent.Position);
        }


        public static float ToRadians(float angle) {
            return (float) ((Math.PI/180)*angle);
        }

        public static float AngleBetweenPoints(CCPoint p1, CCPoint p2) {
            float deltaX = p1.Y - p2.Y;
            float deltaY = p1.X - p2.X;
            // angle in radian:
            var angle = (float) (Math.Atan2(deltaY, deltaX));

            // angle in degree
            angle = (float) (angle*180/(Math.PI));

            // to correct angle (0 - right, 90 - top
            angle = 270 + angle*-1;
            angle %= 360;

            if (angle < 0){
                angle += 360;
            }

            return angle;
        }

        public static float DistanceBetweenPoints(CCPoint p1, CCPoint p2) {
            return (float) (Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow(p2.Y - p1.Y, 2)));
        }

        public static CCPoint GetNormalPointByDirection(float dir) {
            var x = (float) Math.Cos(ToRadians(dir));
            var y = (float) Math.Sin(ToRadians(dir));
            return new CCPoint(x, y);
        }


        /// <summary>
        ///     Удаляет объект со сцены у своего родителя
        /// </summary>
        public void Remove(bool cleanup = true) {
            if (Parent != null){
                Parent.RemoveChild(this, cleanup);
            }
        }
    }
}