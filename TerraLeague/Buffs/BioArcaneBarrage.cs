using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class BioArcaneBarrage : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bio-Arcane Barrage");
            Description.SetDefault("You ranged attacks are coated in acid");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().bioBarrage = true;
            base.Update(player, ref buffIndex);
        }
    }
}
