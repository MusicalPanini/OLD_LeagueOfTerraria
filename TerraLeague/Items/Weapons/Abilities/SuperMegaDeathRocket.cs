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
    public class SuperMegaDeathRocket : Ability
    {
        public SuperMegaDeathRocket(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Super Mega Death Rocket!";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/SMDR";
        }

        public override string GetAbilityTooltip()
        {
            return "Fire a giant rocket that gains damage the longer it flys";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 5 * player.GetModPlayer<PLAYERGLOBAL>().rocketDamageLastStep);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 150;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 30;
        }

        public override int GetBaseManaCost()
        {
            return 150;
        }

        public override string GetDamageTooltip(Player player)
        {
            return "Up to" + GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.RNG) + " ranged damage";
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
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 2.5f);
                int projType = ProjectileType<FishBones_SMDR>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG);
                int knockback = 6;

                SetAnimation(player, abilityItem.item.useTime, abilityItem.item.useAnimation, position + velocity);
                DoEfx(player, type);
                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);

                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
        }
    }
}
