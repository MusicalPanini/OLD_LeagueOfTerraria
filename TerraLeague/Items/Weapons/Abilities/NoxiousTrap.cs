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
    public class NoxiousTrap : Ability
    {
        public NoxiousTrap(AbilityItem item) : base(item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Noxious Trap";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/NoxiousTrap";
        }

        public override string GetAbilityTooltip()
        {
            return "Toss 3 mushroom traps that will rupture and release clouds of venom when an enemy is near";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 1.5);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.SUM:
                    return 12;
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
            return 30;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.SUM) + " summon damage";
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
                int projType = ProjectileType<ToxicBlowgun_NoxiousTrap>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.SUM);
                int knockback = 0;

                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(position, velocity * 1.5f, projType, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(position, velocity * 0.5f, projType, damage, knockback, player.whoAmI);

                SetAnimation(player, 20, 20, position + velocity);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(SoundID.Item1, player.Center);
        }
    }
}
