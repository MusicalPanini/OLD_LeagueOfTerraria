using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class Pyroclasm : Ability
    {
        public Pyroclasm(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Pyroclasm";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Pyroclasm";
        }

        public override string GetAbilityTooltip()
        {
            return "Launch a homing fireball at a target that bounces between enemies 10 times" +
                    "\nDeals double damage to enemies who are 'Ablaze' and causes an explosion";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 4);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 35;
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
            return 100;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (TerraLeague.NPCMouseIsHovering(30, true) != -1)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
                {
                    Vector2 position = player.position;
                    Vector2 velocity = new Vector2(0, 0);
                    int projType = ProjectileType<BurningVengance_Pyroclasm>();
                    int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);
                    int knockback = 0;

                    SetAnimation(player, 10, 10, position + velocity);
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, TerraLeague.NPCMouseIsHovering(30, true), -1);
                    SetCooldowns(player, type);
                }
            }
        }
    }
}
