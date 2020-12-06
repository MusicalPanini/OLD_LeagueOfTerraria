using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons;
using TerraLeague.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class RiteoftheArcane : ModBuff
    {
        public bool initial = true;
        int damage = 0;

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Rite of the Arcane");
            Description.SetDefault("Magic Artillery!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (damage == 0)
            {
                damage = player.buffTime[buffIndex] + 1;
                player.buffTime[buffIndex] = 179;
            }

            player.noKnockback = true;
            player.jump = 0;
            player.wingTime = 0;
            player.noItems = true;
            player.silence = true;
            player.ChangeDir(player.oldDirection);
            player.GetModPlayer<PLAYERGLOBAL>().rightoftheArcaneChannel = true;

            if (player.buffTime[buffIndex] % 5 == 0 && Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                Vector2 position = new Vector2(Main.rand.NextFloat(-400, 400) + player.MountedCenter.X, player.MountedCenter.Y - 700);
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-6, 6), 16);
                Projectile proj = Projectile.NewProjectileDirect(position, velocity, ProjectileType<ArcaneEnergy_Artillery>(), damage, 2, player.whoAmI);

                //Projectile proj = Projectile.NewProjectileDirect(player.MountedCenter, new Vector2(Main.rand.NextFloat(-8 + 7 * player.buffTime[buffIndex] / 180f, 8 - 7 * player.buffTime[buffIndex] / 180f), Main.rand.NextFloat(-18, -16)), ProjectileType<ArcaneEnergy_Artillery>(), damage, 2, player.whoAmI);
            }

            if (player.buffTime[buffIndex] == 0)
                damage = 0;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
