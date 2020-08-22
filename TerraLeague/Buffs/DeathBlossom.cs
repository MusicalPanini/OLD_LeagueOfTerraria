using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons;
using TerraLeague.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class DeathBlossom : ModBuff
    {
        public bool initial = true;
        int damage = 0;

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Death Lotus");
            Description.SetDefault("Spin");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            Main.debuff[Type] = true;
            if (damage == 0)
            {
                damage = player.buffTime[buffIndex] + 1;
                player.buffTime[buffIndex] = 149;
            }

            player.noKnockback = true;
            player.jump = 0;
            player.wingTime = 0;
            player.noItems = true;
            player.silence = true;
            player.ChangeDir(player.oldDirection);
            player.GetModPlayer<PLAYERGLOBAL>().deathLotus = true;

            if (player.buffTime[buffIndex] % 3 == 1 && Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                player.ChangeDir(player.direction == 1 ? -1 : 1);

                for (int i = 0; i < 3; i++)
                {
                    Projectile proj = Projectile.NewProjectileDirect(player.MountedCenter, new Microsoft.Xna.Framework.Vector2(0, 10).RotatedBy(-MathHelper.TwoPi * (player.buffTime[buffIndex] / 75f) + ((MathHelper.TwoPi * i)/3)), ProjectileType<DarksteelDagger_Dagger>(), damage, 2, player.whoAmI, 1, 0);
                    proj = Projectile.NewProjectileDirect(player.MountedCenter, new Microsoft.Xna.Framework.Vector2(0, 10).RotatedBy(MathHelper.TwoPi * (player.buffTime[buffIndex] / 75f) + ((MathHelper.TwoPi * i)/3)), ProjectileType<DarksteelDagger_Dagger>(), damage, 2, player.whoAmI, 1);
                }
            }

            if (player.buffTime[buffIndex] == 0)
                damage = 0;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
