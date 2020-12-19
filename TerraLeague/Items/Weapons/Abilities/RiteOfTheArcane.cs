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
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class RiteOfTheArcane : Ability
    {
        public RiteOfTheArcane(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Rite of the Arcane";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/RiteoftheArcane";
        }

        public override string GetAbilityTooltip()
        {
            return "Channel for 3 seconds, barraging the area around you with magic artillery";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 75;
                default:
                    return 0;
            }
        }

        public override int GetBaseManaCost()
        {
            return 100;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override int GetRawCooldown()
        {
            return 80;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                player.AddBuff(BuffType<RiteoftheArcane>(), GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG));
                SetCooldowns(player, type);
            }
        }
    }
}
