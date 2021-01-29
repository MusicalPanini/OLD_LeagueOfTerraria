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
    public class FoxFire : Ability
    {
        public FoxFire(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Fox-Fire";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/FoxFire";
        }

        public override string GetAbilityTooltip()
        {
            return "Summon " + TerraLeague.CreateScalingTooltip(TerraLeague.MINIONMAXColor, "MINIONS", (int)Main.LocalPlayer.maxMinions, 100) + " + 2 spectral flames that orbit around you";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 1.5);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 45;
                case DamageType.SUM:
                    return 25;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 10;
        }

        public override int GetBaseManaCost()
        {
            return 30;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " + " + GetScalingTooltip(player, DamageType.SUM) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                Vector2 position = player.MountedCenter;
                Vector2 velocity = Vector2.Zero;
                int projType = ProjectileType<OrbofDeception_FoxFire>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG) + GetAbilityScaledDamage(player, DamageType.SUM);

                int knockback = 1;

                int projCount = 2 + player.maxMinions;

                for (int i = 1; i < projCount + 1; i++)
                {
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, ((MathHelper.TwoPi * i) / projCount));
                }
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 20).WithPitchVariance(0.8f), player.Center);
        }
    }
}
