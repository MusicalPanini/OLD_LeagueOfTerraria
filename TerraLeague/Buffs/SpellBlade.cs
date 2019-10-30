using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class SpellBlade : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Spellblade");
            Description.SetDefault("Your next melee attack has increased damage!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().spellBladeBuff = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
