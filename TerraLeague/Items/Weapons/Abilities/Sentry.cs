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
    public class Sentry : Ability
    {
        public Sentry(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Sentry";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Sentry";
        }

        public override string GetAbilityTooltip()
        {
            return "Create a sentry that fires " + TerraLeague.CreateScalingTooltip(TerraLeague.MINIONMAXColor, "MINIONS", (int)Main.LocalPlayer.maxMinions, 100) + " + 5 returning chakrams";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage / 3);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.SUM:
                    return 100;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 14;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return TerraLeague.CreateScalingTooltip(TerraLeague.MINIONMAXColor, "TURRETS", (int)Main.LocalPlayer.maxTurrets, GetAbilityBaseDamage(player) * 100) + " + " + GetScalingTooltip(player, DamageType.SUM) + " summon damage"
                 + "\nUses 10% Crescendum Ammo";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override bool CanCurrentlyBeCast(Player player)
        {
            return player.GetModPlayer<PLAYERGLOBAL>().crescendumAmmo >= 10;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                player.GetModPlayer<PLAYERGLOBAL>().crescendumAmmo -= 10;
                int projType = ProjectileType<Crescendum_Sentry>();
                int damage = (GetAbilityBaseDamage(player) * player.maxTurrets) + GetAbilityScaledDamage(player, DamageType.SUM);
                int knockback = 2;

                player.FindSentryRestingSpot(projType, out int xPos, out int yPos, out int yDis);
                Projectile.NewProjectile((float)xPos, (float)(yPos - yDis) - 3, 0f, 0f, projType, damage, knockback, player.whoAmI, -1);

                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            var sound = Main.PlaySound(new LegacySoundStyle(2, 13), player.Center);
            if (sound != null)
                sound.Pitch = 1f;
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 13, 1f);
        }
    }
}
