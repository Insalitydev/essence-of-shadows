using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared;

namespace EssenceClient.Scenes.Game {
    internal class ChatLayer: CCLayerColor {
        private const int ShowLastMessages = 4;
        private const int IndentBetweenMessages = 10;
        public List<string> Messages;
        private CCLabelTtf _label;

        public ChatLayer() {
            Layer.Scale = 1f;
            Messages = new List<string>();

//            AnchorPoint = CCPoint.AnchorLowerLeft;
            PositionX = Settings.ScreenWidth/2 - 250;
            PositionY = 40;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            _label = new CCLabelTtf("Chat", "kongtext", 8) {
                Color = CCColor3B.White,
                Position = new CCPoint(0, 0)
            };

            AddChild(_label);

            Schedule(Update);
        }

        public override void Update(float dt) {
            base.Update(dt);

            if (Messages.Count > 0){
                _label.Text = Messages.Last();
            }
        }
    }
}