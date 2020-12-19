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
    public class WorldEnder : Ability
    {
        public WorldEnder(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "World Ender";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/WorldEnder";
        }

        public override string GetAbilityTooltip()
        {
            return "Gain 10% melee life steal and flight for 10 seconds";
        }

        public override int GetRawCooldown()
        {
            return 35;
        }

        public override int GetBaseManaCost()
        {
            return 50;
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                player.AddBuff(BuffType<Buffs.DarkinBuff>(), 60 * 10);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 4, 51, -1f);
        }
    }
}
