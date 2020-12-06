using Microsoft.Xna.Framework;
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
        public static int buffDuration = 30;

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
            return 180;
        }
        public override string GetTooltip()
        {
            return "Instantly revive at full life and mana with a speed and defence boost for " + buffDuration + " seconds";
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

            Color color = new Color(68, 253, 180, 0);

            Vector2 TopPointA = player.Center + new Vector2(-64, -32);
            Vector2 MidPointA = player.Center + new Vector2(-48, 0);
            Vector2 BotPointA = player.Center + new Vector2(-32, 12);

            Vector2 TopPointB = player.Center + new Vector2(64, -32);
            Vector2 MidPointB = player.Center + new Vector2(48, 0);
            Vector2 BotPointB = player.Center + new Vector2(32, 12);

            TerraLeague.DustLine(TopPointA, player.Center, 229, 1, 2.5f, color, true, -4, -2);
            TerraLeague.DustLine(MidPointA, player.Center, 229, 1, 2.5f, color, true, -4, 0);
            TerraLeague.DustLine(BotPointA, player.Center, 229, 1, 2.5f, color, true, -4, 1.5f);

            TerraLeague.DustLine(TopPointB, player.Center, 229, 1, 2.5f, color, true, 4, -2);
            TerraLeague.DustLine(MidPointB, player.Center, 229, 1, 2.5f, color, true, 4, 0);
            TerraLeague.DustLine(BotPointB, player.Center, 229, 1, 2.5f, color, true, 4, 1.5f);

            TerraLeague.DustElipce(32, 10, 0, new Vector2(player.MountedCenter.X, player.MountedCenter.Y - 48), 229, color, 2);

            for (int j = 0; j < 18; j++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Main.rand.Next((int)player.position.X - 8, (int)player.position.X + 8), player.position.Y + 16), player.width, player.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(68, 253, 180, 0), Main.rand.Next(2, 3));
                dust.noGravity = true;
            }
        }

    }
}
