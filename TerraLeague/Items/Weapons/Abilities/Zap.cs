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
    public class Zap : Ability
    {
        public Zap(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "ZAP!";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Zap";
        }

        public override string GetAbilityTooltip()
        {
            return "Fire a shock blast that slows";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 1.5);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 160;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 7;
        }

        public override int GetBaseManaCost()
        {
            return 15;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.RNG) + " ranged damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                Vector2 position = player.MountedCenter;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 8f);
                int projType = ProjectileType<PowPow_Zap>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG);
                int knockback = 3;

                SetAnimation(player, 20, 20, position + velocity);
                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                SetCooldowns(player, type);
            }
        }
    }
}
