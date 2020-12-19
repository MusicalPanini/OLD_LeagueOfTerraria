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
    public class BioArcaneBarrage : Ability
    {
        public BioArcaneBarrage(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Bio-Arcane Barrage";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Bio-ArcaneBarrage";
        }

        public override string GetAbilityTooltip()
        {
            return "Your ranged attacks deal On Hit damage based on targets max life for 3 seconds.";
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 5;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 20;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return "4% + " + TerraLeague.CreateScalingTooltip(DamageType.MAG, player.GetModPlayer<PLAYERGLOBAL>().MAG, GetAbilityScalingAmount(player, DamageType.MAG), false, "%") + " of targets max life On Hit" +
                    "\nMax: 20 + " + TerraLeague.CreateScalingTooltip(DamageType.MAG, player.GetModPlayer<PLAYERGLOBAL>().MAG, 50) + " damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                player.AddBuff(BuffType<Buffs.BioArcaneBarrage>(), 60 * 3);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 4, 13, -1f);
        }

        public static int GetMaxOnHit(PLAYERGLOBAL player)
        {
            return (int)(20 + (player.MAG * 0.05 * 10));
        }
    }
}
