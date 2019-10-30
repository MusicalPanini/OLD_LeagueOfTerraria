using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class FleetFoot : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Fleet Foot");
            Description.SetDefault("Increased movement speed!");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 1f;
        }
    }
}
