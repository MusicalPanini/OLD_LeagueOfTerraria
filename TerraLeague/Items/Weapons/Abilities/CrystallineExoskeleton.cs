using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class CrystallineExoskeleton : Ability
    {
        public CrystallineExoskeleton(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Crystalline Exoskeleton";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/CrystallineExoskeleton";
        }

        public override string GetAbilityTooltip()
        {
            return "Gain 'Swiftness' and 10% of your max life as a shield for 6 seconds";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)((player.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep / 10));
        }

        public override int GetBaseManaCost()
        {
            return 45;
        }

        public override string GetDamageTooltip(Player player)
        {
            return TerraLeague.CreateScalingTooltip(UI.HealthbarUI.RedHealthColor.Hex3(), "LIFE", GetAbilityBaseDamage(player), 100, true) + " shielding";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override int GetRawCooldown()
        {
            return 12;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                player.AddBuff(BuffID.Swiftness, 360);
                modPlayer.AddShield(player.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(GetAbilityBaseDamage(player), true), 360, new Color(181, 77, 177), ShieldType.Basic);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 27, -0.5f);
        }
    }
}
