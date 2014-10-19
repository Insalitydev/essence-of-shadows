using System;
using System.Collections.Generic;
using EssenceShared.Entities.Players;
using EssenceShared.Game;
using EssenceShared.Scenes;
using Newtonsoft.Json;

namespace EssenceShared {
    public class AccountState: IDisposable {
        public Stat Exp;
        public int Gold;
        public string HeroId;
        public int Level;
        public Locations Location = Locations.Town;
        private Dictionary<Locations, GameLayer> _locations;

        public string nickname;


        public AccountState(string id, string nickname, Dictionary<Locations, GameLayer> locations) {
            this.nickname = nickname;
            HeroId = id;
            Gold = 0;
            Exp = new Stat(Settings.StartExp) {Current = 0};
            Level = 1;
            _locations = locations;
        }

        public void Dispose() {
            throw new NotImplementedException();
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
            if (Exp.Perc == 1){
                LevelUp();
            }
        }

        public void RecalcStats() {
            Player player = GetPlayer();
            player.Hp.Maximum = Player.BaseHP + 30*Level;
            player.AttackDamage = Player.BaseAD + 3*Level;
        }

        private void LevelUp() {
            Level++;
            Exp.Current = 0;
            Exp.Maximum = (int) (Settings.ExpMultiplier*Exp.Maximum);

            // Inc stats:
            RecalcStats();
        }

        private Player GetPlayer() {
            var pl = _locations[Location].FindEntityById(HeroId) as Player;
            return pl;
        }

        public static AccountState LoadAccountState(string nickname) {
            //TODO: грузить из базы
            throw new NotImplementedException();
        }

        public AccountState SaveAccountState() {
            //TODO: записывать в базу
            throw new NotImplementedException();
        }

        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }
    }
}