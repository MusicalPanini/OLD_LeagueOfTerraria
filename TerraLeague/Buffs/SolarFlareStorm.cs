using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;

namespace TerraLeague.Buffs
{
    public class SolarFlareStorm : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Solar Flare Storm");
            Description.SetDefault("The power of the sun is ready to be unleashed");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().solarStorm = true;
            
            if (player.buffTime[buffIndex] % 30 == 29)
            {
                float x = Main.rand.NextFloat(player.MountedCenter.X - Main.screenWidth / 2, player.MountedCenter.X + Main.screenWidth / 2);
                float y = Main.rand.NextFloat(Main.screenPosition.Y, Main.screenPosition.Y + Main.screenHeight/4);

                Projectile.NewProjectileDirect(new Vector2(x, y), Vector2.Zero, ModContent.ProjectileType<SolariSet_SolarSigil>(), (int)(100 * player.GetModPlayer<PLAYERGLOBAL>().magicDamageLastStep), 2, player.whoAmI);
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
