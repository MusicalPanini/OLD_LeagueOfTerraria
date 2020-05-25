using System;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace TerraLeague.Buffs
{
    public class CenterOfTheUniverse : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Center of the Universe");
            Description.SetDefault("Stars are orbiting you!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 100;

            if (player.ownedProjectileCounts[ProjectileType<StarForgersCore_ForgedStar>()] <= 0)
                player.ClearBuff(Type);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
