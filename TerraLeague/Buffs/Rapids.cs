using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Rapids : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Rapids");
            Description.SetDefault("Movement speed increased by 15%" +
                "\nMagic and minion damage increased by 10%");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.magicDamage += 0.1f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.1;
            player.moveSpeed *= 1.15f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
