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
    public class EssenceFlux : Ability
    {
        public EssenceFlux(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Essence Flux";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/EssenceFlux";
        }

        public override string GetAbilityTooltip()
        {
            return "Fire a ring of magic that applies 'Essence Flux'" +
                "\nHitting a Flux'd enemy with an attack from Nezuk's Gauntlet detonates the mark dealing damage and restoring 100 mana";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(125);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 60;
                case DamageType.MAG:
                    return 90;
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
            return 30;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.RNG) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";
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
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12);
                int projType = ProjectileType<NezuksGauntlet_EssenceFlux>();
                int damage = (int)(1);
                int knockback = 0;

                SetAnimation(player, abilityItem.item.useTime, abilityItem.item.useAnimation, position + velocity);
                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public static int GetFluxDamage(PLAYERGLOBAL player)
        {
            return (int)(125 + (player.RNG * 0.6) + (player.MAG * 0.9));
        }
    }
}
