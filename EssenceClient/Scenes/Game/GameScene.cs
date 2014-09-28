﻿using System.Linq;
using System.Runtime.Remoting.Channels;
using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Player;
using EssenceShared.Entities.Projectiles;
using EssenceShared.Scenes;
using IniParser;
using IniParser.Model;
using Lidgren.Network;

namespace EssenceClient.Scenes.Game {
    internal class GameScene: CCScene {
        private readonly NetGameClient netGameClient;
        private BackgroundLayer _backgroundLayer;
        private ChatLayer _chatLayer;
        private HudLayer _hudLayer;

        private Player myPlayer;

        public GameScene(CCWindow window): base(window) {
            Id = "888888888888888";

            _backgroundLayer = new BackgroundLayer();
            AddChild(_backgroundLayer);

            _gameLayer = new GameLayer();
            AddChild(_gameLayer);

            _chatLayer = new ChatLayer();
            AddChild(_chatLayer);

            _hudLayer = new HudLayer();
            AddChild(_hudLayer);

            var keyListener = new CCEventListenerKeyboard();
            keyListener.OnKeyPressed = OnKeyPressed;
            keyListener.OnKeyReleased = OnKeyReleased;

            AddEventListener(keyListener, this);

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Config.ini");
            netGameClient = new NetGameClient(data["Server"]["ip"], this);
            netGameClient.ConnectToServer();

            Schedule(UpdateNetwork, 0.03f);
            Schedule(Update);
        }

        public string Id { get; private set; }

        public GameLayer _gameLayer { get; private set; }

        public void UpdateNetwork(float dt) {
            base.Update(dt);
            UpdateMyState();
        }

        public override void Update(float dt) {
            base.Update(dt);

            if (myPlayer != null){
                myPlayer.Control(dt);
            }
        }

        public void SetMyId(string id) {
            Log.Print("Set new Id: " + id);
            Id = id;
        }

        public void getChatMessage(string msg) {
            _chatLayer.messages.Add(msg);
        }

        private void UpdateMyState() {
            
            if (myPlayer != null){
                var myps = EntityState.ParseEntity(myPlayer);

                var nc = new NetCommand(NetCommandType.UPDATE_PLAYERSTATE, myps.Serialize());
                netGameClient.Send(nc.Serialize(), NetDeliveryMethod.Unreliable);
            }
            else{
                netGameClient.Send("no ID", NetDeliveryMethod.Unreliable);
                Entity myPl = _gameLayer.FindEntityById(Id);
                if (myPl != null){
                    myPlayer = (Player) myPl;
                }
            }
        }

        internal void UpdateEntity(EntityState entity) {
//            Entity ent = new MysticProjectile(entity.Id, new CCPoint(0, 0));
//            ent.PositionX = entity.PositionX;
//            ent.PositionY = entity.PositionY;
//
//            _gameLayer.UpdateEntity(ent);
        }

        private void OnKeyPressed(CCEventKeyboard e) {
            Input.OnKeyPress(e.Keys);
        }

        private void OnKeyReleased(CCEventKeyboard e) {
            Input.OnKeyRelease(e.Keys);

            if (e.Keys == CCKeys.S){
                netGameClient.SendChatMessage("DASDA"+Id);
            }

            if (e.Keys == CCKeys.A) {
//                myPlayer.Attack(new CCPoint(0, 0));
                var nc = new NetCommand(NetCommandType.CALL_PLAYER_METHOD, "attack");
                netGameClient.Send(nc.Serialize(), NetDeliveryMethod.ReliableOrdered);
            }

            if (e.Keys == CCKeys.Escape){
                Window.DefaultDirector.PopScene();
            }

        }
    }
}