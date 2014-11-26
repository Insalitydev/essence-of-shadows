using CocosSharp;
using EssenceShared.Game;
using EssenceShared.Scenes;

namespace EssenceShared.Entities.Objects {
    public class Smith : Entity {
        private readonly CCLabelTtf labelAttack = Util.GetSmallLabelHint("Damage");
        private readonly CCLabelTtf labelClass = Util.GetSmallLabelHint("Class");
        private readonly CCLabelTtf labelHp = Util.GetSmallLabelHint("Vitality");
        private readonly CCLabelTtf labelSpeed = Util.GetSmallLabelHint("Speed");

        private CCMenuItem upgradeAttack;
        private CCMenuItem upgradeClass;
        private CCMenuItem upgradeHp;
        private CCMenuItem upgradeSpeed;

        public Smith(string id) : base(Resources.ObjectSmith, id) {
            Tag = Tags.Object;
        }

        public override void OnEnter() {
            base.OnEnter();

            if (Parent.Tag == Tags.Client) {
                var label = new CCLabelTtf("Store", "kongtext", 8) {
                    Color = CCColor3B.White,
                    PositionY = 30,
                    PositionX = -5,
                    IsAntialiased = false
                };

                AddChild(label, 10);
            }

            // adding shop GUI
            upgradeHp = new CCMenuItemImage(Resources.GuiIncrease, Resources.GuiIncreaseActive,
                Resources.GuiIncreaseInactive, UpgradeHp);
            upgradeHp.AddChild(labelHp);

            upgradeAttack = new CCMenuItemImage(Resources.GuiIncrease, Resources.GuiIncreaseActive,
                Resources.GuiIncreaseInactive, UpgradeAttack);
            upgradeAttack.AddChild(labelAttack);

            upgradeSpeed = new CCMenuItemImage(Resources.GuiIncrease, Resources.GuiIncreaseActive,
                Resources.GuiIncreaseInactive, UpgradeSpeed);
            upgradeSpeed.AddChild(labelSpeed);

            upgradeClass = new CCMenuItemImage(Resources.GuiIncrease, Resources.GuiIncreaseActive,
                Resources.GuiIncreaseInactive, UpgradeClass);
            upgradeClass.AddChild(labelClass);

            var upgradeMenu = new CCMenu(upgradeHp, upgradeAttack, upgradeSpeed, upgradeClass) {
                Position = new CCPoint(70, 30),
            };
            upgradeMenu.AlignItemsVertically();

            AddChild(upgradeMenu);
        }

        public void UpdateLabels() {
            if (Parent.Tag == Tags.Client) {
                Log.Print("updates");
                AccountState ac = (Parent as GameLayer).MyAccountState;
                labelAttack.Text = "Damage: " + ac.GetUpgradeCost(AcccountUpgrade.Attack);
                labelHp.Text = "Vitality: " + ac.GetUpgradeCost(AcccountUpgrade.Hp);
                labelSpeed.Text = "Speed: " + ac.GetUpgradeCost(AcccountUpgrade.Speed);
                labelClass.Text = "Class: " + ac.GetUpgradeCost(AcccountUpgrade.Class);
                labelAttack.IsAntialiased = false;
                labelHp.IsAntialiased = false;
                labelSpeed.IsAntialiased = false;
                labelClass.IsAntialiased = false;

                if (ac.GetUpgradeCost(AcccountUpgrade.Attack) >= ac.Gold)
                    upgradeAttack.Enabled = false;
                else
                    upgradeAttack.Enabled = true;

                if (ac.GetUpgradeCost(AcccountUpgrade.Hp) >= ac.Gold)
                    upgradeHp.Enabled = false;
                else
                    upgradeHp.Enabled = true;

                if (ac.GetUpgradeCost(AcccountUpgrade.Class) >= ac.Gold)
                    upgradeClass.Enabled = false;
                else
                    upgradeClass.Enabled = true;

                if (ac.GetUpgradeCost(AcccountUpgrade.Speed) >= ac.Gold)
                    upgradeSpeed.Enabled = false;
                else
                    upgradeSpeed.Enabled = true;
            }
        }

        private void UpgradeHp(object obj) {
            EosEvent.RaiseEvent(this, new UpgradeEventArgs(AcccountUpgrade.Hp), EventType.PlayerUpgrade);
        }

        private void UpgradeAttack(object obj) {
            EosEvent.RaiseEvent(this, new UpgradeEventArgs(AcccountUpgrade.Attack), EventType.PlayerUpgrade);
        }

        private void UpgradeSpeed(object obj) {
            EosEvent.RaiseEvent(this, new UpgradeEventArgs(AcccountUpgrade.Speed), EventType.PlayerUpgrade);
        }

        private void UpgradeClass(object obj) {
            EosEvent.RaiseEvent(this, new UpgradeEventArgs(AcccountUpgrade.Class), EventType.PlayerUpgrade);
        }
    }
}