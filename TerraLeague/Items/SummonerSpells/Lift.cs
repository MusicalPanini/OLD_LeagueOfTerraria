using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class LiftRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lift Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Lift";
        }

        public override string GetSpellName()
        {
            return "Lift";
        }

        public override int GetRawCooldown()
        {
            return 60;
        }
        public int GetFlightTime()
        {
            if (NPC.downedGolemBoss)
                return 240;
            else if (NPC.downedPlantBoss)
                return 210;
            else if (NPC.downedMechBossAny)
                return 180;
            else if (Main.hardMode)
                return 150;
            else if (NPC.downedBoss2)
                return 120;
            else
                return 90;
        }

        public override string GetTooltip()
        {
            return "Gain wings for 15 seconds with a flight time of " + (GetFlightTime() / 60.0) + " seconds" +
                "\nFlight duration increases throughout the game";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.AddBuff(BuffType<Lift>(), 15 * 60);
            Efx(player);
            PacketHandler.SendLift(-1, player.whoAmI, player.whoAmI);

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 25).WithPitchVariance(-0.3f), player.Center);

            //Vector2 TopPointA = player.Center + new Vector2(-64 * player.direction, -32);
            //Vector2 MidPointA = player.Center + new Vector2(-48 * player.direction, 0);
            //Vector2 BotPointA = player.Center + new Vector2(-32 * player.direction, 12);

            Color color = new Color(199, 201, 164, 0);

            Vector2 TopPointA = player.Center + new Vector2(-64, -32);
            Vector2 MidPointA = player.Center + new Vector2(-48, 0);
            Vector2 BotPointA = player.Center + new Vector2(-32, 12);

            Vector2 TopPointB = player.Center + new Vector2(64, -32);
            Vector2 MidPointB = player.Center + new Vector2(48, 0);
            Vector2 BotPointB = player.Center + new Vector2(32, 12);

            TerraLeague.DustLine(TopPointA, player.Center, 63, 1, 2.5f, color, true, -4, -2);
            TerraLeague.DustLine(MidPointA, player.Center, 63, 1, 2.5f, color, true, -4, 0);
            TerraLeague.DustLine(BotPointA, player.Center, 63, 1, 2.5f, color, true, -4, 1.5f);

            TerraLeague.DustLine(TopPointB, player.Center, 63, 1, 2.5f, color, true, 4, -2);
            TerraLeague.DustLine(MidPointB, player.Center, 63, 1, 2.5f, color, true, 4, 0);
            TerraLeague.DustLine(BotPointB, player.Center, 63, 1, 2.5f, color, true, 4, 1.5f);
        }
    }
}
