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
    public class ReapingSlash : Ability
    {
        public ReapingSlash(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Reaping Slash";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/ReapingSlash";
        }

        public override string GetAbilityTooltip()
        {
            return "Lunge towards the curser, swinging the scythe around you";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 2);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MEL:
                    return 65;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 12;
        }

        public override int GetBaseManaCost()
        {
            return 30;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MEL) + " melee damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                player.velocity = TerraLeague.CalcVelocityToMouse(player.Center, 10f);
                Vector2 position = player.Center;
                Vector2 velocity = Vector2.Zero;
                int projType = ProjectileType<DarkinScythe_ReapingSlash>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MEL);
                int knockback = 4;

                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, player.velocity.X > 0 ? 1 : -1);
                SetCooldowns(player, type);
            }
        }
    }
}
