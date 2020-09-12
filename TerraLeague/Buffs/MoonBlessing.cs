using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class MoonBlessing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blessing of The Silver Sister");
            Description.SetDefault("10% increased critical strike chance");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeCrit += 10;
            player.rangedCrit += 10;
            player.magicCrit += 10;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.1f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
