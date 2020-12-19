using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class ToxicShot : Ability
    {
        public ToxicShot(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Toxic Shot";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/ToxicShot";
        }

        public override string GetAbilityTooltip()
        {
            return "Your ranged attacks apply 'Venom' and deal additional On Hit damage for 5 seconds";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return 50;
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.SUM:
                    return 30;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 15;
        }

        public override int GetBaseManaCost()
        {
            return 50;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.SUM) + " ranged On Hit damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                player.AddBuff(BuffType<Buffs.ToxicShot>(), 60 * 5);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 102, -1f);
        }

        public static int GetMaxOnHit(PLAYERGLOBAL player)
        {
            return (int)(50 + (player.SUM * 0.3));
        }
    }
}
