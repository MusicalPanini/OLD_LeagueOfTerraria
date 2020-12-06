using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class SurgeRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Surge Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Surge";
        }

        public override string GetSpellName()
        {
            return "Surge";
        }

        public override int GetRawCooldown()
        {
            return 120;
        }
        public override string GetTooltip()
        {
            return "Increase all damage by 1.1x and increased knockback for 10 seconds";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.AddBuff(BuffType<Surge>(), 10 * 60);

            Efx(player);
            PacketHandler.SendSurge(-1, player.whoAmI, player.whoAmI);

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 117).WithPitchVariance(0.8f), player.Center);

            int arrow1Height = 32;
            int arrow1Width = 32;
            int arrow1Dis = -16;

            int arrow2Height = 24;
            int arrow2Width = 24;
            int arrow2Dis = 0;

            int arrow3Height = 16;
            int arrow3Width = 16;
            int arrow3Dis = 16;

            Vector2 Arrow1BottomLeft = player.MountedCenter + new Vector2(-arrow1Width, 0 + arrow1Dis);
            Vector2 Arrow1BottomRight = player.MountedCenter + new Vector2(arrow1Width, 0 + arrow1Dis);
            Vector2 Arrow1Top = player.MountedCenter + new Vector2(0, -arrow1Height + arrow1Dis);

            Vector2 Arrow2BottomLeft = player.MountedCenter + new Vector2(-arrow2Width, 0 + arrow2Dis);
            Vector2 Arrow2BottomRight = player.MountedCenter + new Vector2(arrow2Width, 0 + arrow2Dis);
            Vector2 Arrow2Top = player.MountedCenter + new Vector2(0, -arrow2Height + arrow2Dis);

            Vector2 Arrow3BottomLeft = player.MountedCenter + new Vector2(-arrow3Width, 0 + arrow3Dis);
            Vector2 Arrow3BottomRight = player.MountedCenter + new Vector2(arrow3Width, 0 + arrow3Dis);
            Vector2 Arrow3Top = player.MountedCenter + new Vector2(0, -arrow3Height + arrow3Dis);



            TerraLeague.DustLine(Arrow1BottomLeft , Arrow1Top, 182, 1, 2, new Color(255, 0, 0, 0), true, 0, -6);
            TerraLeague.DustLine(Arrow1BottomRight, Arrow1Top, 182, 1, 2, new Color(255, 0, 0, 0), true, 0, -6);

            TerraLeague.DustLine(Arrow2BottomLeft, Arrow2Top, 182, 1, 2, new Color(255, 0, 0, 0), true, 0, -6);
            TerraLeague.DustLine(Arrow2BottomRight, Arrow2Top, 182, 1, 2, new Color(255, 0, 0, 0), true, 0, -6);

            TerraLeague.DustLine(Arrow3BottomLeft, Arrow3Top, 182, 1, 2, new Color(255, 0, 0, 0), true, 0, -6);
            TerraLeague.DustLine(Arrow3BottomRight, Arrow3Top, 182, 1, 2, new Color(255, 0, 0, 0), true, 0, -6);

            for (int j = 0; j < 18; j++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Main.rand.Next((int)player.position.X - 8, (int)player.position.X + 8), player.position.Y + 16), player.width, player.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(255, 0, 0, 0), Main.rand.Next(2, 3));
                dust.noGravity = true;
            }
        }
    }
}
