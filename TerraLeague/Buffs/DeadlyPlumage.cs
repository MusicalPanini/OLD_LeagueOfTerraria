using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class DeadlyPlumage : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Deadly Plumage");
            Description.SetDefault("You ranged attack speed and movement speed have been increased!");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().deadlyPlumage = true;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.25;
            player.moveSpeed += 0.25f;
        }
    }
}
