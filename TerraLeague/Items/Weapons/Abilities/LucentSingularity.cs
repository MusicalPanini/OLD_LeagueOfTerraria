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
    public class LucentSingularity : Ability
    {
        public LucentSingularity(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Lucent Singularity";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/LucentSingularity";
        }

        public override string GetAbilityTooltip()
        {
            return "Cast a ball of light that creates a slowing zone and detonates after a delay, 'Illuminating' hit enemies." +
                    "\nRecast to detonate early";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 2);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 60;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 13;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return CurrentlyHasSpecialCast(Main.LocalPlayer);
        }

        public override bool CanBeCastWhileCCd()
        {
            return CurrentlyHasSpecialCast(Main.LocalPlayer);
        }

        public override bool CurrentlyHasSpecialCast(Player player)
        {
            int pos = GetPositionOfAbilityInArray(abilityItem);
            if (pos != -1)
                return (player.ownedProjectileCounts[ProjectileType<RadiantStaff_LucentSingularity>()] > 0 && player.GetModPlayer<PLAYERGLOBAL>().AbilityCooldowns[pos] <= GetCooldown() * 60 - 10);
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CurrentlyHasSpecialCast(Main.LocalPlayer))
            {
                Projectile proj = Main.projectile.Where(x => x.type == ProjectileType<RadiantStaff_LucentSingularity>() && x.owner == player.whoAmI).FirstOrDefault();
                if (proj.width != 8)
                    proj.timeLeft = 1;
            }
            else if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                Vector2 position = player.MountedCenter;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12);
                int projType = ProjectileType<RadiantStaff_LucentSingularity>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);
                int knockback = 0;

                SetAnimation(player, abilityItem.item.useTime, abilityItem.item.useAnimation, position + velocity);
                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 8, Terraria.Audio.SoundType.Sound), player.Center);
        }
    }
}
