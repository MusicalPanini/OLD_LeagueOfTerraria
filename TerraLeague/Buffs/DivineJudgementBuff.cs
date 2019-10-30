using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class DivineJudgementBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Divine Judgement");
            Description.SetDefault("The worthy survive!" +
                "\nYou are invincible to all damage!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.invincible = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
