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
    public class DancingGrenade : Ability
    {
        public DancingGrenade(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Dancing Grenade";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/DancingGranade";
        }

        public override string GetAbilityTooltip()
        {
            return "Launch a bouncing grenade that gains 44% more damage for each enemy it hits";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 0.75 * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().rocketDamageLastStep);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 75;
                case DamageType.MAG:
                    return 60;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 8;
        }

        public override int GetBaseManaCost()
        {
            return 30;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.RNG) + " + " + GetScalingTooltip(player, DamageType.MAG) + " ranged damage";
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
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 18f);
                velocity.Y *= 0.8f;
                int projType = ProjectileType<Whisper_DancingGrenade>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG) + GetAbilityScaledDamage(player, DamageType.MAG);
                int knockback = 4;

                SetAnimation(player, 20, 20, position + velocity);
                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
        }
    }
}
