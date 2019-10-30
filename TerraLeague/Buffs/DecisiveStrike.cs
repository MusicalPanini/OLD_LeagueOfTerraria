using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class DecisiveStrike : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Decisive Strike");
            Description.SetDefault("Your next melee attack will deal additional damage and apply 'Slowed'");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.meleeModifer *= 1.5;
            player.moveSpeed += 0.15f;
            player.buffImmune[BuffID.Slow] = true;
            modPlayer.decisiveStrike = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
