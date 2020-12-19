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
    public class EbbAndFlow : Ability
    {
        bool checkingForHealing = false;

        public EbbAndFlow(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Ebb and Flow";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/EbbandFlow";
        }

        public override string GetAbilityTooltip()
        {
            return "Send out a rush of ocean water that bounces from enemy to ally and back to another enemy, damaging and healing";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            if (checkingForHealing)
                return (int)(abilityItem.item.damage * 0.75);
            else
                return (int)(abilityItem.item.damage * 1.5);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    if (checkingForHealing)
                        return 15;
                    else
                        return 50;
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
            return 50;
        }

        public override string GetDamageTooltip(Player player)
        {
            checkingForHealing = false;
            string text = GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";

            checkingForHealing = true;
            text += "\n" + TerraLeague.CreateScalingTooltip(DamageType.NONE, GetAbilityBaseDamage(player), 100, true) + " + " + GetScalingTooltip(player, DamageType.MAG, true) + " healing";

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
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 16f);
                int projType = ProjectileType<TideCallerStaff_EbbandFlow>();
                int knockback = 1;

                checkingForHealing = false;
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG);

                checkingForHealing = true;
                int healing = modPlayer.ScaleValueWithHealPower(GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG));
                
                SetAnimation(player, abilityItem.item.useTime, abilityItem.item.useAnimation, position + velocity);

                Projectile proj = Projectile.NewProjectileDirect(position, velocity, projType, damage, knockback, player.whoAmI, healing);

                SetAnimation(player, 30, 30, position + velocity);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(SoundID.Item20, player.Center);
        }
    }
}
