using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerraLeague.UI
{
    internal class HealthbarUI : UIState
    {
        public static bool visible = true;
        UIElement MainPanel;
        ResourceBar hp;
        ResourceBar mana;
        Texture2D _backgroundTexture;

        public override void OnInitialize()
        {
            if (_backgroundTexture == null)
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/BarBackground");

            MainPanel = new UIElement();
            MainPanel.SetPadding(0);
            MainPanel.Width.Set(500, 0f);
            MainPanel.Height.Set(52, 0f);
            MainPanel.Left.Set(Main.screenWidth / 2 - MainPanel.Width.Pixels / 2, 0f);
            MainPanel.Top.Set(Main.screenHeight - (MainPanel.Height.Pixels + 95), 0f);
            base.Append(MainPanel);

            hp = new ResourceBar(ResourceBarMode.HP, 20, 480);
            hp.Left.Set(10, 0f);
            hp.Top.Set(4f, 0f);
            MainPanel.Append(hp);

            mana = new ResourceBar(ResourceBarMode.MANA, 20, 480);
            mana.Left.Set(10, 0f);
            mana.Top.Set(24f, 0f);
            MainPanel.Append(mana);

            

            for (int i = 0; i < 22; i++)
            {
                UIBuff buffui = new UIBuff(i);
                base.Append(buffui);
            }

        }

        public override void Update(GameTime gameTime)
        {
            MainPanel.Width.Set(500, 0f);

            MainPanel.Height.Set(48, 0f);
            MainPanel.Left.Set(Main.screenWidth / 2 - MainPanel.Width.Pixels / 2, 0f);
            MainPanel.Top.Set(Main.screenHeight - (MainPanel.Height.Pixels + 95), 0f);
            hp.Width.Set(MainPanel.Width.Pixels - 20,0);
            mana.Width.Set(MainPanel.Width.Pixels - 20,0);
            Recalculate();
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = MainPanel.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
        }
    }

    class UIBar : UIElement
    {
        public Color backgroundColor = Color.Gray;
        private Texture2D _backgroundTexture;

        public UIBar()
        {
            if (_backgroundTexture == null)
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/Bar");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), backgroundColor);
        }
    }

    class UIInnerBar : UIElement
    {
        public Color backgroundColor = Color.Gray;
        private Texture2D _backgroundTexture;

        public UIInnerBar()
        {
            if (_backgroundTexture == null)
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/Blank");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), backgroundColor);
        }
    }

    class ResourceBar : UIElement
    {
        private UIInnerBar currentBar;
        private UIInnerBar shieldBar;
        private UIInnerBar magicShieldBar;
        private UIInnerBar physShieldBar;
        private UIText text;
        private UIText regen;
        private ResourceBarMode stat;
        private float width;
        private float height;
        

        public override void OnInitialize()
        {
            Height.Set(height, 0f);
            Width.Set(width, 0f);

            UIBar barBackground = new UIBar();
            barBackground.Left.Set(0f, 0f);
            barBackground.Top.Set(0f, 0f);
            barBackground.backgroundColor = Color.White;
            barBackground.Width.Set(width, 0f);
            barBackground.Height.Set(height, 0f);

            currentBar = new UIInnerBar();
            currentBar.SetPadding(0);
            currentBar.Left.Set(8f, 0f);
            currentBar.Top.Set(2, 0f);
            currentBar.Width.Set(width, 0f);
            currentBar.Height.Set(height - 4, 0f);

            switch (stat)
            {
                case ResourceBarMode.HP:
                    currentBar.backgroundColor = new Color(164, 55, 65); 

                    shieldBar = new UIInnerBar(); 
                    shieldBar.SetPadding(0);
                    shieldBar.Left.Set(8f, 0f);
                    shieldBar.Top.Set(2f, 0f);
                    shieldBar.Width.Set(width, 0f);
                    shieldBar.Height.Set(height - 4, 0f);
                    shieldBar.backgroundColor = new Color(230, 230, 230);

                    magicShieldBar = new UIInnerBar();
                    magicShieldBar.SetPadding(0);
                    magicShieldBar.Left.Set(8f, 0f);
                    magicShieldBar.Top.Set(2f, 0f);
                    magicShieldBar.Width.Set(width, 0f);
                    magicShieldBar.Height.Set(height -4, 0f);
                    magicShieldBar.backgroundColor = new Color(172, 122, 219);

                    physShieldBar = new UIInnerBar();
                    physShieldBar.SetPadding(0);
                    physShieldBar.Left.Set(8f, 0f);
                    physShieldBar.Top.Set(2f, 0f);
                    physShieldBar.Width.Set(width, 0f);
                    physShieldBar.Height.Set(height -4, 0f);
                    physShieldBar.backgroundColor = new Color(219, 190, 118);

                    barBackground.Append(shieldBar);
                    barBackground.Append(magicShieldBar);
                    barBackground.Append(physShieldBar);
                    break;

                case ResourceBarMode.MANA:
                    currentBar.backgroundColor = new Color(46, 67, 114); 
                    break;
                default:
                    break;
            }

            text = new UIText("0|0"); 
            text.Width.Set(width, 0f);
            text.Height.Set(height, 0f);
            text.Top.Set((height / 2 - text.MinHeight.Pixels / 2), 0f);

            regen = new UIText("0/s", 0.75f);
            regen.Width.Set(width, 0f);
            regen.Height.Set(height, 0f);
            regen.Top.Set((height / 2 - text.MinHeight.Pixels / 2 + 3), 0f);
            regen.Left.Set(width / 2 - 26, 0);


            barBackground.Append(currentBar);
            barBackground.Append(text);
            barBackground.Append(regen);
            base.Append(barBackground);
        }

        public ResourceBar(ResourceBarMode stat, int height, int width)
        {
            this.stat = stat;
            this.width = width;
            this.height = height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            regen.Top.Set((Height.Pixels / 2 - text.MinHeight.Pixels / 2 + 3), 0f);
            regen.Left.Set(Width.Pixels / 2 - 26, 0);
            switch (stat)
            {
                case ResourceBarMode.HP:

                    if (player.statLife > modPlayer.GetRealHeathWithoutShield(true))
                    {
                        quotient = (float)modPlayer.GetRealHeathWithoutShield() / (float)player.statLife;

                        shieldBar.Width.Set((width - 16) * (float)(modPlayer.NormalShield) / (float)player.statLife, 0);
                        physShieldBar.Width.Set((width - 16) * (float)(modPlayer.PhysicalShield) / (float)player.statLife, 0);
                        magicShieldBar.Width.Set((width - 16) * (float)(modPlayer.MagicShield) / (float)player.statLife, 0);
                    }
                    else
                    {
                        quotient = (float)modPlayer.GetRealHeathWithoutShield() / (float)modPlayer.GetRealHeathWithoutShield(true);

                        shieldBar.Width.Set((width - 16) * (float)(modPlayer.NormalShield) / (float)modPlayer.GetRealHeathWithoutShield(true), 0);
                        physShieldBar.Width.Set((width - 16) * (float)(modPlayer.PhysicalShield) / (float)modPlayer.GetRealHeathWithoutShield(true), 0);
                        magicShieldBar.Width.Set((width - 16) * (float)(modPlayer.MagicShield) / (float)modPlayer.GetRealHeathWithoutShield(true), 0);
                    }
                    currentBar.Width.Set(quotient * (width - 16), 0f);
                    shieldBar.Left.Set(currentBar.Left.Pixels + currentBar.Width.Pixels,0);
                    physShieldBar.Left.Set(shieldBar.Left.Pixels + shieldBar.Width.Pixels, 0);
                    magicShieldBar.Left.Set(physShieldBar.Left.Pixels + physShieldBar.Width.Pixels, 0);
                    break;

                case ResourceBarMode.MANA:

                    if (player.statMana >= player.statManaMax2)
                        quotient = 1;
                    else
                        quotient = (float)player.statMana / (float)player.statManaMax2;
                    currentBar.Width.Set(quotient * width - 16, 0f);
                    break;

                default:
                    break;
            }

            
            Recalculate(); 

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.LocalPlayer; 
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.GetTotalShield() > 0 && modPlayer.PureHealthLastStep > modPlayer.GetRealHeathWithoutShield())
            {
                int hpDiff = modPlayer.PureHealthLastStep - modPlayer.GetRealHeathWithoutShield();
                int damageToDeal = hpDiff;

                while (damageToDeal > 0 && modPlayer.Shields.Count != 0)
                {
                    if (modPlayer.wasHitByProjOrNPCLastStep == "Proj")
                    {
                        if (modPlayer.MagicShield > 0)
                        {
                            int ShieldNum = -1;

                            for (int i = 0; i < modPlayer.Shields.Count; i++)
                            {
                                if (modPlayer.Shields[i].ShieldType == ShieldType.Magic)
                                {
                                    ShieldNum = i;
                                    break;
                                }
                            }

                            if (ShieldNum != -1)
                            {
                                Shield currentShield = modPlayer.Shields[ShieldNum];

                                if (currentShield.ShieldAmount <= damageToDeal)
                                {
                                    damageToDeal -= currentShield.ShieldAmount;
                                    modPlayer.Shields.RemoveAt(ShieldNum);
                                }
                                else
                                {
                                    modPlayer.Shields[ShieldNum] = new Shield(currentShield.ShieldAmount - damageToDeal, currentShield.ShieldColor, currentShield.AssociatedBuff, currentShield.ShieldType, currentShield.ShieldTimeLeft);
                                    player.statLife += damageToDeal;
                                    break;
                                }
                            }
                            else
                            {
                                modPlayer.wasHitByProjOrNPCLastStep = "None";
                            }
                        }
                        else
                        {
                            modPlayer.wasHitByProjOrNPCLastStep = "None";
                        }
                    }
                    if (modPlayer.wasHitByProjOrNPCLastStep == "NPC")
                    {
                        if (modPlayer.PhysicalShield > 0)
                        {
                            int ShieldNum = -1;

                            for (int i = 0; i < modPlayer.Shields.Count; i++)
                            {
                                if (modPlayer.Shields[i].ShieldType == ShieldType.Physical)
                                {
                                    ShieldNum = i;
                                    break;
                                }
                            }

                            if (ShieldNum != -1)
                            {
                                Shield currentShield = modPlayer.Shields[ShieldNum];

                                if (modPlayer.Shields[0].ShieldAmount <= damageToDeal)
                                {
                                    damageToDeal -= modPlayer.Shields[ShieldNum].ShieldAmount;
                                    modPlayer.Shields.RemoveAt(ShieldNum);
                                }
                                else
                                {
                                    modPlayer.Shields[ShieldNum] = new Shield(currentShield.ShieldAmount - damageToDeal, currentShield.ShieldColor, currentShield.AssociatedBuff, currentShield.ShieldType, currentShield.ShieldTimeLeft);
                                    player.statLife += damageToDeal;
                                    break;
                                }
                            }
                            else
                            {
                                modPlayer.wasHitByProjOrNPCLastStep = "None";
                            }
                        }
                        else
                        {
                            modPlayer.wasHitByProjOrNPCLastStep = "None";
                        }
                    }
                    if (modPlayer.wasHitByProjOrNPCLastStep == "None")
                    {
                        int ShieldNum = -1;


                        if (player.lifeRegen < 0)
                        {
                            for (int i = 0; i < modPlayer.Shields.Count; i++)
                            {
                                if (modPlayer.Shields[i].ShieldType != ShieldType.Magic)
                                {
                                    ShieldNum = i;
                                    break;
                                }
                            }
                        }
                        else
                        {

                            for (int i = 0; i < modPlayer.Shields.Count; i++)
                            {
                                if (modPlayer.Shields[i].ShieldType != ShieldType.Physical && modPlayer.Shields[i].ShieldType != ShieldType.Magic)
                                {
                                    ShieldNum = i;
                                    break;
                                }
                            }
                        }

                        if (ShieldNum != -1)
                        {
                            Shield currentShield = modPlayer.Shields[ShieldNum];

                            if (modPlayer.Shields[0].ShieldAmount <= damageToDeal)
                            {
                                damageToDeal -= modPlayer.Shields[ShieldNum].ShieldAmount;
                                modPlayer.Shields.RemoveAt(ShieldNum);
                            }
                            else
                            {
                                modPlayer.Shields[ShieldNum] = new Shield(currentShield.ShieldAmount - damageToDeal, currentShield.ShieldColor, currentShield.AssociatedBuff, currentShield.ShieldType, currentShield.ShieldTimeLeft);
                                player.statLife += damageToDeal;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                player.statLife += hpDiff - damageToDeal;
                modPlayer.wasHitByProjOrNPCLastStep = "None";
            }

            switch (stat)
            {
                case ResourceBarMode.HP:
                    if (modPlayer.GetTotalShield() > 0)
                        text.SetText("" + modPlayer.GetRealHeathWithoutShield() + "+(" + modPlayer.GetTotalShield() + ") / " + (modPlayer.GetRealHeathWithoutShield(true))); //Set Life
                    else
                        text.SetText("" + player.statLife + " / " + player.statLifeMax2);

                    regen.SetText((double)player.lifeRegen/2 + "/s", 0.6f, false);
                    break;

                case ResourceBarMode.MANA:
                    text.SetText("" + player.statMana + " / " + player.statManaMax2); //Set Mana
                    regen.SetText(modPlayer.manaRegen + "/s", 0.6f, false);
                    break;

                default:
                    break;
            }
            base.Update(gameTime);
        }
    }

    class UIBuff : UIElement
    {
        private int buffNumber;
        private UIBuffImage buffImage;
        private UIText bufftime;
        private UIBuffText buffText;
        bool rightMouseDownLastStep = false;

        public UIBuff(int buffNum)
        {
            buffNumber = buffNum;
        }

        public override void OnInitialize()
        {
            Height.Set(24, 0f);
            Width.Set(16, 0f);  

            Left.Set((24 * buffNumber) + (Main.screenWidth/2) - 250, 0);
            Top.Set(Main.screenHeight - 171, 0);

            bufftime = new UIText("0s",0.75f);
            bufftime.Width.Set(16, 0f);
            bufftime.Height.Set(8, 0f);
            bufftime.Top.Set(16, 0f);

            buffImage = new UIBuffImage(buffNumber);
            buffImage.Width.Set(16, 0);
            buffImage.Height.Set(16, 0);

            buffText = new UIBuffText("");

            base.Append(buffImage);
            base.Append(bufftime);
            base.Append(buffText);
            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            Left.Set((24 * buffNumber) + (Main.screenWidth / 2) - 250, 0);
            Top.Set(Main.screenHeight - 171, 0);

            buffImage.ChangeImage(Main.LocalPlayer.buffType[buffNumber]);
            if ((Main.LocalPlayer.buffTime[buffNumber] / 60) == 0 || Main.buffNoTimeDisplay[Main.LocalPlayer.buffType[buffNumber]])
            {
                bufftime.SetText("");
            }
            else
            {
                if (Main.LocalPlayer.buffTime[buffNumber] / 60 > 60)
                {
                    bufftime.SetText(((Main.LocalPlayer.buffTime[buffNumber] / 60) / 60).ToString() + "m");
                }
                else
                {
                    bufftime.SetText((Main.LocalPlayer.buffTime[buffNumber] / 60).ToString() + "s");
                }
            }
            
            if (IsMouseHovering)
            {
                string color = "0099cc";

                string buffDescription = "[c/"+ color + ":" + Lang.GetBuffName(Main.LocalPlayer.buffType[buffNumber]) + "]" +
                    "\n" + Lang.GetBuffDescription(Main.LocalPlayer.buffType[buffNumber]);


                if (Main.LocalPlayer.buffType[buffNumber] == Terraria.ID.BuffID.MonsterBanner)
                {
                    string name1 = "";

                    for (int i = 0; i < Main.LocalPlayer.NPCBannerBuff.Count(); i++)
                    {
                        if (Item.BannerToNPC(i) != 0 && Main.player[Main.myPlayer].NPCBannerBuff[i])
                        {
                            if (name1 == "")
                                name1 = Lang.GetNPCNameValue(Item.BannerToNPC(i));
                            else
                            {
                                string name2 = Lang.GetNPCNameValue(Item.BannerToNPC(i));

                                int spaces = 18 - name1.Length;

                                buffDescription += "\n" + name1 + ", " + name2;

                                name1 = "";
                            }
                        }
                    }

                    if (name1 != "")
                        buffDescription += "\n" + name1;
                }
                else if (Main.LocalPlayer.buffType[buffNumber] == Terraria.ID.BuffID.ManaSickness)
                {
                    buffDescription += (int)(Main.LocalPlayer.manaSickReduction * 100) + "%";
                }

                buffText.SetText(buffDescription);

                if (Lang.GetBuffName(Main.LocalPlayer.buffType[buffNumber]) != "")
                {
                    buffText.Left.Set(-Left.Pixels + Main.screenWidth/2 - 250, 0);

                    int count = buffText.Text.Split('\n').Length;

                    buffText.Top.Set(-28*count, 0);
                }
                else
                {
                    buffText.SetText("");
                }

                
                if (rightMouseDownLastStep && !Main.mouseRight && !Main.debuff[Main.LocalPlayer.buffType[buffNumber]])
                {
                    Main.LocalPlayer.ClearBuff(Main.LocalPlayer.buffType[buffNumber]);
                }
                if (Main.mouseRight)
                {
                    rightMouseDownLastStep = true;
                }
                else
                {
                    rightMouseDownLastStep = false;
                }
            }
            else
            {
                buffText.SetText("");
            }

            Recalculate();
            base.Update(gameTime);
        }
    }

    class UIBuffImage : UIElement
    {
        private Texture2D buffImage;
        private int buffNumber;

        public UIBuffImage(int buffNum)
        {
            buffNumber = buffNum;
        }

        public void ChangeImage(int type)
        {
            buffImage = Main.buffTexture[type];
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (buffImage != null)
            {
                CalculatedStyle dimensions = GetDimensions();
                Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
                int width = (int)Math.Ceiling(dimensions.Width);
                int height = (int)Math.Ceiling(dimensions.Height);
                spriteBatch.Draw(buffImage, new Rectangle(point1.X, point1.Y, 16, 16), Color.White);
            }
        }

        public void RClick()
        {

        }

        
    }

    class UIBuffText : UIText
    {
        public UIBuffText(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            Width.Set(500, 0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }

    internal enum ResourceBarMode
    {
        HP,
        MANA
    }
}
