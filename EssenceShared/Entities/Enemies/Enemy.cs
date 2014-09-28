using System;
using System.Runtime.Remoting.Channels;
using CocosSharp;
using EssenceShared.Entities.Projectiles;

namespace EssenceShared.Entities.Enemies {
    public class Enemy : Entity {

        public Enemy(string id): base(Resources.ItemChest, id) {
            Scale = 4;
        }
    }
}
