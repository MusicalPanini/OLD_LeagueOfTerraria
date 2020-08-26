using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Overdrive : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Overdrive");
            Description.SetDefault("10% increased damage" +
                "\n20% increased move speed");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage += 0.1f;
            player.rangedDamage += 0.1f;
            player.magicDamage += 0.1f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.1f;
            player.moveSpeed += 0.2f;
        }
    }
}
