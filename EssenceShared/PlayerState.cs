using System;
using EssenceShared.Entities.Player;
using Newtonsoft.Json;

namespace EssenceShared {
    public class PlayerState {

        public string Id { get; private set; }

        public float PositionX;
        public float PositionY;
        public string TextureName;


        public PlayerState(string id) {
            Id = id;
        }

        public static PlayerState ParsePlayer(Player pl) {
            var ps = new PlayerState(pl.Id);
            ps.PositionX = pl.PositionX;
            ps.PositionY = pl.PositionY;
            ps.TextureName = pl.Texture.Name.ToString();
            return ps;
        }

        /** Пакует все необходимые данные в строку json
         Метод выполняется на стороне клиента */

        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }

        /** Здесь необходимо перезаписывать все изменяемые данные состояния игрока 
         Метод выполняется на сервере */

        public void Deserialize(string json) {
            throw new NotImplementedException();
        }
    }
}