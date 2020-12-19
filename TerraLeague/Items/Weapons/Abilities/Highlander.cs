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
    public class Highlander : Ability
    {
        public Highlander(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Highlander";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Highlander";
        }

        public override string GetAbilityTooltip()
        {
            return "Gain increased movement speed, 40% increased melee speed, and immunity to slows for 7 seconds";
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
                player.AddBuff(BuffType<Buffs.Highlander>(), 60 * 7);
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
