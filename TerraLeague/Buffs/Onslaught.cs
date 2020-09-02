using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Microsoft.Xna.Framework;

namespace TerraLeague.Buffs
{
    public class Onslaught : ModBuff
    {
        int damage = 0;

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Onslaught");
            Description.SetDefault("Rapidly attack near by enemies");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().onslaught = true;

            Main.debuff[Type] = true;
            if (damage == 0)
            {
                damage = player.buffTime[buffIndex] + 1;
                player.buffTime[buffIndex] = 239;
            }

            if (player.buffTime[buffIndex] % 10 == 0 && Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                var npcs = TerraLeague.GetAllNPCsInRange(player.MountedCenter, 300, true);

                for (int i = 0; i < npcs.Count; i++)
                {
                    NPC npc = Main.npc[npcs[i]];

                    float X = Main.rand.NextFloat(npc.Left.X - (npc.width / 2), npc.Right.X + (npc.width / 2));
                    float Y = Main.rand.NextFloat(npc.Top.Y - (npc.height / 2), npc.Bottom.Y + (npc.height / 2));
                    Vector2 pos = new Vector2(X, Y);
                    Vector2 vel = (npc.Center - pos).SafeNormalize(Vector2.One);

                    Projectile.NewProjectileDirect(pos, vel, ModContent.ProjectileType<Projectiles.Severum_Onslaught>(), damage, 0, player.whoAmI, npc.whoAmI);
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
