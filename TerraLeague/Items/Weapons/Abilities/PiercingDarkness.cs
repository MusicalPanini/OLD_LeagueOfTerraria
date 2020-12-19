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
    public class PiercingDarkness : Ability
    {
        bool checkingForHealing = false;

        public PiercingDarkness(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Piercing Darkness";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/PiercingDarkness";
        }

        public override string GetAbilityTooltip()
        {
            return "Fire a spectral laser damaging enemies and healing allies";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            if (checkingForHealing)
                return (int)(abilityItem.item.damage / 3);
            else
                return (int)(abilityItem.item.damage * 2);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.RNG:
                    if (checkingForHealing)
                        return 50;
                    else
                        return 125;
                case DamageType.MAG:
                    return 35;
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
            return 80;
        }

        public override string GetDamageTooltip(Player player)
        {
            checkingForHealing = false;
            string text = GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.RNG) + " ranged damage";

            checkingForHealing = true;
            text += "\n" + TerraLeague.CreateScalingTooltip(DamageType.NONE, GetAbilityBaseDamage(player), 100, true) + " + " + GetScalingTooltip(player, DamageType.RNG, true) + " + " + GetScalingTooltip(player, DamageType.MAG, true) + " healing";

            return text;
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
                Vector2 position = player.MountedCenter;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 8f);
                int projType = ProjectileType<LightCannon_PiercingDarkness>();
                int knockback = 0;

                checkingForHealing = false;
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG);

                checkingForHealing = true;
                int healing = modPlayer.ScaleValueWithHealPower(GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.RNG) + GetAbilityScaledDamage(player, DamageType.MAG));

                Projectile proj = Projectile.NewProjectileDirect(position, velocity, projType, damage, knockback, player.whoAmI, healing);

                SetAnimation(player, 30, 30, position + velocity);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 13, 1f);
        }
    }
}
