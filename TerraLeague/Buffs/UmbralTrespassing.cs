using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class UmbralTrespassing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Umbral Trespassing");
            Description.SetDefault("Hiding in their soul");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().umbralTrespassing = true;
            player.GetModPlayer<PLAYERGLOBAL>().projectileDodge = true;
            player.GetModPlayer<PLAYERGLOBAL>().trueInvis = true;
            player.buffImmune[BuffType<GrievousWounds>()] = true;
        }
    }
}
