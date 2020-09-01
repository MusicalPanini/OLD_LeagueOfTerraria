using Terraria;
using Terraria.ModLoader;
using TerraLeague.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class TheBall : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("The Ball");
            Description.SetDefault("The Ball is angry");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<ClockworkStaff_TheBall>()] <= 0)
                player.ClearBuff(Type);
            else
                player.buffTime[buffIndex] = 100;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}

