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
    public class EvolvedWings : Ability
    {
        public EvolvedWings(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Evolved Wings";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/EvolvedWings";
        }

        public override string GetAbilityTooltip()
        {
            return "Gain wings and 50% increased melee damage for 4 seconds";
        }

        public override int GetRawCooldown()
        {
            return 22;
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
                player.AddBuff(BuffType<Buffs.EvolvedWings>(), 60 * 4);
                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(SoundID.NPCDeath1, player.MountedCenter);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 97, -3 * player.direction, -2);
                dust.scale = 2;
            }
        }
    }
}
