using System;
using Newtonsoft.Json;

namespace EssenceShared {
    public enum NetCommandType {
        SAY,
        UPDATE_GAMESTATE,
        UPDATE_PLAYERSTATE,
        CONNECT,
        DISCONNECT
    }

    public class NetCommand {
        private DateTime _createTime;
        public string Data { get; private set; }
        public NetCommandType Type { get; private set; }


        /** _data - строка в формате json с необходимыми полями для указанного типа комманды */

        public NetCommand(NetCommandType type, string data) {
            _createTime = DateTime.Now;
            Type = type;
            Data = data;
        }

        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }

        public static NetCommand Deserialize(string json) {
            return JsonConvert.DeserializeObject<NetCommand>(json);
        }
    }
}