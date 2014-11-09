using System;
using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Objects;
using EssenceShared.Entities.Players;
using EssenceShared.Game;
using EssenceShared.Scenes;
using IniParser;
using IniParser.Model;
using Lidgren.Network;

namespace EssenceClient.Scenes.Game {
    /// <summary>
    ///     Основная сцена для игры на клиенте. Создает игровой слой и управляет общим состоянием игры для клиента
    /// </summary>
    internal class GameScene: CCScene {
        private const int CameraDelta = 4;
        private readonly ChatLayer _chatLayer;
        private readonly NetGameClient _netGameClient;
        private BackgroundLayer _backgroundLayer;
        private int _cameraHeight = 900;
        private int _cameraX;
        private int _cameraY;
        private HudLayer _hudLayer;
        private int _mousePosX;
        private int _mousePosY;
        private int _sightRadius = 900;

        public GameScene(CCWindow window): base(window) {
            Id = "";

            _backgroundLayer = new BackgroundLayer();
            AddChild(_backgroundLayer);

            var cameraVisibleBounds = new CCSize(Settings.ScreenWidth, Settings.ScreenHeight);
            var camera = new CCCamera(CCCameraProjection.Projection3D, cameraVisibleBounds,
                new CCPoint3(Settings.ScreenWidth, Settings.ScreenHeight, 10));

            GameLayer = new GameLayer {
                Tag = Tags.Client,
                Camera = camera,
            };
            AddChild(GameLayer);

            _chatLayer = new ChatLayer();
            AddChild(_chatLayer);

            _hudLayer = new HudLayer();
            AddChild(_hudLayer);


            var keyListener = new CCEventListenerKeyboard {OnKeyPressed = OnKeyPressed, OnKeyReleased = OnKeyReleased};
            AddEventListener(keyListener, this);

            var mouseListener = new CCEventListenerMouse {
                OnMouseDown = OnMouseDown,
                OnMouseUp = OnMouseUp,
                OnMouseMove = OnMouseScroll
            };
            AddEventListener(mouseListener, this);

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("Config.ini");
            string nickname = data["Client"]["nickname"];
            _netGameClient = new NetGameClient(data["Server"]["ip"], this);
            _netGameClient.ConnectToServer(nickname);

            InitEvents();

            Schedule(Update);
            Schedule(UpdateNetwork, Settings.NetworkFreqUpdate);
        }

        public Player MyPlayer { get; private set; }

        public string Id { get; private set; }

        public GameLayer GameLayer { get; private set; }

        private void InitEvents() {
            EosEvent.PlayerUpgrade += (sender, args)=>CallPlayerUpgrade((UpgradeEventArgs) args);
        }

        private void CallPlayerUpgrade(UpgradeEventArgs args) {
            string command = "";
            switch (args.Upgrade){
                case AcccountUpgrade.Attack:
                    command = "upgradeAttack";
                    break;
                case AcccountUpgrade.Hp:
                    command = "upgradeHp";
                    break;
                case AcccountUpgrade.Speed:
                    command = "upgradeSpeed";
                    break;
                case AcccountUpgrade.Class:
                    command = "upgradeClass";
                    break;
            }
            var nc = new NetCommand(NetCommandType.CallPlayerMethod, command);
            _netGameClient.Send(nc, NetDeliveryMethod.ReliableOrdered);
        }


        public override void Update(float dt) {
            base.Update(dt);
            
            UpdateControl(dt);
            UpdateCamera();
            UpdateVisibility();
        }

        private void UpdateControl(float dt) {
            if (MyPlayer != null){
                MyPlayer.Control(dt);
                // поправка на камеру:
                if (GameLayer.Camera != null){
                    var camx = (int) (GameLayer.Camera.TargetInWorldspace.X - Settings.ScreenWidth/2);
                    var camy = (int) (GameLayer.Camera.TargetInWorldspace.Y - Settings.ScreenHeight/2);

                    if (Input.IsMousePressed(CCMouseButton.LeftButton) && MyPlayer.AttackCooldownCounter == 0){
                        // Стреляем при нажатой левой кнопке
                        var nc = new NetCommand(NetCommandType.CallPlayerMethod,
                            "attack." + (_mousePosX + camx) + "." + (_mousePosY + camy));
                        _netGameClient.Send(nc, NetDeliveryMethod.ReliableOrdered);
                        MyPlayer.AttackCooldownCounter = MyPlayer.AttackCooldown;

                        Log.Print(GameLayer.TileAt(_mousePosX + camx, _mousePosY + camy));
                    }
                }
            }
        }

        public void UpdateNetwork(float dt) {
            base.Update(dt);

            UpdateMyState();
        }

        /// <summary>
        ///     Помечает объекты, которые находятся все зоны видимости, скрытыми.
        ///     Позволяет не тратить на них ресурсы при отрисовке
        ///     Выстраивает порядок отрисовки объектам
        /// </summary>
        private void UpdateVisibility() {
            if (MyPlayer != null)
                foreach (CCNode ccNode in GameLayer.Children){
                    ccNode.Visible = MyPlayer.DistanceTo(ccNode.Position) < _sightRadius;

                    if (ccNode is CCSprite){
                        var ccSp = ccNode as CCSprite;
                        ccNode.ZOrder =
                            (int) (Settings.ScreenHeight - ccSp.PositionY + (ccSp.Texture.PixelsHigh*ccSp.ScaleY)/2);

                        if (ccNode is Entity){
                            var ccEntity = ccNode as Entity;
                            ccNode.ZOrder += (int)(ccEntity.Height * ccEntity.ScaleY);
                        }

                        if (ccNode.Tag == Tags.MapTile){
                            ccNode.ZOrder -= Settings.TileSize*6;
                        }
                    }
                }
        }

        /// <summary>
        ///     Обеспечивает работу камеры: Слежение за игроком и позицию над полем игры
        /// </summary>
        private void UpdateCamera() {
            if (Input.IsKeyIn(CCKeys.O)){
                _cameraHeight -= 3;
            }
            if (Input.IsKeyIn(CCKeys.P)){
                _cameraHeight += 3;
            }
            if (MyPlayer != null){
                // псевдо-три-дэ
                if (Math.Abs(_cameraX - MyPlayer.PositionX) > 400) _cameraX = (int) MyPlayer.PositionX;
                if (Math.Abs(_cameraY - MyPlayer.PositionY) > 400) _cameraY = (int) MyPlayer.PositionY;
                if (_cameraX < MyPlayer.PositionX - CameraDelta) _cameraX += 8;
                if (_cameraX > MyPlayer.PositionX + CameraDelta) _cameraX -= 8;
                if (_cameraY < MyPlayer.PositionY - CameraDelta) _cameraY += 8;
                if (_cameraY > MyPlayer.PositionY + CameraDelta) _cameraY -= 8;
                GameLayer.Camera.CenterInWorldspace = new CCPoint3(_cameraX, _cameraY, _cameraHeight);
                //                GameLayer.Camera.TargetInWorldspace = new CCPoint3(MyPlayer.Position.X * 2 - _cameraX, MyPlayer.Position.Y * 2 - _cameraY, 0);
                GameLayer.Camera.TargetInWorldspace = new CCPoint3(MyPlayer.Position, 0);
            }
        }


        /// <summary>
        ///     Формирует состояние своего игрока и отсылает его на сервер
        ///     Если игрок клиента не найден - ищет его
        /// </summary>
        private void UpdateMyState() {
            if (MyPlayer != null){
                EntityState myps = EntityState.ParseEntity(MyPlayer);

                var nc = new NetCommand(NetCommandType.UpdatePlayerstate, myps.Serialize());
                _netGameClient.Send(nc, NetDeliveryMethod.Unreliable);
            }
            else{
                Entity myPl = GameLayer.FindEntityById(Id);
                if (myPl != null){
                    MyPlayer = (Player) myPl;
                }
            }
        }

        public void SetMyId(string newId) {
            Log.Print("Set new Id: " + newId);
            Id = newId;
        }

        public void AppendChatMessage(string msg) {
            _chatLayer.Messages.Add(msg);
        }

        private void OnKeyPressed(CCEventKeyboard e) {
            Input.OnKeyPress(e.Keys);
        }

        private void OnKeyReleased(CCEventKeyboard e) {
            Input.OnKeyRelease(e.Keys);

            if (e.Keys == CCKeys.C){
                _netGameClient.SendChatMessage("Hello!");
            }

            if (e.Keys == CCKeys.X){
                var nc = new NetCommand(NetCommandType.CallPlayerMethod, "attack.100.100");
                _netGameClient.Send(nc, NetDeliveryMethod.ReliableOrdered);
            }

            if (e.Keys == CCKeys.Escape){
                NetGameClient.Client.Shutdown("Exit by client");
                Window.DefaultDirector.PopScene();
            }

            if (e.Keys == CCKeys.T){
                Console.WriteLine(MyPlayer.Mask.ToString());
                Console.WriteLine(GameLayer.MyAccountState.HpLevel);
                Console.WriteLine(GameLayer.MyAccountState.AttackLevel);
                Console.WriteLine(GameLayer.MyAccountState.SpeedLevel);
                Console.WriteLine(GameLayer.MyAccountState.ClassLevel);
            }
        }

        private void OnMouseDown(CCEventMouse e) {
            Input.OnMousePress(e.MouseButton);
        }

        private void OnMouseUp(CCEventMouse e) {
            Input.OnMouseRelease(e.MouseButton);
        }

        private void OnMouseScroll(CCEventMouse e) {
            /** get scale coef.*/
            float windowScaleX = Window.WindowSizeInPixels.Width/Settings.ScreenWidth;
            float windowScaleY = Window.WindowSizeInPixels.Height/Settings.ScreenHeight;

            /** Актуальные координаты */
            _mousePosX = (int) (e.CursorX/windowScaleX);
            _mousePosY = (int) (e.CursorY/windowScaleY);
        }
    }
}