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
    public class HexsplosiveMineField : Ability
    {
        public HexsplosiveMineField(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Hexsplosive Mine Field";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/MineField";
        }

        public override string GetAbilityTooltip()
        {
            return "Toss a cluster of small bombs that creates a mine field";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 0.8);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 20;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 12;
        }

        public override int GetBaseManaCost()
        {
            return 20;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage per mine";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                Vector2 position = player.MountedCenter;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 10);
                int projType = ProjectileType<Hexplosives_HexplosiveMineField>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);
                int knockback = 7;

                SetAnimation(player, abilityItem.item.useTime, abilityItem.item.useAnimation, position + velocity);
                DoEfx(player, type);
                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 19, SoundType.Sound), player.Center);
        }
    }
}
