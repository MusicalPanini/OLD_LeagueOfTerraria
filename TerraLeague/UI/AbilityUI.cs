using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using TerraLeague;
using Terraria.UI;
using System;
using Terraria.ID;
using System.Linq;
using Terraria.Graphics;
using System.Collections.Generic;
using TerraLeague.Items.Weapons;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague.UI
{
    internal class AbilityUI : UIState
    {
        PLAYERGLOBAL modPlayer;
        public UIElement MainPanel;
        Texture2D _backgroundTexture;
        public static bool visible = true;
        AbilitySlotUI abilityPanel1;
        AbilitySlotUI abilityPanel2;
        AbilitySlotUI abilityPanel3;
        AbilitySlotUI abilityPanel4;

        UIText toolTip = new UIText("", 1);

        public override void OnInitialize()
        {
            if (_backgroundTexture == null)
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/AbilityBackground");
            MainPanel = new UIElement();
            MainPanel.SetPadding(0);
            MainPanel.Width.Set(195, 0f);
            MainPanel.Height.Set(54, 0f);
            MainPanel.Left.Set(Main.screenWidth / 2 - MainPanel.Width.Pixels / 2, 0f);
            MainPanel.Top.Set(Main.screenHeight - (MainPanel.Height.Pixels + 44), 0f);

            int abilityboxsize = 44;

            abilityPanel1 = new AbilitySlotUI(5, 5, abilityboxsize, AbilityType.Q);
            abilityPanel2 = new AbilitySlotUI(52, 5, abilityboxsize, AbilityType.W);
            abilityPanel3 = new AbilitySlotUI(99, 5, abilityboxsize, AbilityType.E);
            abilityPanel4 = new AbilitySlotUI(146, 5, abilityboxsize, AbilityType.R);

            toolTip.Left.Set(0, 0);
            toolTip.Top.Set(0, 0);
            toolTip.Width.Set(500, 0);
            toolTip.Top.Set(0, 0);

            MainPanel.Append(abilityPanel1);
            MainPanel.Append(abilityPanel2);
            MainPanel.Append(abilityPanel3);
            MainPanel.Append(abilityPanel4);
            MainPanel.Append(toolTip);

            base.Append(MainPanel);
            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            MainPanel.Top.Set(Main.screenHeight - (MainPanel.Height.Pixels + 44), 0f);
            MainPanel.Left.Set((Main.screenWidth / 2) - (MainPanel.Width.Pixels / 2), 0f);

            modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            if (abilityPanel1.IsMouseHovering)
                SetToolTip(modPlayer.Abilities[0], AbilityType.Q);
            else if (abilityPanel2.IsMouseHovering)
                SetToolTip(modPlayer.Abilities[1], AbilityType.W);
            else if (abilityPanel3.IsMouseHovering)
                SetToolTip(modPlayer.Abilities[2], AbilityType.E);
            else if (abilityPanel4.IsMouseHovering)
                SetToolTip(modPlayer.Abilities[3], AbilityType.R);
            else
                toolTip.SetText("");

            Recalculate();
            RecalculateChildren();
            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = MainPanel.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);

            base.DrawSelf(spriteBatch);
        }

        void SetToolTip(Ability ability, AbilityType type)
        {
            if (ability != null)
            {
                List<string> tooltip = new List<string>();
                tooltip.Add(TerraLeague.CreateColorString(TerraLeague.TooltipHeadingColor, ability.GetAbilityName()));
                if (ability.GetDamageTooltip(Main.LocalPlayer) != "")
                    tooltip.AddRange(ability.GetDamageTooltip(Main.LocalPlayer).Split('\n'));
                tooltip.AddRange(ability.GetAbilityTooltip().Split('\n'));
                tooltip.Add(ability.GetCooldown() + " second cooldown");

                TerraLeague.instance.tooltipUI.DrawText(tooltip.ToArray());
            }
        }
    }

    public class AbilitySlotUI : UIElement
    {
        Texture2D nullImage;
        Texture2D clear;
        Texture2D _backgroundTexture;

        AbilityType abilityType;
        UIText slotNum;
        UIText slotMana;
        UIText slotCD;
        UIImage slotIcon;
        UIImage slotOOM;
        UIImage slotSpecialCast;

        public AbilitySlotUI(int left, int top, int length, AbilityType type)
        {
            if (_backgroundTexture == null)
                _backgroundTexture = TerraLeague.instance.GetTexture("UI/AbilityBorder");

            abilityType = type;
            nullImage = TerraLeague.instance.GetTexture("AbilityImages/Template");
            clear = TerraLeague.instance.GetTexture("AbilityImages/Clear");

            Left.Set(left, 0);
            Top.Set(top, 0);
            Width.Set(length, 0);
            Height.Set(length, 0);

            slotIcon = new UIImage(nullImage);
            slotIcon.Left.Pixels = 5.5f;
            slotIcon.Top.Pixels = 6;
            Append(slotIcon);

            slotOOM = new UIImage(clear);
            slotOOM.Left.Pixels = 5.5f;
            slotOOM.Top.Pixels = 6;
            Append(slotOOM);

            slotSpecialCast = new UIImage(clear);
            slotSpecialCast.Left.Pixels = 5.5f;
            slotSpecialCast.Top.Pixels = 6;
            Append(slotSpecialCast);

            slotNum = new UIText(((int)abilityType + 1).ToString(), 0.75f);
            slotNum.Left.Pixels = 2;
            slotNum.Top.Pixels = 30;
            Append(slotNum);

            slotMana = new UIText("", 0.75f);
            slotMana.Left.Pixels = 22;
            slotMana.Top.Pixels = 0;
            slotMana.TextColor = Color.CornflowerBlue;
            Append(slotMana);

            slotCD = new UIText("", 1);
            slotCD.Left.Pixels = 15;
            slotCD.Top.Pixels = 12;
            Append(slotCD);

        }

        public override void Update(GameTime gameTime)
        {
            if (GetIfAbilityExists(abilityType))
            {
                slotIcon.SetImage(GetImage(abilityType));
                slotMana.SetText(GetCost(abilityType));
                slotIcon.ImageScale = 1;
            }
            else
            {
                slotIcon.SetImage(nullImage);
                slotOOM.SetImage(clear);
                slotSpecialCast.SetImage(clear);
                slotIcon.ImageScale = 0;
                slotMana.SetText("");
            }

            Recalculate();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);

            string slotNumText = "N/A";
            switch (abilityType)
            {
                case AbilityType.Q:
                    slotNumText = TerraLeague.ConvertKeyString(TerraLeague.QAbility);
                    break;
                case AbilityType.W:
                    slotNumText = TerraLeague.ConvertKeyString(TerraLeague.WAbility);
                    break;
                case AbilityType.E:
                    slotNumText = TerraLeague.ConvertKeyString(TerraLeague.EAbility);
                    break;
                case AbilityType.R:
                    slotNumText = TerraLeague.ConvertKeyString(TerraLeague.RAbility);
                    break;
                default:
                    break;
            }
            slotNum.SetText(slotNumText);

            slotCD.SetText(GetCooldown(abilityType));
            slotCD.Left.Pixels = 0;
            slotCD.Top.Pixels = 12;
            slotCD.Width.Pixels = this.Width.Pixels;
            slotCD.HAlign = 0.5f;

            if (GetIfAbilityExists(abilityType))
            {
                PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

                slotIcon.SetImage(GetImage(abilityType));

                if (modPlayer.Abilities[(int)abilityType].GetScaledManaCost() > Main.LocalPlayer.statMana)
                    slotOOM.SetImage(TerraLeague.instance.GetTexture("AbilityImages/OOM"));
                else if(!Ability.CheckIfNotOnCooldown(Main.LocalPlayer, abilityType) || !modPlayer.Abilities[(int)abilityType].CanCurrentlyBeCast(Main.LocalPlayer))
                    slotOOM.SetImage(TerraLeague.instance.GetTexture("AbilityImages/CantCast"));
                else
                    slotOOM.SetImage(TerraLeague.instance.GetTexture("AbilityImages/Clear"));

                if (modPlayer.Abilities[(int)abilityType].CurrentlyHasSpecialCast(Main.LocalPlayer))
                    slotSpecialCast.SetImage(TerraLeague.instance.GetTexture("AbilityImages/SpecialCast"));
                else
                    slotSpecialCast.SetImage(clear);


                slotMana.SetText(GetCost(abilityType));
                slotIcon.ImageScale = 1;
            }
            else
            {
                slotIcon.SetImage(nullImage);
                slotOOM.SetImage(clear);
                slotSpecialCast.SetImage(clear);
                slotIcon.ImageScale = 0;
                slotMana.SetText("");
            }

            Recalculate();
            base.Draw(spriteBatch);
        }

        string GetCost(AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            try
            {
                if (modPlayer.Abilities[(int)type].GetScaledManaCost() > 0)
                    return modPlayer.Abilities[(int)type].GetScaledManaCost().ToString();
                else
                    return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        Texture2D GetImage(AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            try
            {
                return TerraLeague.instance.GetTexture(modPlayer.Abilities[(int)type].GetIconTexturePath());
            }
            catch (Exception)
            {
                return TerraLeague.instance.GetTexture("AbilityImages/Template");
            }
        }

        string GetCooldown(AbilityType type)
        {
            float cooldown = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().AbilityCooldowns[(int)type];

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

        bool GetIfAbilityExists(AbilityType type)
        {
            if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().Abilities[(int)type] != null)
                return true;
            else
                return false;
        }
    }
}
