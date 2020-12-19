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
    public class Courage : Ability
    {
        public Courage(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Courage";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Courage";
        }

        public override string GetAbilityTooltip()
        {
            return "Gain 6 armor and resist for 5 seconds";
        }

        public override int GetRawCooldown()
        {
            return 10;
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
                player.AddBuff(BuffType<Buffs.Courage>(), 60 * 5);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 37, -1f);

            int radius = 100;
            for (int i = 0; i < radius / 5; i++)
            {
                Vector2 pos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (radius / 5f)))) + player.MountedCenter;

                Dust dustR = Dust.NewDustPerfect(pos, DustID.Iron, Vector2.Zero, 0, default(Color), 1.5f);
                dustR.noGravity = true;
                dustR.velocity = (dustR.position - player.MountedCenter) * -0.1f + player.velocity;
            }
        }
    }
}
