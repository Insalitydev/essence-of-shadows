﻿using EssenceShared.Entities.Players;

namespace EssenceShared.Entities.Projectiles {
    public class EnemyMeleeProjectile: Entity {
        public EnemyMeleeProjectile(int damage, string url, string id)
            : base(url, id) {
            Scale = Settings.Scale;
            Tag = Tags.EnemyProjectile;
            AttackDamage = damage;
            Speed = 0;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            Schedule(Update);
            Schedule(Delete, 0.1f);
            Rotation = 360 - Direction;

            if (Parent.Tag == Tags.Server){
                MoveByAngle(Direction, Texture.PixelsWide*Settings.Scale);
            }
        }

        public void Delete(float dt) {
            RemoveAllChildren(true);
            Remove();
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Player){
                if (other as Player != null){
                    (other as Player).Hp.Current -= AttackDamage;
                }
            }
        }
    }
}