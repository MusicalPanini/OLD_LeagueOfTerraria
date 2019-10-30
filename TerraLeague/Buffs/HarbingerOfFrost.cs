using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class HarbingerOfFrost : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Harbinger of Frost");
            Description.SetDefault("A chill grasps your heart!" +
                "\nA Frost Storm rages around you");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.frostHarbinger = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
