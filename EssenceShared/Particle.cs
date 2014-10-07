using CocosSharp;
using EssenceShared.Entities;

namespace EssenceShared {
    public enum ParticleType {
        LevelUp,
        ProjectileTrail
    }

    public class Particle {
        /** owner - ссылается на объект, кто создавал эмитер. Если указан, эмитер следует за ним*/

        public static CCParticleSystem GetEmiter(string texture, ParticleType type, int dieAfter, CCPoint where,
            Entity owner = null) {
            CCPoint pos;
            if (owner != null)
                pos = new CCPoint(owner.Texture.PixelsWide/2, owner.Texture.PixelsHigh/2);
            else
                pos = where;


            CCParticleSystem emiter = new CCParticleMeteor(pos);

            switch (type){
                case ParticleType.ProjectileTrail:
                    emiter = new CCParticleMeteor(pos) {
                        Scale = 0.1f,
                        SpeedVar = 250,
                        Texture = CCTextureCache.SharedTextureCache.AddImage(texture)
                    };
                    if (owner != null)
                        emiter.Gravity = Entity.GetNormalPointByDirection(owner.Direction)*-2000;

                    break;
                case ParticleType.LevelUp:
                    emiter = new CCParticleSun(pos) {
                        Scale = 0.1f,
                        SpeedVar = 150,
                        Texture = CCTextureCache.SharedTextureCache.AddImage(texture)
                    };
                    break;
            }

            emiter.ScheduleOnce((float x)=>emiter.RemoveFromParent(), dieAfter);

            return emiter;
        }
    }
}