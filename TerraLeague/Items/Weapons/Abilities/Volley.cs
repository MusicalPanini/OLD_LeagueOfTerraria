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
    public class Volley : Ability
    {
        public Volley(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Volley";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Volley";
        }

        public override string GetAbilityTooltip()
        {
            return "Fire 9 slowing shots in a cone";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().arrowDamageLastStep * 1.25);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 40;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 5;
        }

        public override int GetBaseManaCost()
        {
            return 30;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.RNG) + " ranged damage per arrow";
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
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 13f);
                int projType = ProjectileType<TrueIceBow_Volley>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG);
                int knockback = 1;

                int numberProjectiles = 9;
                float startingAngle = 30;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(startingAngle));
                    Projectile.NewProjectile(position, perturbedSpeed, projType, damage, knockback, player.whoAmI);
                    startingAngle -= 7.5f;
                }

                SetAnimation(player, 20, 20, position + velocity);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 5), player.Center);
        }
    }
}
