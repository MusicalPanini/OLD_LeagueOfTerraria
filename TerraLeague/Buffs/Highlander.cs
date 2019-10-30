using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class Highlander : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Highlander");
            Description.SetDefault("Increased movement and melee speed, and immunity to slows");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeSpeed *= 1.4f;
            player.GetModPlayer<PLAYERGLOBAL>().highlander = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.OgreSpit] = true;
        }
    }
}
