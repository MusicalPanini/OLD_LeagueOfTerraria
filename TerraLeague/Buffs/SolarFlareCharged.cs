using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class SolarFlareCharged : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Solar Flare");
            Description.SetDefault("The power of the sun is ready to be unleashed");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().solariCharge < PLAYERGLOBAL.solariMaxCharge)
                player.ClearBuff(Type);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
