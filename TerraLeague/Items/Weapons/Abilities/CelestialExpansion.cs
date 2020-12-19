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
    public class CelestialExpansion : Ability
    {
        public CelestialExpansion(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Celestial Expansion";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/CelestialExpansion";
        }

        public override string GetAbilityTooltip()
        {
            return "Expands the range and size of your Forged Stars and increases all minion damage output";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return 20;
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.SUM:
                    return 15;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 1;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetTooltip()
        {
            return (GetDamageTooltip(Main.LocalPlayer) == "" ? "" : GetDamageTooltip(Main.LocalPlayer) + "\n") +
            (GetBaseManaCost() == 0 ? "" : "Uses " + GetScaledManaCost() + " mana + 30 per second\n") +
            (GetAbilityTooltip() == "" ? "" : GetAbilityTooltip());
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.SUM) + " flat increased summon damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.HasBuff(BuffType<Buffs.CelestialExpansion>()))
            {
                player.ClearBuff(BuffType<Buffs.CelestialExpansion>());
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
            else if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                player.AddBuff(BuffType<Buffs.CelestialExpansion>(), 61);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 45, -0.5f);
        }

        public static int GetBonusDamage(PLAYERGLOBAL player)
        {
            return (int)(20 + (player.SUM * 0.15));
        }
    }
}
