using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class ForDemacia : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("For Demacia!");
            Description.SetDefault("Increased attack speed by 10%");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().forDemacia = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
