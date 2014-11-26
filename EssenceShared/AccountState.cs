using System;
using System.Collections.Generic;
using EssenceShared.Entities.Players;
using EssenceShared.Game;
using EssenceShared.Scenes;
using Newtonsoft.Json;

namespace EssenceShared {
    public enum AcccountUpgrade {
        Hp,
        Attack,
        Speed,
        Class
    }

    public class AccountState {
        [JsonProperty(PropertyName = "A")] public int AttackLevel;
        [JsonProperty(PropertyName = "C")] public int ClassLevel;
        [JsonProperty(PropertyName = "E")] public Stat Exp;
        [JsonProperty(PropertyName = "G")] public int Gold;
        [JsonProperty(PropertyName = "i")] public string HeroId;
        [JsonProperty(PropertyName = "H")] public int HpLevel;
        [JsonProperty(PropertyName = "L")] public int Level;
        [JsonProperty(PropertyName = "M")] public Locations Location = Locations.Town;
        [JsonProperty(PropertyName = "S")] public int SpeedLevel;
        private Dictionary<Locations, GameLayer> _locations;
        [JsonProperty(PropertyName = "N")] public string nickname;


        public AccountState(string id, string nickname, Dictionary<Locations, GameLayer> locations) {
            this.nickname = nickname;
            HeroId = id;
            Gold = 0;
            Exp = new Stat(Settings.StartExp) {Current = 0};
            Level = 1;
            _locations = locations;

            HpLevel = 1;
            AttackLevel = 1;
            SpeedLevel = 1;
            ClassLevel = 1;
        }

        public bool Upgrade(AcccountUpgrade upgrade) {
            bool result = false;
            if (PayGold(GetUpgradeCost(upgrade))) {
                result = true;
                switch (upgrade) {
                    case AcccountUpgrade.Attack:
                        AttackLevel++;
                        break;
                    case AcccountUpgrade.Hp:
                        HpLevel++;
                        break;
                    case AcccountUpgrade.Speed:
                        SpeedLevel++;
                        break;
                    case AcccountUpgrade.Class:
                        ClassLevel++;
                        break;
                }
            }
            RecalcStats();
            return result;
        }

        public int GetUpgradeCost(AcccountUpgrade upgrade) {
            switch (upgrade) {
                case AcccountUpgrade.Attack:
                    return AttackLevel*750;
                case AcccountUpgrade.Hp:
                    return HpLevel*500;
                case AcccountUpgrade.Speed:
                    return SpeedLevel*1000;
                case AcccountUpgrade.Class:
                    return ClassLevel*5000;
            }
            return 0;
        }

        public bool PayGold(int count) {
            if (Gold >= count) {
                Gold -= count;
                return true;
            }
            return false;
        }

        public void SwitchLocation(Locations locationTo) {
            Player player = GetPlayer();
            _locations[Location].RemoveChild(player);
            Location = locationTo;
            _locations[locationTo].AddEntity(player);

            EosEvent.RaiseEvent(player, new EventArgs(), EventType.ChangeLocation);
        }

        public void SetLocationsDict(Dictionary<Locations, GameLayer> locations) {
            _locations = locations;
        }

        /// <summary>
        ///     Вызывается классом Player
        /// </summary>
        public void Update() {
            if (Exp.Perc == 1) {
                LevelUp();
            }
        }

        public void RecalcStats() {
            Player player = GetPlayer();
            player.Hp.Maximum = Player.BaseHp + 30*(Level - 1) + 20*(HpLevel - 1);
            player.AttackDamage = Player.BaseAd + 3*(Level - 1) + 2*(AttackLevel - 1);
            player.Speed = Player.BaseSpeed + 20*(SpeedLevel - 1);
        }

        private void LevelUp() {
            Level++;
            Exp.Current = 0;
            Exp.Maximum = (int) (Settings.ExpMultiplier*Exp.Maximum);

            RecalcStats();
        }

        private Player GetPlayer() {
            var pl = _locations[Location].FindEntityById(HeroId) as Player;
            return pl;
        }

        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }
    }
}