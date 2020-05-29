using Terraria;
using Terraria.ModLoader;
using TerraLeague.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class SandSolder : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sand Solder");
            Description.SetDefault("A solder made of sand will fight for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<EmperoroftheSands_SandSolder>()] <= 0)
                player.ClearBuff(Type);
            else
                player.buffTime[buffIndex] = 100;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
