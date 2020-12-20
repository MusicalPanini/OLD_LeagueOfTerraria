using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public abstract class Ability
    {
        internal ModItem abilityItem;
        internal AbilitiesPacketHandler PacketHandler = new AbilitiesPacketHandler(7);

        virtual public string GetTooltip()
        {
            return (GetDamageTooltip(Main.LocalPlayer) == "" ? "" : GetDamageTooltip(Main.LocalPlayer) + "\n") +
            (GetBaseManaCost() == 0 ? "" : "Uses " + GetScaledManaCost() + " mana\n") +
            (GetAbilityTooltip() == "" ? "" : GetAbilityTooltip());
        }

        /// <summary>
        /// Gets the name of the Ability
        /// </summary>
        /// <returns></returns>
        virtual public string GetAbilityName()
        {
            return "Ability Name";
        }

        /// <summary>
        /// Gets the path for the ability icon. Default returns "AbilityImages/Template"
        /// </summary>
        /// <returns></returns>
        virtual public string GetIconTexturePath()
        {
            return "AbilityImages/Template";
        }

        /// <summary>
        /// Gets the Abilities tooltip text
        /// </summary>
        /// <returns></returns>
        virtual public string GetAbilityTooltip()
        {
            return "This ability does a thing";
        }

        /// <summary>
        /// Gets the base damage of the ability. Default returns 0
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        virtual public int GetAbilityBaseDamage(Player player)
        {
            return 0;
        }

        virtual public int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            if (dam == DamageType.MEL)
                return 0;
            else if (dam == DamageType.RNG)
                return 0;
            else if (dam == DamageType.MAG)
                return 0;
            else if (dam == DamageType.SUM)
                return 0;
            else
                return 0;
        }

        /// <summary>
        /// Gets the scaled damage for the requested DamageType
        /// </summary>
        /// <param name="player"></param>
        /// <param name="dam"></param>
        /// <returns></returns>
        public int GetAbilityScaledDamage(Player player, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            int num = 0;

            switch (dam)
            {
                case DamageType.MEL:
                    num = (int)(modPlayer.MEL * GetAbilityScalingAmount(player, dam) * 0.01);
                    break;
                case DamageType.RNG:
                    num = (int)(modPlayer.RNG * GetAbilityScalingAmount(player, dam) * 0.01);
                    break;
                case DamageType.MAG:
                    num = (int)(modPlayer.MAG * GetAbilityScalingAmount(player, dam) * 0.01);
                    break;
                case DamageType.SUM:
                    num = (int)(modPlayer.SUM * GetAbilityScalingAmount(player, dam) * 0.01);
                    break;
                default:
                    break;
            }

            return num;
        }

        virtual public int GetBaseManaCost()
        {
            return 0;
        }

        public int GetScaledManaCost()
        {
            return (int)(GetBaseManaCost() * Main.LocalPlayer.manaCost);
        }

        virtual public string GetDamageTooltip(Player player)
        {
            return "";
        }

        virtual public float GetCooldown()
        {
            float cooldown = (float)Math.Round(GetRawCooldown() * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().CdrLastStep, 1);

            return cooldown < 1 ? 1 : cooldown;
        }

        virtual public int GetRawCooldown()
        {
            return 0;
        }

        static public bool CheckIfNotOnCooldown(Player player, AbilityType type)
        {
            return player.GetModPlayer<PLAYERGLOBAL>().AbilityCooldowns[(int)type] <= 0;
        }

        virtual public bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        virtual public bool CanBeCastWhileCCd()
        {
            return false;
        }

        virtual public bool CanCurrentlyBeCast(Player player)
        {
            if (CanBeCastWhileCCd() || !CanBeCastWhileCCd() && !player.noItems && !player.silence)
                if (CanBeCastWhileUsingItem() || !CanBeCastWhileUsingItem() && player.itemAnimation <= 0)
                    return true;

            return false;
        }

        virtual public bool CurrentlyHasSpecialCast(Player player)
        {
            return false;
        }

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

            modPlayer.AbilityCooldowns[(int)type] = (int)(GetCooldown() * 60);
        }

        virtual public void DoEffect(Player player, AbilityType type)
        {
            return;
        }

        
        public void DoEfx(Player player, AbilityType type, bool SendToServer = true)
        {
            if (SendToServer && Main.netMode == NetmodeID.MultiplayerClient)
                PacketHandler.SendEfx(-1, player.whoAmI, abilityItem.item.type, player.whoAmI, type);

            Efx(player);
        }

        virtual public void Efx(Player player)
        {
            return;
        }

        protected string GetScalingTooltip(Player player, DamageType dam, bool useHealpower = false, string extraText = "")
        {
            string line = "";
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            switch (dam)
            {
                case DamageType.MEL:
                    line = TerraLeague.CreateScalingTooltip(dam, modPlayer.MEL, GetAbilityScalingAmount(player, dam), useHealpower, extraText);
                    break;
                case DamageType.RNG:
                    line = TerraLeague.CreateScalingTooltip(dam, modPlayer.RNG, GetAbilityScalingAmount(player, dam), useHealpower, extraText);
                    break;
                case DamageType.MAG:
                    line = TerraLeague.CreateScalingTooltip(dam, modPlayer.MAG, GetAbilityScalingAmount(player, dam), useHealpower, extraText);
                    break;
                case DamageType.SUM:
                    line = TerraLeague.CreateScalingTooltip(dam, modPlayer.SUM, GetAbilityScalingAmount(player, dam), useHealpower, extraText);
                    break;
                default:
                    break;
            }
            return line;
        }

        protected virtual void SetAnimation(Player player, int useTime, int animationTime, Vector2 target)
        {
            player.GetModPlayer<PLAYERGLOBAL>().SetTempUseItem(abilityItem.item.type);

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

        protected int GetPositionOfAbilityInArray(ModItem modItem)
        {
            var abilityItem = modItem.item.GetGlobalItem<AbilityItemGLOBAL>();
            for (int i = 0; i < 4; i++)
            {
                if (abilityItem.GetAbility((AbilityType)i) != null)
                    if (abilityItem.GetAbility((AbilityType)i).GetType() == GetType())
                        return i;
            }
            return -1;
        }
    }
}
