using TerraLeague.Items.SummonerSpells;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class EvolvedWings : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Evolved Wings");
            Description.SetDefault("You've sprouted wings");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.wingTimeMax = 45;
            player.wings = 18;
            player.wingsLogic = 29;

            player.meleeDamage += 0.5f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {

        }
    }
}
