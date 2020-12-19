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
    public class DeathLotus : Ability
    {
        public DeathLotus(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Death Lotus";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/DeathBlossom";
        }

        public override string GetAbilityTooltip()
        {
            return "Spin in place for 2.5 seconds, rapidly throwing daggers in all directions";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MEL:
                    return 15;
                case DamageType.MAG:
                    return 12;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 40;
        }

        public override int GetBaseManaCost()
        {
            return 50;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MEL) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage per dagger";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                player.AddBuff(BuffType<DeathBlossom>(), GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG));
                SetCooldowns(player, type);
            }
        }
    }
}
