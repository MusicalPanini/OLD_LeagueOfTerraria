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
    public class Starcall : Ability
    {
        public Starcall(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Starcall";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Starcall";
        }

        public override string GetAbilityTooltip()
        {
            return "Call down a star to impact the surface." +
                    "\nGain 'Rejuvenation' for 5 seconds if you damage an enemy.";
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
            return 6;
        }

        public override int GetBaseManaCost()
        {
            return 20;
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
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                Vector2 position = player.Center;
                position.Y -= 600;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12f);
                int projType = ProjectileType<CelestialStaff_Starcall>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);

                Projectile.NewProjectile(position, velocity, projType, damage, 0, player.whoAmI);
                SetAnimation(player, 10, 10, Main.MouseWorld);
                SetCooldowns(player, type);
            }
        }
    }
}
