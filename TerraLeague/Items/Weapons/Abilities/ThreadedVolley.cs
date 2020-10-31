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
    public class ThreadedVolley : Ability
    {
        public ThreadedVolley(AbilityItem item) : base(item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Threaded Volley";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/ThreadedVolley";
        }

        public override string GetAbilityTooltip()
        {
            return "Prepare 5 rock shards and throw them in the original cast direction";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 1.2);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 40;
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
            return 12;
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
                Vector2 position = player.MountedCenter;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(player.Top, 1f);
                int projType = ProjectileType<StoneweaversStaff_ThreadedVolley>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);
                int knockback = 3;

                Main.PlaySound(new LegacySoundStyle(2, 70), player.Center);

                for (int i = 0; i < 5; i++)
                {
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, i);
                }
                SetAnimation(player, abilityItem.item.useTime, abilityItem.item.useAnimation, position + velocity);
                SetCooldowns(player, type);
            }
        }
    }
}
