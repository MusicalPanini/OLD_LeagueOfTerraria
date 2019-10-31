using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria.ID;
using TerraLeague.Items.SummonerSpells;
using TerraLeague.Items.CustomItems;

namespace TerraLeague.UI
{
    internal class ItemUI : UIState
    {
        public UIElement MainPanel;
        public UISummonerPanel SummonerPanel;
        public UIItemPanel ItemPanel;
        public UIStatPanel StatPanel;
        public static bool visible = true;
        public static bool extraStats = false;
        public static bool panelLocked = false;

        public override void OnInitialize()
        {
            MainPanel = new UIElement();
            MainPanel.SetPadding(0);
            MainPanel.Left.Set(Main.screenWidth - 316, 0f);
            MainPanel.Top.Set(Main.screenHeight - 216, 0f);
            MainPanel.Width.Set(250, 0f);
            MainPanel.Height.Set(101, 0f);
            
            ItemPanel = new UIItemPanel(99,0,149,101, new Color(10, 100, 50));
            SummonerPanel = new UISummonerPanel(0, 47,99,54, new Color(10, 100, 50));
            StatPanel = new UIStatPanel(66, Main.screenHeight - (int)(54 + 44), 150, 54, new Color(10, 100, 50));

            MainPanel.Append(SummonerPanel);
            MainPanel.Append(ItemPanel);
            Append(MainPanel);
            Append(StatPanel);
        }

        public override void Update(GameTime gameTime)
        {
            StatPanel.extraStats = extraStats;

            if (Main.invasionProgressNearInvasion)
            {
                MainPanel.Left.Set(Main.screenWidth - (MainPanel.Width.Pixels + 240), 0);
                MainPanel.Top.Set(Main.screenHeight - (MainPanel.Height.Pixels + 44), 0f);
            }
            else
            {
                MainPanel.Left.Set(Main.screenWidth - (MainPanel.Width.Pixels + 40), 0);
                MainPanel.Top.Set(Main.screenHeight - (MainPanel.Height.Pixels + 44), 0f);
            }

            if (!ItemPanel.GetDimensions().ToRectangle().Intersects(GetDimensions().ToRectangle()))
            {
                var parentSpace = GetDimensions().ToRectangle();
                ItemPanel.Left.Pixels = Utils.Clamp(ItemPanel.Left.Pixels, 0, parentSpace.Right - ItemPanel.Width.Pixels);
                ItemPanel.Top.Pixels = Utils.Clamp(ItemPanel.Top.Pixels, 0, parentSpace.Bottom - ItemPanel.Height.Pixels);
            }
            SummonerPanel.Width.Pixels = 108;
            ItemPanel.Recalculate();
            SummonerPanel.Recalculate();
            Recalculate();
            base.Update(gameTime);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (MainPanel.ContainsPoint(MousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
        
    }

    class UISummonerPanel : UIPanel
    {
        UISummonerSlot Slot1;
        UISummonerSlot Slot2;

        public UISummonerPanel(int left, int top, int width, int height, Color color)
        {
            SetPadding(0);
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(width, 0f);
            Height.Set(height, 0f);
            BackgroundColor = color;

            Slot1 = new UISummonerSlot(1,5,5,44);
            Append(Slot1);

            Slot2 = new UISummonerSlot(2,52,5,44);
            Append(Slot2);
        }
    }

    class UISummonerSlot : UIPanel
    {
        Texture2D placeholderArt = Main.buffTexture[BuffID.Oiled];
        public UIImage sumImage;
        public UIText sumCD;
        UIText toolTip;
        int slotNum;

        public UISummonerSlot(int SlotNum, int left, int top, int dimentions)
        {
            slotNum = SlotNum;
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(dimentions, 0f);
            Height.Set(dimentions, 0f);
            BackgroundColor = new Color(35, 100, 80);

            sumImage = new UIImage(placeholderArt);
            sumImage.Width.Pixels = Width.Pixels;
            sumImage.Height.Pixels = Height.Pixels;
            sumImage.Left.Pixels = -6;
            sumImage.Top.Pixels = -6;
            Append(sumImage);

            sumCD = new UIText("", 1);
            sumCD.Left.Pixels = 9;
            sumCD.Top.Pixels = 2;
            Append(sumCD);

            toolTip = new UIText("",1);
            toolTip.Width.Set(500, 0f);
            Append(toolTip);
        }

        public override void Update(GameTime gameTime)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            SummonerSpell spell = modPlayer.sumSpells[slotNum - 1];

            if (modPlayer.sumSpells[slotNum - 1] != null)
            {
                sumImage.SetImage(TerraLeague.instance.GetTexture(spell.GetIconTexturePath()));
                sumImage.ImageScale = 1;

                if (modPlayer.sumCooldowns[slotNum - 1] == 0)
                    sumCD.SetText("");
                else
                    sumCD.SetText((modPlayer.sumCooldowns[slotNum - 1] / 60 + 1).ToString());

                sumCD.Left.Pixels = 8 - (sumCD.Text.Length * 4);
            }
            else
            {
                sumImage.SetImage(placeholderArt);
                sumImage.ImageScale = 0;
                sumCD.SetText("");
            }

            if (IsMouseHovering)
            {
                string text = "[c/0099cc:" + spell.GetSpellName() + "]\n";
                text += spell.GetTooltip();
                text += "\n" + spell.GetCooldown() + " second cooldown";

                toolTip.SetText(text);

                toolTip.Left.Set((-(Main.screenWidth - 316) - (slotNum == 1 ? 5 : 52)) + (Main.screenWidth/2 - 288), 0);

                int count = toolTip.Text.Split('\n').Length;
                toolTip.Top.Set((-28 * count) - 90, 0);
            }
            else
            {
                toolTip.SetText("");
            }

            sumImage.Recalculate();
            sumCD.Recalculate();
            Recalculate();
            base.Update(gameTime);
        }
    }

    class UIItemPanel : UIPanel
    {
        UIItemSlot Item1;
        UIItemSlot Item2;
        UIItemSlot Item3;
        UIItemSlot Item4;
        UIItemSlot Item5;
        UIItemSlot Item6;

        public UIItemPanel(int left, int top, int width, int height, Color color)
        {
            SetPadding(0);
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(width, 0f);
            Height.Set(height, 0f);
            BackgroundColor = color;

            Item1 = new UIItemSlot(1, 5, 5, 44);
            Append(Item1);

            Item2 = new UIItemSlot(2, 52, 5, 44);
            Append(Item2);

            Item3 = new UIItemSlot(3, 99, 5, 44);
            Append(Item3);

            Item4 = new UIItemSlot(4, 5, 52, 44);
            Append(Item4);

            Item5 = new UIItemSlot(5, 52, 52, 44);
            Append(Item5);

            Item6 = new UIItemSlot(6, 99, 52, 44);
            Append(Item6);
        }
    }

    class UIItemSlot : UIPanel
    {
        Texture2D placeholderArt = Main.buffTexture[BuffID.Oiled];
        UIImage itemImage;
        UIText itemStat;
        UIText itemKey;
        UIToolTip toolTip;
        int slotNum;

        public UIItemSlot(int SlotNum, int left, int top, int dimentions)
        {
            slotNum = SlotNum;
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(dimentions, 0f);
            Height.Set(dimentions, 0f);
            BackgroundColor = new Color(35, 100, 80);

            itemImage = new UIImage(placeholderArt);
            itemImage.Width.Pixels = Width.Pixels;
            itemImage.Height.Pixels = Height.Pixels;
            itemImage.Left.Pixels = -6;
            itemImage.Top.Pixels = -6;
            Append(itemImage);

            itemStat = new UIText("", 0.75f);
            itemStat.Left.Pixels = 8;
            itemStat.Top.Pixels = 24;
            Append(itemStat);

            itemKey = new UIText(slotNum.ToString(), 0.75f);
            itemKey.Left.Pixels = -7;
            itemKey.Top.Pixels = -8;
            Append(itemKey);

            if (slotNum > 3)
                toolTip = new UIToolTip((slotNum - 4) * -47, -47, Main.LocalPlayer.armor[slotNum + 2].type);
            else
                toolTip = new UIToolTip((slotNum -1) * -47, 0, Main.LocalPlayer.armor[slotNum + 2].type);

            Append(toolTip);
        }

        public override void Update(GameTime gameTime)
        {
            string itemSlotText = "N/A";
            switch (slotNum)
            {
                case 1:
                    itemSlotText = TerraLeague.ConvertKeyString(TerraLeague.Item1);
                    break;
                case 2:
                    itemSlotText = TerraLeague.ConvertKeyString(TerraLeague.Item2);
                    break;
                case 3:
                    itemSlotText = TerraLeague.ConvertKeyString(TerraLeague.Item3);
                    break;
                case 4:
                    itemSlotText = TerraLeague.ConvertKeyString(TerraLeague.Item4);
                    break;
                case 5:
                    itemSlotText = TerraLeague.ConvertKeyString(TerraLeague.Item5);
                    break;
                case 6:
                    itemSlotText = TerraLeague.ConvertKeyString(TerraLeague.Item6);
                    break;
                default:
                    break;
            }
            itemKey.SetText(itemSlotText);

            LeagueItem legItem = Main.LocalPlayer.armor[slotNum + 2].modItem as LeagueItem;

            if (legItem != null)
            {
                itemStat.SetText(legItem.GetStatText());

                if (legItem.OnCooldown(Main.LocalPlayer))
                {
                    BackgroundColor = new Color(50, 50, 50);
                }
                else if (legItem.GetActive() != null)
                {
                    BackgroundColor = new Color(70, 150, 120);
                }
                else
                {
                    BackgroundColor = new Color(35, 100, 80);
                }
            }
            else
            {
                itemStat.SetText("");
                BackgroundColor = new Color(35, 100, 80);
            }


            if (Main.LocalPlayer.armor[slotNum + 2].active)
            {
                Texture2D texture = GetTexture(Main.LocalPlayer.armor[slotNum + 2]);
                itemImage.SetImage(texture);
                itemImage.Left.Pixels = ((32 - texture.Width) / 2) - 6;
                itemImage.Top.Pixels = ((32 - texture.Height) / 2) - 6;
                itemImage.ImageScale = 1;
            }
            else
            {
                itemImage.SetImage(placeholderArt);
                itemImage.ImageScale = 0;
            }


            if (IsMouseHovering)
            {
                if (Lang.GetItemName(Main.LocalPlayer.armor[slotNum + 2].type).ToString() != "")
                {
                    toolTip.itemType = Main.LocalPlayer.armor[slotNum + 2].type;
                    toolTip.drawText = true;
                }
                else
                {
                    toolTip.drawText = false;
                }
            }
            else
            {
                toolTip.drawText = false;
            }
            Recalculate();
            base.Update(gameTime);
        }

        private Texture2D GetTexture(Item item)
        {
            Texture2D texture = Main.itemTexture[item.type];

            if (texture.Width * 2 < texture.Height)
            {
                Rectangle newBounds = texture.Bounds;
                newBounds.X = 0;
                newBounds.Y = 0;
                newBounds.Width = texture.Width;
                newBounds.Height = texture.Width;

                Texture2D croppedTexture = new Texture2D(Main.graphics.GraphicsDevice, newBounds.Width, newBounds.Height);

                Color[] data = new Color[newBounds.Width * newBounds.Height];
                texture.GetData(0, newBounds, data, 0, newBounds.Width * newBounds.Height);
                croppedTexture.SetData(data);

                return croppedTexture;
            }

            return texture;
        }
    }

    class UIToolTip : UIElement
    {
        public int itemType;
        public bool drawText = false;
        UIToolTipLine[] uiLines = new UIToolTipLine[16];

        public UIToolTip(int left, int top, int type)
        {
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(500, 0f);

            for (int i = 0; i < uiLines.Length; i++)
            {
                uiLines[i] = new UIToolTipLine(0, -320 + (28 * i), i.ToString());
                Append(uiLines[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (drawText)
            {
                try
                {
                    ModItem modItem = null;
                    LeagueItem legItem;
                    string[] activeTip;
                    string[] primPassiveTip;
                    string[] secPassiveTip;
                    int lines = 0;
                    System.Collections.Generic.List<string> extraLines = new System.Collections.Generic.List<string>();

                    for (int i = 3; i < 9; i++)
                    {
                        if (Main.LocalPlayer.armor[i].type == itemType)
                        {
                            modItem = Main.LocalPlayer.armor[i].modItem;
                            break;
                        }
                    }

                    legItem = modItem as LeagueItem;
                    
                    if (legItem != null)
                    {
                        if (legItem.GetActive() != null)
                        {
                            activeTip = legItem.GetActive().Tooltip(Main.LocalPlayer, legItem).Split('\n');
                            for (int i = 0; i < activeTip.Length; i++)
                            {
                                extraLines.Add(activeTip[i]);
                            }
                        }
                        if (legItem.GetPrimaryPassive() != null)
                        {
                            primPassiveTip = legItem.GetPrimaryPassive().Tooltip(Main.LocalPlayer, legItem).Split('\n');
                            for (int i = 0; i < primPassiveTip.Length; i++)
                            {
                                extraLines.Add(primPassiveTip[i]);
                            }
                        }
                        if (legItem.GetSecondaryPassive() != null)
                        {
                            secPassiveTip = legItem.GetSecondaryPassive().Tooltip(Main.LocalPlayer, legItem).Split('\n');
                            for (int i = 0; i < secPassiveTip.Length; i++)
                            {
                                extraLines.Add(secPassiveTip[i]);
                            }
                        }
                    }

                    lines += Lang.GetTooltip(itemType).Lines;
                    Height.Set(28 * lines, 0);

                    string name = "[c/0099cc:" + Lang.GetItemName(itemType) + "]";
                    var poop = Lang.GetTooltip(itemType);

                    for (int i = 0; i < uiLines.Length; i++)
                    {
                        if (i >= uiLines.Length - (lines + extraLines.Count + 1))
                        {
                            if (uiLines.Length - (lines + extraLines.Count + 1) == i)
                                uiLines[i].SetText(name);
                            else if (uiLines.Length - extraLines.Count > i)
                                uiLines[i].SetText(Lang.GetTooltip(itemType).GetLine(i - (uiLines.Length - (extraLines.Count + lines))));
                            else
                                uiLines[i].SetText(extraLines[i - (uiLines.Length - extraLines.Count)]);
                        }
                        else
                        {
                            uiLines[i].SetText("");
                        }
                    }
                }
                catch (Exception)
                {
                    for (int i = 0; i < uiLines.Length; i++)
                    {
                        uiLines[i].SetText("");
                    }
                }
                
            }
            else
            {
                for (int i = 0; i < uiLines.Length; i++)
                {
                    uiLines[i].SetText("");
                    uiLines[i].Left.Set(-(Main.screenWidth - 174) + (Main.screenWidth/2 - 250), 0);
                    uiLines[i].Top.Set(-320 + (28 * i) - 172, 0);
                }
            }

            Recalculate();
        }
    }

    class UIToolTipLine : UIText
    {
        public UIToolTipLine(int left, int top, string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            SetText(text,textScale, large);
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(500, 0f);
            Height.Set(28, 0);
        }
    }

    class UIStatPanel : UIPanel
    {
        public bool extraStats = false;

        UIText meleeStats;
        UIText rangedStats;
        UIText magicStats;
        UIText summonStats;
        UIText armorStats;
        UIText resistStats;

        UIText CDRStats;
        UIText ammoStats;
        UIText healStats;

        UIText tooltip;

        public UIStatPanel(int left, int top, int width, int height, Color color)
        {
            SetPadding(0);
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(width, 0f);
            Height.Set(height, 0f);
            BackgroundColor = color;


            armorStats = new UIText("ARM: 000", 0.65f);
            armorStats.Left.Pixels = 8;
            armorStats.Top.Pixels = 6;
            armorStats.TextColor = Color.Yellow;

            resistStats = new UIText("RST: 000", 0.65f);
            resistStats.Left.Pixels = 80;
            resistStats.Top.Pixels = 6;
            resistStats.TextColor = Color.LightSteelBlue;

            meleeStats = new UIText("MEL: 000%", 0.65f);
            meleeStats.Left.Pixels = 8;
            meleeStats.Top.Pixels = 22;
            meleeStats.TextColor = Color.Orange;

            rangedStats = new UIText("RNG: 000%", 0.65f);
            rangedStats.Left.Pixels = 80;
            rangedStats.Top.Pixels = 22;
            rangedStats.TextColor = Color.LightSeaGreen;

            magicStats = new UIText("MAG: 000%", 0.65f);
            magicStats.Left.Pixels = 8;
            magicStats.Top.Pixels = 38;
            magicStats.TextColor = Color.MediumPurple;

            summonStats = new UIText("SUM: 000%", 0.65f);
            summonStats.Left.Pixels = 80;
            summonStats.Top.Pixels = 38;
            summonStats.TextColor = Color.SkyBlue;

            CDRStats = new UIText("CDR: 40%", 0.65f);
            CDRStats.Left.Pixels = 8;
            CDRStats.Top.Pixels = 54;
            CDRStats.TextColor = Color.White;

            healStats = new UIText("HEAL: 000%", 0.65f);
            healStats.Left.Pixels = 80;
            healStats.Top.Pixels = 54;
            healStats.TextColor = Color.Green;

            ammoStats = new UIText("AMMO: 100%", 0.65f);
            ammoStats.Left.Pixels = 8;
            ammoStats.Top.Pixels = 70;
            ammoStats.TextColor = Color.Gray;

            tooltip = new UIText("");
            tooltip.Left.Set(Main.screenWidth/2 - 250 - Left.Pixels, 0);
            tooltip.Top.Set(Main.screenHeight - 171 - Top.Pixels, 0);

            Append(meleeStats);
            Append(rangedStats);
            Append(magicStats);
            Append(summonStats);
            Append(armorStats);
            Append(resistStats);
            Append(CDRStats);
            Append(healStats);
            Append(ammoStats);
            Append(tooltip);
        }

        public override void Update(GameTime gameTime)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            armorStats.Width.Set(30,0);
            resistStats.Width.Set(30,0);
            meleeStats.Width.Set(30,0);
            rangedStats.Width.Set(30,0);
            magicStats.Width.Set(30,0);
            summonStats.Width.Set(30,0);
            CDRStats.Width.Set(30,0);
            healStats.Width.Set(30,0);
            ammoStats.Width.Set(30,0);

            if (extraStats)
            {
                Height.Set(86, 0f);
                GetStats(true);
            }
            else
            {
                Height.Set(54, 0f);
                GetStats();
            }
            Top.Set(Main.screenHeight - (Height.Pixels + 44), 0f);

            string text = "";

            if (armorStats.IsMouseHovering)
            {
                text = "[c/FFFF00:Armor]" +
                    "\nReduces damage from contact by 0.5 (0.75 in Expert) damage per point";
            }
            else if (resistStats.IsMouseHovering)
            {
                text = "[c/B0C4DE:Resist]" +
                    "\nReduces damage from projectiles by 0.5 (0.75 in Expert) damage per point";
            }
            else if (meleeStats.IsMouseHovering)
            {
                text = "[c/FFA500:Melee Damage]" +
                    "\nUsed for Abilities and Items scaling damage." +
                    "\nMelee Weapons Deal " + (int)(modPlayer.meleeDamageLastStep * 100) + "% damage.";
            }
            else if (rangedStats.IsMouseHovering)
            {
                text = "[c/20B2AA:Ranged Damage]" +
                    "\nUsed for Abilities and Items scaling damage." +
                    "\nRanged Weapons Deal " + (int)(modPlayer.rangedDamageLastStep * 100) + "% damage.";
            }
            else if (magicStats.IsMouseHovering)
            {
                text = "[c/8E70DB:Magic Damage]" +
                    "\nUsed for Abilities and Items scaling damage." +
                    "\nMagic Weapons Deal " + (int)(modPlayer.magicDamageLastStep * 100) + "% damage.";
            }
            else if (summonStats.IsMouseHovering)
            {
                text = "[c/87CEEB:Summon Damage]" +
                    "\nUsed for Abilities and Items scaling damage." +
                    "\nSummoner Weapons Deal " + (int)(modPlayer.minionDamageLastStep * 100) + "% damage.";
            }
            else if (CDRStats.IsMouseHovering)
            {
                text = "[c/FFFFFF:Cooldown Reduction]" +
                    "\nThe percentage reduction for abilities and summoner spells (Max 40%)";
            }
            else if (ammoStats.IsMouseHovering)
            {
                text = "[c/808080:Ammo Consume Chance]" +
                    "\nThe percent chance to consume ammo";
            }
            else if (healStats.IsMouseHovering)
            {
                text = "[c/008000:Healing Power]" +
                    "\nThe percent increase in all your healing";
            }


            int count = text.Split('\n').Length;

            tooltip.Top.Set((Main.screenHeight - 171 - Top.Pixels) - (28 * count), 0);
            tooltip.Left.Set(Main.screenWidth / 2 - 250 - Left.Pixels, 0);
            tooltip.SetText(text);
            Recalculate();
            base.Update(gameTime);
        }

        public void GetStats(bool extra = false)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            armorStats.SetText("ARM: " + modPlayer.armor.ToString());
            resistStats.SetText("RST: " + modPlayer.resist.ToString());
            meleeStats.SetText("MEL: " + modPlayer.MEL);
            rangedStats.SetText("RNG: " + modPlayer.RNG);
            magicStats.SetText("MAG: " + modPlayer.MAG);
            summonStats.SetText("SUM: " + modPlayer.SUM);

            if (extra)
            {
                ammoStats.SetText("AMMO: " + (modPlayer.ConsumeAmmoChance * 100).ToString() + "%");
                healStats.SetText("HEAL: " + (modPlayer.healPower * 100).ToString() + "%");
                CDRStats.SetText("CDR: " + ((1 - modPlayer.Cdr) * 100).ToString() + "%");
            }
            else
            {
                ammoStats.SetText("");
                healStats.SetText("");
                CDRStats.SetText("");
            }
        }
    }
}