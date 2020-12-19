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
    public class HeightenedSenses : Ability
    {
        public HeightenedSenses(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Heightened Senses";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/HeightenedSenses";
        }

        public override string GetAbilityTooltip()
        {
            return "Shows location of enemies, traps and treasure for 4 seconds";
        }

        public override int GetRawCooldown()
        {
            return 90;
        }

        public override int GetBaseManaCost()
        {
            return 100;
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                player.AddBuff(BuffType<Buffs.HeightenedSenses>(), 60 * 4);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 4, -0.5f);
            TerraLeague.DustRing(133, player, Color.White);
        }
    }
}
