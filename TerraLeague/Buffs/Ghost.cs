using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Ghost : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ghost");
            Description.SetDefault("Increased movement speed and Immunity to knockback");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().ghosted = true;
            player.noKnockback = true;
        }
    }
}
