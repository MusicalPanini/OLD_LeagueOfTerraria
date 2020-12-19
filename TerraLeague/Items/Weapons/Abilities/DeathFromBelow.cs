using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class DeathFromBelow : Ability
    {
        public DeathFromBelow(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Death From Below";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/DeathFromBelow";
        }

        public override string GetAbilityTooltip()
        {
            return "Summon 2 etherial blades at the targeted location that strike upward in a cross-section after a delay." +
                    "\nKills will allow you to cast again for a short time.";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 1.5);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MEL:
                    return 125;
                default:
                    return 0;
            }
        }

        public override int GetBaseManaCost()
        {
            if (CurrentlyHasSpecialCast(Main.LocalPlayer))
                return 0;
            else
                return 50;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MEL) + " melee damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override int GetRawCooldown()
        {
            return 60;
        }

        public override bool CurrentlyHasSpecialCast(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().deathFromBelowRefresh)
                return true;
            else
                return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if ((CheckIfNotOnCooldown(player, type) || CurrentlyHasSpecialCast(player)) && player.CheckMana(GetScaledManaCost(), true))
            {
                if (CurrentlyHasSpecialCast(player))
                    player.ClearBuff(BuffType<DeathFromBelowRefresh>());
                else
                    SetCooldowns(player, type);

                Vector2 position = Main.MouseWorld;
                Vector2 velocity = Vector2.Zero;
                int projType = ProjectileType<BoneSkewer_DeathFromBelow>();
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MEL);

                int knockback = 0;

                Projectile.NewProjectile(position + new Vector2(95, 90), velocity, projType, damage, knockback, player.whoAmI, 1);
                Projectile.NewProjectile(position + new Vector2(-95, 90), velocity, projType, damage, knockback, player.whoAmI, -1);

                DoEfx(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 117, -0.5f);
        }
    }
}
