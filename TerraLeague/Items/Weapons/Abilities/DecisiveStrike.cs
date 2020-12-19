using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class DecisiveStrike : Ability
    {
        public DecisiveStrike(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Decisive Strike";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/DecisiveStrike";
        }

        public override string GetAbilityTooltip()
        {
            return "You gain 15% movement speed and your next melee attack will deal 150% damage and apply 'Slowed'";
        }

        public override int GetRawCooldown()
        {
            return 9;
        }

        public override int GetBaseManaCost()
        {
            return 0;
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                player.AddBuff(BuffType<Buffs.DecisiveStrike>(), 60 * 5);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 9, -0.5f);

            for (int j = 0; j < 10; j++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 64, 0, -10);
                dust.velocity.X *= 0;
                dust.velocity.Y -= 4;
                dust.noGravity = true;
                dust.scale = 2;
            }
        }
    }
}
