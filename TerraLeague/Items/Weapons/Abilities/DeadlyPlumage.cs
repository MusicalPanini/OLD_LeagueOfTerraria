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
    public class DeadlyPlumage : Ability
    {
        public DeadlyPlumage(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Deadly Plumage";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/DeadlyPlumage";
        }

        public override string GetAbilityTooltip()
        {
            return "Gain 25% movement speed and ranged attack speed for 5 seconds";
        }

        public override int GetRawCooldown()
        {
            return 18;
        }

        public override int GetBaseManaCost()
        {
            return 60;
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                player.AddBuff(BuffType<Buffs.DeadlyPlumage>(), 60 * 5);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 117).WithPitchVariance(0.8f), player.Center);
        }
    }
}
