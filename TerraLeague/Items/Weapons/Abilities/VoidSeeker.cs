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
    public class VoidSeeker : Ability
    {
        public VoidSeeker(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Void Seeker";
        }

        public override string GetIconTexturePath()
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.magicDamageLastStep < modPlayer.rangedDamageLastStep)
                return "AbilityImages/VoidSeeker";
            else
                return "AbilityImages/VoidSeekerM";
        }

        public override string GetAbilityTooltip()
        {
            return "Launch a void blast that applies 2 stacks of 'Caustic Wounds' to the hit enemy." +
                    "\nAt [c/" + TerraLeague.MAGColor + ":50 MAG], Void Seeker will deal magic damage instead and apply 3 stacks.";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 3);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    return 150;
                case DamageType.MAG:
                    return 60;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 18;
        }

        public override int GetBaseManaCost()
        {
            return 30;
        }

        public override string GetDamageTooltip(Player player)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.MAG >= 50)
                return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.RNG) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";
            else
                return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.RNG) + " + " + GetScalingTooltip(player, DamageType.MAG) + " ranged damage";
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
                int projType = ProjectileType<CrystalineVoidEnergy_VoidSeeker>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG) + GetAbilityScaledDamage(player, DamageType.MAG);
                int knockback = 1;

                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                int poweredUp = 0;
                if (modPlayer.MAG >= 50)
                    poweredUp = 1;

                Projectile proj = Projectile.NewProjectileDirect(player.Center, velocity, projType, damage, knockback, player.whoAmI, poweredUp);


                SetAnimation(player, 20, 20, position + velocity);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 91, -1f);
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 43, -0.5f);
        }
    }
}
