using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Immolate : Passive
    {
        int effectRadius;

        public Immolate(int EffectRadius)
        {
            effectRadius = EffectRadius;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: IMMOLATE -] [c/99e6ff:Set near by enemies on fire]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

       

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            DoThing(player, modItem);
            base.PostPlayerUpdate(player, modItem);
        }

        public void DoThing(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.shieldFrame == 0)
            {
                for (int i = 0; i < 18; i++)
                {
                    Vector2 vel = new Vector2(13, 0).RotatedBy(MathHelper.ToRadians(20 * i));

                    Dust dust = Dust.NewDustPerfect(player.Center, 6, vel, 0, default(Color), 3);
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
            for (int i = 0; i < 1; i++)
            {
                Vector2 pos = new Vector2(player.position.X, player.position.Y + (player.height * 0.9f));
                Dust dustIndex = Dust.NewDustDirect(pos, player.width, player.height / 10, 6, 12f, -1f, 100, default(Color), 1.25f);
                dustIndex.noGravity = true;
                dustIndex.velocity.Y *= 0.4f;
                dustIndex.velocity.X *= 0.6f;
                dustIndex.velocity.X += player.velocity.X;
                dustIndex.noLight = true;
                Dust dustIndex2 = Dust.NewDustDirect(pos, player.width, player.height / 10, 6, -12f, -1f, 100, default(Color), 1.5f);
                dustIndex2.noGravity = true;
                dustIndex2.noLight = true;
                dustIndex2.velocity.Y *= 0.4f;
                dustIndex2.velocity.X *= 0.6f;
                dustIndex2.velocity.X += player.velocity.X;
            }

            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC DamTarget = Main.npc[i];

                float damtoX = DamTarget.position.X + (float)DamTarget.width * 0.5f - player.Center.X;
                float damtoY = DamTarget.position.Y + (float)DamTarget.height * 0.5f - player.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(damtoX * damtoX + damtoY * damtoY));

                if (distance < effectRadius && !DamTarget.townNPC && DamTarget.lifeMax > 5 && !DamTarget.immortal)
                {
                    Main.npc[i].AddBuff(BuffType<Buffs.Sunfire>(), 5);
                }
            }
        }
    }
}
