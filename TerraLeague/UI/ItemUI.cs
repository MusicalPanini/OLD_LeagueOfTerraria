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
using TerraLeague.Items.Boots;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons;

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
            StatPanel = new UIStatPanel(66f / Main.screenWidth, (Main.screenHeight - (int)(58 + 48)) / (float)Main.screenHeight, 150, 54, new Color(10, 100, 50));

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
            Player player = Main.LocalPlayer;
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (player.GetModPlayer<PLAYERGLOBAL>().currentGun != 0 && !Main.playerInventory)
            {
                int left = (int)(Main.screenWidth - 92);
                int top = (int)(Main.screenHeight - 386);
                Rectangle destRec = new Rectangle(left, top, 48, 232);

                Texture2D texture = TerraLeague.instance.GetTexture("UI/LunariAmmoBar");
                Rectangle sourRec = new Rectangle(0, 0, 48, 232);
                spriteBatch.Draw(texture, destRec, sourRec, Color.White);

                Texture2D texture2 = TerraLeague.instance.GetTexture("UI/SmallBlank_V");
                Rectangle sourRec2 = new Rectangle(0, 0, 8, 8);
                Color color;
                Rectangle ammoBarPos;
                int ammoBarHeight = 0;

                // Calibrum
                color = new Color(141, 252, 245);
                ammoBarHeight = (int)(216 * (modPlayer.calibrumAmmo / 100f));
                ammoBarPos = new Rectangle(left + 4, top + 8 + (216 - ammoBarHeight), 8, ammoBarHeight);
                spriteBatch.Draw(texture2, ammoBarPos, sourRec2, color);

                color = new Color(216, 0, 32);
                ammoBarHeight = (int)(216 * (modPlayer.severumAmmo / 100f));
                ammoBarPos = new Rectangle(left + 12, top + 8 + (216 - ammoBarHeight), 8, ammoBarHeight);
                spriteBatch.Draw(texture2, ammoBarPos, sourRec2, color);

                color = new Color(200, 37, 255);
                ammoBarHeight = (int)(216 * (modPlayer.gravitumAmmo / 100f));
                ammoBarPos = new Rectangle(left + 20, top + 8 + (216 - ammoBarHeight), 8, ammoBarHeight);
                spriteBatch.Draw(texture2, ammoBarPos, sourRec2, color);

                color = new Color(0, 148, 255);
                ammoBarHeight = (int)(216 * (modPlayer.infernumAmmo / 100f));
                ammoBarPos = new Rectangle(left + 28, top + 8 + (216 - ammoBarHeight), 8, ammoBarHeight);
                spriteBatch.Draw(texture2, ammoBarPos, sourRec2, color);

                color = new Color(255, 255, 255);
                ammoBarHeight = (int)(216 * (modPlayer.crescendumAmmo / 100f));
                ammoBarPos = new Rectangle(left + 36, top + 8 + (216 - ammoBarHeight), 8, ammoBarHeight);
                spriteBatch.Draw(texture2, ammoBarPos, sourRec2, color);
            }

            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (MainPanel.ContainsPoint(MousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
    }

    class UISummonerPanel : UIElement
    {
        UISummonerSlot Slot1;
        UISummonerSlot Slot2;
        Texture2D _backgroundTexture = null;

        public UISummonerPanel(int left, int top, int width, int height, Color color)
        {
            SetPadding(0);
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(width, 0f);
            Height.Set(height, 0f);
            if (_backgroundTexture == null)
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/SummonerBackground");

            Slot1 = new UISummonerSlot(1,5,5,44);
            Append(Slot1);

            Slot2 = new UISummonerSlot(2,52,5,44);
            Append(Slot2);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y - 2);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            base.DrawSelf(spriteBatch);
        }
    }

    class UISummonerSlot : UIElement
    {
        Texture2D _backgroundTexture = null;
        Texture2D placeholderArt = Main.buffTexture[BuffID.Oiled];
        public UIImage sumImage;
        public UIText sumCD;
        UIText itemKey;
        UIText toolTip;
        int slotNum;

        public UISummonerSlot(int SlotNum, int left, int top, int dimentions)
        {
            slotNum = SlotNum;
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(dimentions, 0f);
            Height.Set(dimentions, 0f);
            if (_backgroundTexture == null)
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/AbilityBorder");

            sumImage = new UIImage(placeholderArt);
            sumImage.Width.Pixels = Width.Pixels;
            sumImage.Height.Pixels = Height.Pixels;
            sumImage.Left.Pixels = 6;
            sumImage.Top.Pixels = 4;
            Append(sumImage);

            sumCD = new UIText("", 1);
            sumCD.Left.Pixels = 8;
            sumCD.Top.Pixels = 12;
            Append(sumCD);

            itemKey = new UIText(slotNum.ToString(), 0.75f);
            itemKey.Left.Pixels = 2;
            itemKey.Top.Pixels = -2;
            Append(itemKey);

            toolTip = new UIText("",1);
            toolTip.Width.Set(500, 0f);
            Append(toolTip);
        }

        public override void Update(GameTime gameTime)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            SummonerSpell spell = modPlayer.sumSpells[slotNum - 1];

            string itemSlotText = "N/A";
            switch (slotNum)
            {
                case 1:
                    itemSlotText = TerraLeague.ConvertKeyString(TerraLeague.Sum1);
                    break;
                case 2:
                    itemSlotText = TerraLeague.ConvertKeyString(TerraLeague.Sum2);
                    break;
                default:
                    break;
            }
            itemKey.SetText(itemSlotText);

            if (modPlayer.sumSpells[slotNum - 1] != null)
            {
                sumImage.SetImage(TerraLeague.instance.GetTexture(spell.GetIconTexturePath()));
                sumImage.ImageScale = 1;

                sumCD.SetText(GetCooldown(slotNum));

                sumCD.Left.Pixels = 0;
                sumCD.Width.Pixels = this.Width.Pixels;
                sumCD.HAlign = 0.5f;
            }
            else
            {
                sumImage.SetImage(placeholderArt);
                sumImage.ImageScale = 0;
                sumCD.SetText("");
            }

            if (IsMouseHovering)
            {
                string text = TerraLeague.CreateColorString(TerraLeague.TooltipHeadingColor, spell.GetSpellName());
                text += "\n" + spell.GetTooltip();
                text += "\n" + spell.GetCooldown() + " second cooldown";

                TerraLeague.instance.tooltipUI.DrawText(text.Split('\n'));
            }

            sumImage.Recalculate();
            sumCD.Recalculate();
            Recalculate();
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y - 2);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            base.DrawSelf(spriteBatch);
        }

        string GetCooldown(int slot)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            float cooldown = modPlayer.sumCooldowns[slot - 1];

            if (cooldown > 0)
            {
                if (cooldown > 10 * 60)
                {
                    return (cooldown / 60).ToString().Split('.')[0];
                }
                else
                {
                    string text = (Math.Round(cooldown / 60, 1)).ToString();
                    if (text.Length == 1)
                        text += ".0";
                    return text;
                }
            }
            else
            {
                return "";
            }
        }
    }

    class UIItemPanel : UIElement
    {
        UIItemSlot Item1;
        UIItemSlot Item2;
        UIItemSlot Item3;
        UIItemSlot Item4;
        UIItemSlot Item5;
        UIItemSlot Item6;

        Texture2D _backgroundTexture = null;

        public UIItemPanel(int left, int top, int width, int height, Color color)
        {
            SetPadding(0);
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(width, 0f);
            Height.Set(height, 0f);

            if (_backgroundTexture == null)
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/ItemBackground");

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

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y - 2);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            base.DrawSelf(spriteBatch);
        }
    }

    class UIItemSlot : UIElement
    {
        Texture2D _backgroundTexture = null;
        Texture2D placeholderArt = Main.buffTexture[BuffID.Oiled];
        UIImage itemImage;
        UIText itemCooldown;
        UIText itemStat;
        UIText itemKey;
        int slotNum;

        public UIItemSlot(int SlotNum, int left, int top, int dimentions)
        {
            if (_backgroundTexture == null)
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/ItemBorder");

            slotNum = SlotNum;
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(dimentions, 0f);
            Height.Set(dimentions, 0f);

            itemImage = new UIImage(placeholderArt);
            itemImage.Width.Pixels = Width.Pixels;
            itemImage.Height.Pixels = Height.Pixels;
            itemImage.Left.Pixels = 0;//-6;
            itemImage.Top.Pixels = 0;//-6;
            Append(itemImage);

            itemStat = new UIText("", 0.75f);
            itemStat.Left.Pixels = 8;
            itemStat.Top.Pixels = 24;
            Append(itemStat);

            itemCooldown = new UIText("", 1);
            itemCooldown.Width.Pixels = dimentions;
            itemCooldown.Height.Pixels = dimentions;
            itemCooldown.HAlign = 0.5f;
            itemCooldown.VAlign = 0.5f;
            Append(itemCooldown);

            itemKey = new UIText(slotNum.ToString(), 0.75f);
            itemKey.Left.Pixels = 2;
            itemKey.Top.Pixels = -2;
            Append(itemKey);
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
                if (legItem.OnCooldown(Main.LocalPlayer))
                {
                    itemCooldown.SetText(legItem.GetStatText());
                    itemStat.SetText("");
                }
                else
                {
                    itemCooldown.SetText("");
                    itemStat.SetText(legItem.GetStatText());
                }

                if (legItem.OnCooldown(Main.LocalPlayer))
                {
                    _backgroundTexture = TerraLeague.instance.GetTexture("UI/ItemBorderCooldown");
                }
                else if (legItem.Active != null)
                {
                    _backgroundTexture = TerraLeague.instance.GetTexture("UI/ItemBorderActive");
                }
                else
                {
                    _backgroundTexture = TerraLeague.instance.GetTexture("UI/ItemBorder");
                }
            }
            else
            {
                itemStat.SetText("");
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/ItemBorder");
            }


            if (Main.LocalPlayer.armor[slotNum + 2].active)
            {
                Texture2D texture = GetTexture(Main.LocalPlayer.armor[slotNum + 2]);
                itemImage.SetImage(texture);
                itemImage.Left.Pixels = ((32 - texture.Width) / 2) + 6;
                itemImage.Top.Pixels = ((32 - texture.Height) / 2) + 4;
                itemStat.Left.Pixels = 16;
                itemStat.Top.Pixels = 30;
                itemImage.ImageScale = 1;
                itemCooldown.HAlign = 0.5f;
                itemCooldown.Top.Pixels = 12;
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
                    ModItem modItem = Main.LocalPlayer.armor[slotNum + 2].modItem;

                    if (modItem != null)
                    {
                        string[] activeTip;
                        string[] primPassiveTip;
                        System.Collections.Generic.List<string> activePassiveTooltips = new System.Collections.Generic.List<string>();

                        for (int i = 3; i < 9; i++)
                        {
                            if (Main.LocalPlayer.armor[i].type == modItem.item.type)
                            {
                                modItem = Main.LocalPlayer.armor[i].modItem;
                                break;
                            }
                        }

                        legItem = modItem as LeagueItem;

                        if (legItem != null)
                        {
                            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, legItem);
                            if (slot != -1)
                            {
                                if (legItem.Active != null)
                                {
                                    if (legItem.Active.currentlyActive)
                                    {
                                        activeTip = legItem.Active.Tooltip(Main.LocalPlayer, legItem).Split('\n');
                                        for (int i = 0; i < activeTip.Length; i++)
                                        {
                                            activePassiveTooltips.Add(activeTip[i]);
                                        }
                                    }
                                }
                                if (legItem.Passives != null)
                                {
                                    for (int j = 0; j < legItem.Passives.Length; j++)
                                    {
                                        if (legItem.Passives[j].currentlyActive)
                                        {
                                            primPassiveTip = legItem.Passives[j].Tooltip(Main.LocalPlayer, legItem).Split('\n');
                                            for (int i = 0; i < primPassiveTip.Length; i++)
                                            {
                                                activePassiveTooltips.Add(primPassiveTip[i]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var bootItem = modItem as LeagueBoot;
                            if (bootItem != null)
                            {
                                primPassiveTip = bootItem.BuildFullTooltip(false).Split('\n');
                                for (int i = 0; i < primPassiveTip.Length; i++)
                                {
                                    activePassiveTooltips.Add(primPassiveTip[i]);
                                }
                            }
                        }

                        string heading = TerraLeague.CreateColorString(TerraLeague.TooltipHeadingColor, Lang.GetItemName(modItem.item.type).Value);
                        var itemsBaseTooltip = Lang.GetTooltip(modItem.item.type);

                        string[] compiledTooltip = new string[ToolTipUI.MaxLines];
                        compiledTooltip[0] = heading;

                        int baseTooltipLines = itemsBaseTooltip.Lines;
                        if (baseTooltipLines == 1 && itemsBaseTooltip.GetLine(0) == "")
                            baseTooltipLines = 0;

                        for (int i = 0; i < baseTooltipLines; i++)
                        {
                            if (i + 1 > ToolTipUI.MaxLines)
                                break;
                            compiledTooltip[i + 1] = itemsBaseTooltip.GetLine(i);
                        }
                        for (int i = 0; i < activePassiveTooltips.Count; i++)
                        {
                            if (i + 1 + baseTooltipLines > ToolTipUI.MaxLines)
                                break;
                            compiledTooltip[i + 1 + baseTooltipLines] = activePassiveTooltips[i];
                        }

                        TerraLeague.instance.tooltipUI.DrawText(compiledTooltip);
                    }
                    else
                    {
                        string heading = TerraLeague.CreateColorString(TerraLeague.TooltipHeadingColor, Lang.GetItemName(Main.LocalPlayer.armor[slotNum + 2].type).Value);
                        var itemsBaseTooltip = Lang.GetTooltip(Main.LocalPlayer.armor[slotNum + 2].type);

                        string[] compiledTooltip = new string[ToolTipUI.MaxLines];
                        compiledTooltip[0] = heading;

                        for (int i = 0; i < itemsBaseTooltip.Lines; i++)
                        {
                            if (i + 1 > ToolTipUI.MaxLines)
                                break;
                            compiledTooltip[i + 1] = itemsBaseTooltip.GetLine(i);
                        }
                        TerraLeague.instance.tooltipUI.DrawText(compiledTooltip);
                    }
                }
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

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y - 2);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            base.DrawSelf(spriteBatch);
        }
    }

    class UIStatPanel : UIState
    {
        bool moveMode = false;
        float mouseXOri = 0;
        float mouseYOri = 0;
        public float LeftPercent = 0;
        public float TopPercent = 0;

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
        UIText manaStats;

        UIText tooltip;

        Texture2D _backgroundTexture;

        public UIStatPanel(float left, float top, int width, int height, Color color)
        {
            SetPadding(0);
            Left.Set(0, left);
            Top.Set(0, top);
            LeftPercent = left;
            TopPercent = top;
            Width.Set(width, 0f);
            Height.Set(height, 0f);
            if (_backgroundTexture == null)
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/StatsBackgroundSmall");

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
            CDRStats.TextColor = TerraLeague.ConvertHexToColor(TerraLeague.CDRColor);

            healStats = new UIText("HEAL: 000%", 0.65f);
            healStats.Left.Pixels = 80;
            healStats.Top.Pixels = 54;
            healStats.TextColor = Color.Green;

            ammoStats = new UIText("AMMO: 100%", 0.65f);
            ammoStats.Left.Pixels = 8;
            ammoStats.Top.Pixels = 70;
            ammoStats.TextColor = Color.Gray;

            manaStats = new UIText("MANA: 000%", 0.65f);
            manaStats.Left.Pixels = 80;
            manaStats.Top.Pixels = 70;
            manaStats.TextColor = Color.RoyalBlue;


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
            Append(manaStats);
            Append(tooltip);
        }

        public override void Update(GameTime gameTime)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            if (moveMode)
            {
                Left.Set(0, LeftPercent + ((Main.MouseScreen.X - mouseXOri) / (float)Main.screenWidth));
                Top.Set(0, TopPercent + ((Main.MouseScreen.Y - mouseYOri) / (float)Main.screenHeight));
            }
            else
            {
                var dimentions = this.GetDimensions();
                if (dimentions.X < 0)
                {
                    LeftPercent = 0;
                    Left.Set(0, LeftPercent);
                }
                if (dimentions.X + Width.Pixels > Main.screenWidth)
                {
                    LeftPercent = (Main.screenWidth - Width.Pixels) / (float)Main.screenWidth;
                    Left.Set(0, LeftPercent);
                }
                if (dimentions.Y < 0)
                {
                    TopPercent = 0;
                    Top.Set(0, TopPercent);
                }
                if (dimentions.Y + Height.Pixels > Main.screenHeight)
                {
                    TopPercent = (Main.screenHeight - Height.Pixels) / (float)Main.screenHeight;
                    Top.Set(0, TopPercent);
                }
            }

            armorStats.Width.Set(30,0);
            resistStats.Width.Set(30,0);
            meleeStats.Width.Set(30,0);
            rangedStats.Width.Set(30,0);
            magicStats.Width.Set(30,0);
            summonStats.Width.Set(30,0);
            CDRStats.Width.Set(30,0);
            healStats.Width.Set(30,0);
            ammoStats.Width.Set(30,0);

            manaStats.Width.Set(30,0);

            if (extraStats)
            {
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/StatsBackgroundLarge");

                Height.Set(90, 0f);
                GetStats(true);
            }
            else
            {
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/StatsBackgroundSmall");

                Height.Set(58, 0f);
                GetStats();
            }

            string text = "";

            if (!moveMode)
            {
                if (armorStats.IsMouseHovering)
                {
                    text = TerraLeague.CreateColorString(TerraLeague.ARMORColor, "Armor") +
                        "\nReduces damage from contact by " + Math.Round(100 - (modPlayer.ArmorDamageReduction * 100f), 2) + "%" +
                        "\nCurrent Armor consists of" +
                        "\n" + TerraLeague.CreateColorString(TerraLeague.ARMORColor, "From Armor increases: " + modPlayer.armorLastStep);
                    if (TerraLeague.UseCustomDefenceStat)
                        text += "\n" + TerraLeague.CreateColorString(TerraLeague.DEFColor, "From Defence increases: " + modPlayer.defenceLastStep);
                }
                else if (resistStats.IsMouseHovering)
                {
                    text = TerraLeague.CreateColorString(TerraLeague.RESISTColor, "Resist") +
                        "\nReduces damage from projectiles by " + Math.Round(100 - (modPlayer.ResistDamageReduction * 100f), 2) + "%" +
                        "\nCurrent Resist consists of" +
                        "\n" + TerraLeague.CreateColorString(TerraLeague.RESISTColor, "From Resist increases: " + modPlayer.resistLastStep);
                    if (TerraLeague.UseCustomDefenceStat)
                        text += "\n" + TerraLeague.CreateColorString(TerraLeague.DEFColor, "From Defence increases: " + modPlayer.defenceLastStep);
                }
                else if (meleeStats.IsMouseHovering)
                {
                    text = TerraLeague.CreateColorString(TerraLeague.MELColor, "Melee Damage") +
                        "\nUsed for Abilities and Items scaling damage. Gain a flat amount that increases throughout the game plus 1.5 per 1% melee damage" +
                        "\nMelee Weapons Deal " + (int)(modPlayer.meleeDamageLastStep * 100) + "% damage." +
                        "\nExtra Damage: +" + modPlayer.meleeFlatDamage +
                        "\nCrit Chance: +" + (modPlayer.player.meleeCrit - modPlayer.player.HeldItem.crit - 4) + "%" +
                        "\nLife Steal: " + (int)(modPlayer.lifeStealMelee) +
                        "\nFlat On Hit: " + modPlayer.meleeOnHit +
                        "\nArmor Penetration: " + (modPlayer.meleeArmorPen + modPlayer.player.armorPenetration);
                }
                else if (rangedStats.IsMouseHovering)
                {
                    text = TerraLeague.CreateColorString(TerraLeague.RNGColor, "Ranged Damage") +
                        "\nUsed for Abilities and Items scaling damage. Gain 2 per 1% ranged damage" +
                        "\nRanged Weapons Deal " + (int)(modPlayer.rangedDamageLastStep * 100) + "% damage." +
                        "\nExtra Damage: +" + modPlayer.rangedFlatDamage +
                        "\nCrit Chance: +" + (modPlayer.player.rangedCrit - modPlayer.player.HeldItem.crit - 4) + "%" +
                        "\nLife Steal: " + (int)(modPlayer.lifeStealRange) +
                        "\nFlat On Hit: " + modPlayer.rangedOnHit +
                        "\nArmor Penetration: " + (modPlayer.rangedArmorPen + modPlayer.player.armorPenetration);
                }
                else if (magicStats.IsMouseHovering)
                {
                    text = TerraLeague.CreateColorString(TerraLeague.MAGColor, "Magic Damage") +
                        "\nUsed for Abilities and Items scaling damage. Gain 2.5 per 1% magic damage" +
                        "\nMagic Weapons Deal " + (int)(modPlayer.magicDamageLastStep * 100) + "% damage." +
                        "\nExtra Damage: +" + modPlayer.magicFlatDamage +
                        "\nCrit Chance: +" + (modPlayer.player.magicCrit - modPlayer.player.HeldItem.crit - 4) + "%" +
                        "\nLife Steal: " + (int)(modPlayer.lifeStealMagic) +
                        "\nFlat On Hit: " + modPlayer.magicOnHit +
                        "\nArmor Penetration: " + (modPlayer.magicArmorPen + modPlayer.player.armorPenetration);
                }
                else if (summonStats.IsMouseHovering)
                {
                    text = TerraLeague.CreateColorString(TerraLeague.SUMColor, "Summon Damage") +
                        "\nUsed for Abilities and Items scaling damage. Gain 1.75 per 1% minion damage" +
                        "\nSummoner Weapons Deal " + (int)(modPlayer.minionDamageLastStep * 100) + "% damage." +
                        "\nExtra Damage: +" + modPlayer.minionFlatDamage +
                        "\nLife Steal: " + (int)(modPlayer.lifeStealMinion) +
                        "\nFlat On Hit: " + modPlayer.meleeOnHit +
                        "\nArmor Penetration: " + (modPlayer.minionArmorPen + modPlayer.player.armorPenetration) +
                        "\nMinions: " + (modPlayer.player.maxMinions) +
                        " ~ Sentries: " + (modPlayer.player.maxTurrets);
                }
                else if (CDRStats.IsMouseHovering)
                {
                    text = TerraLeague.CreateColorString(TerraLeague.CDRColor, "Haste") +
                        "\nThe percent increase in spell/item casts" +
                        "\nAbility Haste: " + modPlayer.abilityHaste + " (" + Math.Round(100 - (modPlayer.Cdr * 100), 2) + "% reduction)" +
                        "\nItem Haste: " + modPlayer.itemHaste + " (" + Math.Round(100 - (modPlayer.ItemCdr * 100), 2) + "% reduction)" +
                        "\nSummoner Spell Haste: " + modPlayer.summonerHaste + " (" + Math.Round(100 - (modPlayer.SummonerCdr * 100), 2) + "% reduction)";
                }
                else if (ammoStats.IsMouseHovering)
                {
                    text = TerraLeague.CreateColorString(TerraLeague.RNGATSColor, "Ranged Attack Speed") +
                        "\nThe percent increase in ranged weapons attack speed";

                    Item item = Main.LocalPlayer.HeldItem;
                    if (item.ranged)
                    {
                        float useTimeMulti = item.GetGlobalItem<Items.TerraLeagueITEMGLOBAL>().UseTimeMultiplier(item, Main.LocalPlayer);
                        text += "\n" + item.Name + " fires " + Math.Round(60f / (item.useTime * (2 - useTimeMulti)), 2) + " times per second";
                    }
                    else
                    {
                        text += "\nHold a Ranged Weapon to see its fire rate";
                    }
                }
                else if (healStats.IsMouseHovering)
                {
                    text = TerraLeague.CreateColorString(TerraLeague.HEALColor, "Heal Power") +
                        "\nThe percent increase in all your outgoing healing and shielding";
                }
                else if (manaStats.IsMouseHovering)
                {
                    text = TerraLeague.CreateColorString(TerraLeague.MANAREDUCTColor, "Mana Cost Reduction") +
                        "\nThe percent reduction of all mana costs";
                }
            }

            if (text != "")
                TerraLeague.instance.tooltipUI.DrawText(text.Split('\n'));
            base.Update(gameTime);
        }

        public override void RightMouseDown(UIMouseEvent evt)
        {
            moveMode = true;

            mouseXOri = Main.MouseScreen.X;
            mouseYOri = Main.MouseScreen.Y;

            base.RightMouseDown(evt);
        }

        public override void RightMouseUp(UIMouseEvent evt)
        {
            moveMode = false;

            LeftPercent = Left.Percent;
            TopPercent = Top.Percent;

            base.RightMouseUp(evt);
        }

        public void GetStats(bool extra = false)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            armorStats.SetText("ARM: " + (modPlayer.armor + (TerraLeague.UseCustomDefenceStat ? modPlayer.defenceLastStep : 0)));
            resistStats.SetText("RST: " + (modPlayer.resist + (TerraLeague.UseCustomDefenceStat ? modPlayer.defenceLastStep : 0)));
            meleeStats.SetText("MEL: " + modPlayer.MEL);
            rangedStats.SetText("RNG: " + modPlayer.RNG);
            magicStats.SetText("MAG: " + modPlayer.MAG);
            summonStats.SetText("SUM: " + modPlayer.SUM);

            if (extra)
            {
                ammoStats.SetText("ATS: " + (Math.Round(modPlayer.rangedAttackSpeed * 100)).ToString() + "%");
                healStats.SetText("HEAL: " + (modPlayer.healPower * 100).ToString() + "%");
                CDRStats.SetText("HST: " + modPlayer.abilityHaste);
                manaStats.SetText("MANA: " + ((int)((1 - modPlayer.player.manaCost) * 100)).ToString() + "%");
            }
            else
            {
                ammoStats.SetText("");
                healStats.SetText("");
                CDRStats.SetText("");
                manaStats.SetText("");
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y - 2);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            base.DrawSelf(spriteBatch);
        }
    }
}