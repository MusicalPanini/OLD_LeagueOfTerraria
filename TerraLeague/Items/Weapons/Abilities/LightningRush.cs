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
    public class LightningRush : Ability
    {
        public LightningRush(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Lightning Rush";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/LightningRush";
        }

        public override string GetAbilityTooltip()
        {
            return "Surround yourself with lightning, dealing damage on contact." +
                    "\nYou are immune to contact damage for the duration";
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
                    return 80;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 25;
        }

        public override int GetBaseManaCost()
        {
            return 50;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                player.AddBuff(BuffType<Buffs.LightningRush>(), 180);
                Projectile.NewProjectile(player.MountedCenter, Vector2.Zero, ProjectileType<HeartoftheTempest_LightningRush>(), GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG), 0, player.whoAmI);
                player.GetModPlayer<PLAYERGLOBAL>().lightningRush = true;
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 3, 53, 0.25f);
        }
    }
}
