using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using TerraLeague.Items.SummonerSpells;

namespace TerraLeague.Buffs
{
    public class Garrison : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Garrison");
            Description.SetDefault("Magic has made you resilient");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().garrison = true;
            player.GetModPlayer<PLAYERGLOBAL>().armor += GarrisonRune.GetScalingStat();
            player.GetModPlayer<PLAYERGLOBAL>().resist += GarrisonRune.GetScalingStat();
            player.statDefense += GarrisonRune.GetScalingStat();
            player.noKnockback = true;
            player.longInvince = true;
            player.statLifeMax2 += 50;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}