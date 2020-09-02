using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class ExcessiveForce : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Excessive Force");
            Description.SetDefault("Your next melee attack will deal additional damage and splash outward from the enemy");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.excessiveForce = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
