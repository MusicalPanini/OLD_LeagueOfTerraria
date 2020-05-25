using Terraria;
using Terraria.ModLoader;
using TerraLeague.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class EvolutionTurrets : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Evolution Turret");
            Description.SetDefault("A machine that fights for you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {

            if (player.ownedProjectileCounts[ProjectileType<HextechWrench_EvolutionTurret>()] <= 0)
                player.ClearBuff(Type);
            else
                player.buffTime[buffIndex] = 100;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
