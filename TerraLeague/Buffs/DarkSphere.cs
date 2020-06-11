using Terraria;
using Terraria.ModLoader;
using TerraLeague.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class DarkSphere : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Dark Sphere");
            Description.SetDefault("You are manipulating spheres of negative emotion!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<DarkSovereignsStaff_DarkSphere>()] <= 0)
                player.ClearBuff(Type);
            else
                player.buffTime[buffIndex] = 100;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
