using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared.Entities.Players;
using EssenceShared.Game;

namespace EssenceShared.Entities.Enemies {
    public abstract class Enemy: Entity {
        protected Entity Target;
        protected int SightRadius;
        protected int Speed;

        public Enemy(string url, string id): base(url, id) {
            Target = null;
            SightRadius = 500;
            Speed = 150;
            Scale = Settings.Scale;
            Tag = Tags.Enemy;
            Hp = new Stat(100);
        }

        public override void OnEnter() {
            base.OnEnter();
            if (Parent.Tag == Tags.Server){
                Schedule(Action);
            }
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.PlayerProjectile){
                var player = other.GetOwner() as Player;
                if (player != null)
                    Damage(player.AttackDamage);
            }
        }

        /// <summary>
        ///     Возвращает список игроков. Сортирует в порядке близости к себе
        /// </summary>
        protected List<Player> GetPlayers() {
            return
                Parent.Children.Where(x=>x.Tag == Tags.Player)
                    .OrderBy(x=>DistanceTo(x.Position))
                    .Cast<Player>()
                    .ToList();
        }

        private void Damage(int p) {
            Hp.Current -= p;
            if (Hp.Perc == 0){
                Schedule(Die, 0.01f);
            }
        }

        protected virtual void Die(float dt) {
            Remove();
        }

        /// <summary>
        ///     Метод ИИ, решает что делать в каждый момент времени
        /// </summary>
        protected virtual void Action(float dt) {
        }

        protected override void Draw() {
            base.Draw();
            drawHealthBar();
        }

        private void drawHealthBar() {
            CCDrawingPrimitives.Begin();

            CCDrawingPrimitives.DrawSolidRect(new CCPoint(0, 0), new CCPoint(Texture.PixelsWide, -2), CCColor4B.Black);
            CCDrawingPrimitives.DrawSolidRect(new CCPoint(0, 0), new CCPoint(Texture.PixelsWide*Hp.Perc, -2),
                CCColor4B.Red);

            CCDrawingPrimitives.End();
        }
    }
}