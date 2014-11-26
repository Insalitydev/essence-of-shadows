using System;
using EssenceShared.Entities;

namespace EssenceShared.Game {
    public enum EventType {
        CreatureDied,
        PlayerShoot,
        PlayerDied,
        ChangeLocation,
        PlayerUpgrade
    }

    public class EosEvent {
        public static event EventHandler ChangeLocation;
        public static event EventHandler PlayerUpgrade;

        public static void RaiseEvent(Entity obj, EventArgs eventArgs, EventType eventType) {
            try {
                switch (eventType) {
                    case EventType.ChangeLocation:
                        ChangeLocation(obj, eventArgs);
                        break;
                    case EventType.PlayerUpgrade:
                        PlayerUpgrade(obj, eventArgs);
                        break;
                }
            }
            catch (NullReferenceException) {
                Log.Print("NullReference in RaiseEvent", LogType.Error);
            }
        }
    }

    public class UpgradeEventArgs : EventArgs {
        public AcccountUpgrade Upgrade;

        public UpgradeEventArgs(AcccountUpgrade upg) {
            Upgrade = upg;
        }
    }
}