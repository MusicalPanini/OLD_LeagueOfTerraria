using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Faded : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Faded");
            Description.SetDefault("You can't use the Faded Mirror");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = true;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
