﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.SummonerSpells
{
    public class ReviveRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Revive Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Revive";
        }

        public override string GetSpellName()
        {
            return "Revive";
        }

        public override int GetRawCooldown()
        {
            return 300;
        }
        public override string GetTooltip()
        {
            return "Instantly revive at half life where you died with a speed and defence boost";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            if (player.dead)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                modPlayer.reviving = true;
                player.respawnTimer = 0;
                SetCooldowns(player, spellSlot);
            }
        }

        static public void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29), player.Center);
            for (int j = 0; j < 18; j++)
            {
                int num2 = Dust.NewDust(new Vector2(Main.rand.Next((int)player.position.X - 8, (int)player.position.X + 8), player.position.Y + 16), player.width, player.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(68, 253, 180, 0), Main.rand.Next(2, 3));
                Main.dust[num2].noGravity = true;
            }
        }

    }
}
