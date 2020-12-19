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
    public class GravityField : Ability
    {
        public GravityField(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Gravity Field";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/GravityField";
        }

        public override string GetAbilityTooltip()
        {
            return "Deploy a gravitational imprisonment device, that stuns caught enemies";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 1.4);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.SUM:
                    return 70;
                case DamageType.MAG:
                    return 70;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 24;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " + " + GetScalingTooltip(player, DamageType.SUM) + " ranged damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                int projType = ProjectileType<HexCoreStaff_GravityField>();
                player.FindSentryRestingSpot(projType, out int xPos, out int yPos, out int yDis);
                Vector2 position = new Vector2(xPos, (yPos - yDis) + 14);
                Vector2 velocity = Vector2.Zero;
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.SUM) + GetAbilityScaledDamage(player, DamageType.MAG);
                int knockback = 0;

                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                SetAnimation(player, abilityItem.item.useTime, abilityItem.item.useAnimation, position + velocity);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 82), player.Center);
        }
    }
}
