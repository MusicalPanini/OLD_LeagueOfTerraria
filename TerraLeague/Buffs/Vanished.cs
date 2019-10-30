using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Vanished : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Vanished");
            Description.SetDefault("Can't hit what you can't see!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().projectileDodge = true;
            player.GetModPlayer<PLAYERGLOBAL>().trueInvis = true;
            player.invis = true;
            player.aggro = -750;
            player.moveSpeed += 0.15f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
