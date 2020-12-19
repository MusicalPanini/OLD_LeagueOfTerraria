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
    public class EmperorsDivide : Ability
    {
        public EmperorsDivide(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Emperors Divide";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/EmperorsDivide";
        }

        public override string GetAbilityTooltip()
        {
            return "Call forward 2 walls of sand shields that knock enemies away";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 2);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.SUM:
                    return 80;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 45;
        }

        public override int GetBaseManaCost()
        {
            return 60;
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
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                DoEfx(player, type);
                Vector2 position = player.MountedCenter;
                Vector2 velocity = new Vector2(16, 0);
                int projType = ProjectileType<EmperoroftheSands_EmperorsDivide>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.SUM);
                int knockback = 30;

                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(position, -velocity, projType, damage, knockback, player.whoAmI);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 82, -0.5f);
        }
    }
}
