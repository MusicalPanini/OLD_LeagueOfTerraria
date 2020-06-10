using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class LightningRush : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Lightning Rush");
            Description.SetDefault("Energy flows through you");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().contactDodge = true;
            player.GetModPlayer<PLAYERGLOBAL>().lightningRush = true;
            player.GetModPlayer<PLAYERGLOBAL>().trueInvis = true;
            player.silence = true;
            player.noItems = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
