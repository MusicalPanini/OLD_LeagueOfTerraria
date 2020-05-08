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
            for (int j = 0; j < 18; j++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Main.rand.Next((int)player.position.X - 8, (int)player.position.X + 8), player.position.Y + 16), player.width, player.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(0, 255, 255, 0), Main.rand.Next(Main.rand.Next(2, 3)));
                dust.noGravity = true;
            }
        }
    }
}
