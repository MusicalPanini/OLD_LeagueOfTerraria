using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    /// <summary>
    /// <para>An AbilityItem is an item that can house up to 4 abilities (AbilityType: Q, W, E, and R) and a hand full of methods to support them.</para>
    /// <para>Each cycle, the game will check the players hand and inventory for one of each of the ability type that may exist within them using the DoesAbilityExist(AbilityType) Method.</para>
    /// <para>If this method returns true, it will start to construct the ability using the sucessful type and supporting methods.</para>
    /// </summary>
    abstract public class AbilityItem : ModItem
    {
        internal AbilitiesPacketHandler PacketHandler = new AbilitiesPacketHandler(7);

        /// <summary>
        /// <para>Returns the path for the requested AbilityType's Icons Texture</para>
        /// Override to set image path.
        /// Default returns: "AbilityImages/Template"
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        virtual public string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/Template";
            else if (type == AbilityType.W)
                return "AbilityImages/Template";
            else if (type == AbilityType.E)
                return "AbilityImages/Template";
            else if (type == AbilityType.R)
                return "AbilityImages/Template";
            else
                return "AbilityImages/Template";
        }

        /// <summary>
        /// <para>Modifies the Items tooltip</para>
        /// Recommended to not override.
        /// </summary>
        /// <param name="tooltips"></param>
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            if (tt != null)
            {
                int pos = tooltips.IndexOf(tt);

                string text = GetWeaponTooltip() != "" ? "\n" + GetWeaponTooltip() : "";
                if (GetIfAbilityExists(AbilityType.Q))
                {
                    text += "\n[c/2eb82e:Ability " + TerraLeague.ConvertKeyString(TerraLeague.QAbility) + ":] [c/5cd65c:" + GetAbilityName(AbilityType.Q) + "]" +
                        "\n" + GetTooltip(AbilityType.Q);
                }
                if (GetIfAbilityExists(AbilityType.W))
                {
                    text += "\n[c/2eb82e:Ability " + TerraLeague.ConvertKeyString(TerraLeague.WAbility) + ":] [c/5cd65c:" + GetAbilityName(AbilityType.W) + "]" +
                        "\n" + GetTooltip(AbilityType.W);
                }
                if (GetIfAbilityExists(AbilityType.E))
                {
                    text += "\n[c/2eb82e:Ability " + TerraLeague.ConvertKeyString(TerraLeague.EAbility) + ":] [c/5cd65c:" + GetAbilityName(AbilityType.E) + "]" +
                        "\n" + GetTooltip(AbilityType.E);
                }
                if (GetIfAbilityExists(AbilityType.R))
                {
                    text += "\n[c/2eb82e:Ability " + TerraLeague.ConvertKeyString(TerraLeague.RAbility) + ":] [c/5cd65c:" + GetAbilityName(AbilityType.R) + "]" +
                        "\n" + GetTooltip(AbilityType.R);
                }
                text += "\n[c/cc9900:'" + GetQuote() + "']";
                
                string[] lines = text.Split('\n');
                tooltips.RemoveAt(pos);


                for (int i = 1; i < lines.Count(); i++)
                {
                    tooltips.Insert(pos + i-1, new TooltipLine(TerraLeague.instance, "Tooltip" + (i-1), lines[i]));
                }
            }

            base.ModifyTooltips(tooltips);
        }

        /// <summary>
        /// <para>Returns the Weapons tooltip</para>
        /// Override to set
        /// </summary>
        /// <returns></returns>
        virtual public string GetWeaponTooltip()
        {
            return "";
        }

        /// <summary>
        /// <para>Returns the requested AbilityType's tooltip</para>
        /// Default Returns: "This ability does a thing"
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        virtual public string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "This ability does a thing";
            else if (type == AbilityType.W)
                return "This ability does a thing";
            else if (type == AbilityType.E)
                return "This ability does a thing";
            else if (type == AbilityType.R)
                return "This ability does a thing";
            else
                return "This ability does a thing";
        }

        /// <summary>
        /// Returns the Champions quote for the Weapon.
        /// </summary>
        /// <returns></returns>
        virtual public string GetQuote()
        {
            return "";
        }

        /// <summary>
        /// <para>Returns a complete tooltip for the requested AbilityType consisting of the damage: GetDamageTooltip(type), mana cost: GetBaseManaCost(type), and ability description: GetAbilityTooltip(type)</para>
        /// Can be overriden to suit needs, but not necessary
        /// </summary>
        /// <returns></returns>
        virtual public string GetTooltip(AbilityType type)
        {
            return (GetDamageTooltip(Main.LocalPlayer, type) == "" ? "" : GetDamageTooltip(Main.LocalPlayer, type) + "\n") +
            (GetBaseManaCost(type) == 0 ? "" : "Uses " + GetScaledManaCost(type) + " mana\n") +
            (GetAbilityTooltip(type) == "" ? "" : GetAbilityTooltip(type));
        }

        /// <summary>
        /// <para>Returns if the requested AbilityType actually exists on the item</para>
        /// Default returns false
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        virtual public bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q)
                return false;
            else if (type == AbilityType.W)
                return false;
            else if (type == AbilityType.E)
                return false;
            else if (type == AbilityType.R)
                return false;
            else
                return false;
        }

        /// <summary>
        /// <para>Gets the base damage of the requested AbilityType</para>
        /// Default returns the items base damage
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        virtual public int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return item.damage;
            else if (type == AbilityType.W)
                return item.damage;
            else if (type == AbilityType.E)
                return item.damage;
            else if (type == AbilityType.R)
                return item.damage;
            else
                return item.damage;
        }

        /// <summary>
        /// <para>Returns the scaling amount for a specific DamageType for the requested AbilityType</para>
        /// View method implementation for an example of how to set up. default returns 0
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <param name="dam"></param>
        /// <returns></returns>
        virtual public int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.MEL)
                    return 0;
                else if (dam == DamageType.RNG)
                    return 0;
                else if (dam == DamageType.MAG)
                    return 0;
                else if (dam == DamageType.SUM)
                    return 0;
            }
            else if(type == AbilityType.W)
            {
                if (dam == DamageType.MEL)
                    return 0;
                else if (dam == DamageType.RNG)
                    return 0;
                else if (dam == DamageType.MAG)
                    return 0;
                else if (dam == DamageType.SUM)
                    return 0;
            }
            else if (type == AbilityType.E)
            {
                if (dam == DamageType.MEL)
                    return 0;
                else if (dam == DamageType.RNG)
                    return 0;
                else if (dam == DamageType.MAG)
                    return 0;
                else if (dam == DamageType.SUM)
                    return 0;
            }
            else if (type == AbilityType.R)
            {
                if (dam == DamageType.MEL)
                    return 0;
                else if (dam == DamageType.RNG)
                    return 0;
                else if (dam == DamageType.MAG)
                    return 0;
                else if (dam == DamageType.SUM)
                    return 0;
            }
            return 0;
        }

        /// <summary>
        /// <para>Returns the requested AbilityType's scaled damage of specific DamageType</para>
        /// It fetchs the Scaling amount of the DamageType and scales it with the players damage (eg: MEL, RNG, MAG, SUM)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <param name="dam"></param>
        /// <returns></returns>
        public int GetAbilityScalingDamage(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            int num = 0;

            switch (dam)
            {
                case DamageType.MEL:
                    num = (int)(modPlayer.MEL * GetAbilityScalingAmount(player, type, dam) * 0.01);
                    break;
                case DamageType.RNG:
                    num = (int)(modPlayer.RNG * GetAbilityScalingAmount(player, type, dam) * 0.01);
                    break;
                case DamageType.MAG:
                    num = (int)(modPlayer.MAG * GetAbilityScalingAmount(player, type, dam) * 0.01);
                    break;
                case DamageType.SUM:
                    num = (int)(modPlayer.SUM * GetAbilityScalingAmount(player, type, dam) * 0.01);
                    break;
                default:
                    break;
            }

            return num;
        }

        /// <summary>
        /// Gets the mana cost of the ability
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        virtual public int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 0;
            else if (type == AbilityType.W)
                return 0;
            else if (type == AbilityType.E)
                return 0;
            else if (type == AbilityType.R)
                return 0;
            else
                return 0;
        }

        /// <summary>
        /// Returns the mana cost of the requested AbilityType, scaled by the players current mana cost reduction
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetScaledManaCost(AbilityType type)
        {
            return (int)(GetBaseManaCost(type) * Main.LocalPlayer.manaCost);
        }

        /// <summary>
        /// <para>Returns the damage portion of the tooltip to tell what kind of damage the ability will do</para>
        /// <para>(eg, "Enemies with 'Deadly Venom' take" + GetAbilityDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " damage per stack")</para>
        /// Can also be used to denote healing or sheilding if you so choose
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        virtual public string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return "";
            else if (type == AbilityType.W)
                return "";
            else if (type == AbilityType.E)
                return "";
            else if (type == AbilityType.R)
                return "";
            else
                return "";
        }

        /// <summary>
        /// Returns the Cooldown of the requested AbilityType, adjusted by the players cooldown reduction
        /// </summary>
        /// <returns></returns>
        virtual public int GetCooldown(AbilityType type)
        {
            int cooldown = (int)(GetRawCooldown(type) * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().cdrLastStep);

            return cooldown < 1 ? 1 : cooldown;
        }

        /// <summary>
        /// <para>Returns the name of the requested AbilityType</para>
        /// Default returns "Ability X"
        /// </summary>
        virtual public string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Ability 1";
            else if (type == AbilityType.W)
                return "Ability 2";
            else if (type == AbilityType.E)
                return "Ability 3";
            else if (type == AbilityType.R)
                return "Ability 4";
            else
                return "Ability X";
        }

        /// <summary>
        /// <para>Returns the unscaled cooldown of the requested AbilityType</para>
        /// Default returns 0
        /// </summary>
        virtual public int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 0;
            else if (type == AbilityType.W)
                return 0;
            else if (type == AbilityType.E)
                return 0;
            else if (type == AbilityType.R)
                return 0;
            else
                return 0;
        }

        /// <summary>
        /// Returns if the requested AbilityType is currently on cooldown
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        virtual public bool CheckIfNotOnCooldown(Player player, AbilityType type)
        {
            return player.GetModPlayer<PLAYERGLOBAL>().AbilityCooldowns[(int)type] <= 0;
        }

        /// <summary>
        /// <para>Check if the requested AbilityType can be used while using other items (eg: Casting while swinging a sword)</para>
        /// Default returns false
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        virtual public bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        /// <summary>
        /// <para>Checks if the requested AbilityType can be cast based on Cooldowns, CC, using other items, etc.</para>
        /// Is used for UI to gray out the ability icon
        /// </summary>
        virtual public bool CanCurrentlyBeCast(Player player, AbilityType type)
        {
            if (CanBeCastWhileCCd(type) || !CanBeCastWhileCCd(type) && !player.noItems && !player.silence)
                if (CanBeCastWhileUsingItem(type) || !CanBeCastWhileUsingItem(type) && player.itemAnimation <= 0)
                    return true;

            return false;
        }

        /// <summary>
        /// <para>Check if the requested AbilityType can be used while Silenced, Cursed, etc.</para>
        /// Default returns false
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        virtual public bool CanBeCastWhileCCd(AbilityType type)
        {
            return false;
        }

        /// <summary>
        /// <para>Check if there is some kind of special interaction with the requested AbilityType. (eg: A second cast interaction or bonus for low health)</para>
        /// If true, will show a white out line on the ability icon. Default returns false
        /// </summary>
        virtual public bool CurrentlyHasSpecialCast(Player player, AbilityType type)
        {
            return false;
        }

        /// <summary>
        /// Sets the cooldowns of the requested AbilityType
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        protected void SetCooldowns(Player player, AbilityType type)
        {
            player.manaRegenDelay = 90;

            if (player.HasBuff(BuffType<SheenFlux>()))
            {

            }
            else if (player.GetModPlayer<PLAYERGLOBAL>().spellblade)
            {
                player.AddBuff(BuffType<SpellBlade>(), 240);
                player.AddBuff(BuffType<SheenFlux>(), 300);
            }

            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.AbilityCooldowns[(int)type] = (GetCooldown(type) * 60);
        }

        virtual public void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return;
            else if (type == AbilityType.W)
                return;
            else if (type == AbilityType.E)
                return;
            else if (type == AbilityType.R)
                return;
            else
                return;
        }

        /// <summary>
        /// Performs the effects of the requested AbilityType from Efx(Player, AbilityType) and sends to server if online and SendToServer is true
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <param name="SendToServer"></param>
        public void DoEfx(Player player, AbilityType type, bool SendToServer = true)
        {
            if (SendToServer && Main.netMode == NetmodeID.MultiplayerClient)
                PacketHandler.SendEfx(-1, player.whoAmI, item.type, player.whoAmI, type);

            Efx(player, type);
        }

        /// <summary>
        /// The effects that the requested AbilityType can do (eg: dust particals, sound effects)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        virtual public void Efx(Player player, AbilityType type)
        {
            return;
        }

        /// <summary>
        /// <para>Returns the requested AbilityType's scaling damage tooltip based on DamageType</para>
        /// Returns eg: "[c/00ffaa:123]"
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <param name="dam"></param>
        /// <returns></returns>
        protected string GetScalingTooltip(Player player, AbilityType type, DamageType dam)
        {
            string line = "";
            int Damage = GetAbilityScalingDamage(player, type, dam);

            switch (dam)
            {
                case DamageType.MEL:
                    line = "[c/" + TerraLeague.MELColor + ":" + Damage + "]";
                    break;
                case DamageType.RNG:
                    line = "[c/" + TerraLeague.RNGColor + ":" + Damage + "]";
                    break;
                case DamageType.MAG:
                    line = "[c/" + TerraLeague.MAGColor + ":" + Damage + "]";
                    break;
                case DamageType.SUM:
                    line = "[c/" + TerraLeague.SUMColor + ":" + Damage + "]";
                    break;
                default:
                    break;

                //case DamageType.MEL:
                //    line = "[c/" + TerraLeague.MELColor + ":" + Scaling + "% MEL(" + Damage + ")]";
                //    break;
                //case DamageType.RNG:
                //    line = "[c/" + TerraLeague.RNGColor + ":" + Scaling + "% RNG(" + Damage + ")]";
                //    break;
                //case DamageType.MAG:
                //    line = "[c/" + TerraLeague.MAGColor + ":" + Scaling + "% MAG(" + Damage + ")]";
                //    break;
                //case DamageType.SUM:
                //    line = "[c/" + TerraLeague.SUMColor + ":" + Scaling + "% SUM(" + Damage + ")]";
                //    break;
                //default:
                //    break;
            }

            return line;
        }

        protected virtual void SetAnimation(Player player, int useTime, int animationTime, Vector2 target)
        {
            player.GetModPlayer<PLAYERGLOBAL>().SetTempUseItem(item.type);

            float xDist = player.MountedCenter.X - target.X;
            float yDist = player.MountedCenter.Y - target.Y;

            int facing = -1;
            if (target.X < player.MountedCenter.X)
                facing = 1;

            player.itemRotation = (float)Math.Atan2((double)(yDist * (float)facing), (double)(xDist * (float)facing));

            player.ChangeDir(-facing);
            player.itemLocation = Vector2.Zero;
            player.itemAnimationMax = animationTime + 1;
            player.itemAnimation = animationTime;
            player.itemTime = useTime;
            NetMessage.SendData(MessageID.ItemAnimation, -1, -1, null, player.whoAmI, 0f, 0f, 0f, 0, 0, 0);

        }
    }
}
