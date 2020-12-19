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
    public class ExcessiveForce : Ability
    {
        public ExcessiveForce(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Excessive Force";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/ExcessiveForce";
        }

        public override string GetAbilityTooltip()
        {
            return "Your next melee attack will deal 300% more damage and splash outward";
        }

        public override int GetRawCooldown()
        {
            return 16;
        }

        public override int GetBaseManaCost()
        {
            return 30;
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
            {
                player.AddBuff(BuffType<Buffs.ExcessiveForce>(), 240);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 42, 24, -0.5f);
            for (int j = 0; j < 10; j++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Fire, 0, -10);
                dust.velocity.X *= 0;
                dust.velocity.Y -= 4;
                dust.noGravity = true;
                dust.scale = 2;
            }
        }
    }
}
