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
    public class MicroRockets : Ability
    {
        public MicroRockets(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Hextech Micro-Rockets";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/MicroRockets";
        }

        public override string GetAbilityTooltip()
        {
            return "Fire 5 rockets in a cone towards your cursor";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 0.9 * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().rocketDamageLastStep);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.SUM:
                    return 25;
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
            return 20;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.SUM) + " summon damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                DoEfx(player, type);
                int projType = ProjectileType<HextechWrench_MicroRocket>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.SUM);
                int knockback = 1;

                int numberProjectiles = 5;
                int distance = 24;
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 relPosition = new Vector2(0 - (distance * 2) + (i * distance), 0).RotatedBy(TerraLeague.CalcAngle(player.Center, Main.MouseWorld) + MathHelper.PiOver2);
                    Vector2 position = new Vector2(player.MountedCenter.X + relPosition.X, player.MountedCenter.Y + relPosition.Y);
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 15f);

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                }
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 11), player.Center);
        }
    }
}
