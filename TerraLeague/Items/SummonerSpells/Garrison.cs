using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class GarrisonRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Garrison Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Garrison";
        }

        public override string GetSpellName()
        {
            return "Garrison";
        }

        public override int GetRawCooldown()
        {
            return 150;
        }

        static public int GetScalingStat()
        {
            if (NPC.downedGolemBoss)
                return 70;
            else if (NPC.downedPlantBoss)
                return 50;
            else if (NPC.downedMechBossAny)
                return 35;
            else if (Main.hardMode)
                return 20;
            else if (NPC.downedBoss2)
                return 10;
            else
                return 5;
        }

        public override string GetTooltip()
        {
            return "Gain 50 life and " + GetScalingStat() +  " armor, resist, and defence for 10 seconds" +
                "\nThe defensive stats increases throughout the game";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.AddBuff(BuffType<Garrison>(), 10 * 60);
            Efx(player);
            PacketHandler.SendGarrison(-1, player.whoAmI, player.whoAmI);

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 117).WithPitchVariance(0.8f), player.Center);
            for (int j = 0; j < 18; j++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Main.rand.Next((int)player.position.X - 8, (int)player.position.X + 8), player.position.Y + 16), player.width, player.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(131, 234, 46, 0), Main.rand.Next(2, 3));
                dust.noGravity = true;
            }
        }
    }
}
