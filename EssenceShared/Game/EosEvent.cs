using System;
using EssenceShared.Entities;
using EssenceShared.Entities.Players;

namespace EssenceShared.Game {
    public enum EventType {
        CreatureDied,
        PlayerShoot,
        PlayerDied,
        ChangeLocation
    }

    public class EosEvent {
        public static event EventHandler ChangeLocation;

        public static void RaiseEvent(Entity obj, EventArgs eventArgs, EventType eventType) {
            switch (eventType){
                case EventType.ChangeLocation:
                    ChangeLocation(obj, eventArgs);
                    break;
            }
        }
    }

}