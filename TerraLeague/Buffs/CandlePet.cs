using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class CandlePet : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Eternal Flame");
            Description.SetDefault("Emit etherial flames");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.lightPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.LightPet_FlameControl>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.MountedCenter, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<Projectiles.LightPet_FlameControl>(), 0, 0f, player.whoAmI, 0f, 0f);
            }
            player.buffTime[buffIndex] = 10;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
