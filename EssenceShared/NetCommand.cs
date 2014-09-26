using System;

namespace EssenceShared {
    enum NetCommandType {
        SAY,
        UPDATE_GAMESTATE,
        UPDATE_PLAYERSTATE,
        CONNECT,
        DISCONNECT
    }
    public class NetCommand {
        private NetCommandType type;
        private string data;
        private DateTime createTime;
    }
}