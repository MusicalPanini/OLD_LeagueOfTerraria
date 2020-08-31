using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class Immolate : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Immolate");
            Description.SetDefault("Your burning nearby enemies");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().immolate = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            
        }
    }
}
