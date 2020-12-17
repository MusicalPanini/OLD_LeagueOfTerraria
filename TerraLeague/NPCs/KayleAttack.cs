using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace TerraLeague.NPCs
{
    public class KayleAttack : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire Spellblade");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.EnchantedSword];
        }
        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 48;
            npc.aiStyle = 23;
            npc.damage = 20;
            npc.defense = 0;
            npc.lifeMax = 15;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 0;
            npc.buffImmune[20] = true;
            npc.buffImmune[24] = true;
            npc.buffImmune[39] = true;
            npc.knockBackResist = 0.4f;
            animationType = NPCID.EnchantedSword;
            npc.SpawnedFromStatue = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override bool PreAI()
        {
            if (NPC.CountNPCS(NPCType<TargonBoss>()) <= 0)
            {
                npc.active = false;
            }

            Lighting.AddLight(npc.Center, TargonBoss.KayleColor.ToVector3());

            return base.PreAI();
        }

        public override void AI()
        {
            Lighting.AddLight(new Vector2((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f)), 0.3f, 0.3f, 0.05f);

            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }
            if (npc.ai[0] == 0f)
            {
                //float num298 = 9f;
                //Vector2 vector31 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                //float num299 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector31.X;
                //float num300 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector31.Y;
                //float num301 = (float)Math.Sqrt((double)(num299 * num299 + num300 * num300));
                //float num302 = num301;
                //num301 = num298 / num301;
                //num299 *= num301;
                //num300 *= num301;
                //npc.velocity.X = num299;
                //npc.velocity.Y = num300;
                npc.velocity = TerraLeague.CalcVelocityToPoint(npc.Center, Main.player[npc.target].MountedCenter, 16);
                npc.rotation = npc.velocity.ToRotation() + MathHelper.PiOver4;
                npc.ai[0] = 1f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
            }
            else if (npc.ai[0] == 1f)
            {
                npc.rotation = npc.velocity.ToRotation() + MathHelper.PiOver4;
                if (npc.justHit)
                {
                    npc.ai[0] = 2f;
                    npc.ai[1] = 0f;
                }
                npc.velocity *= 0.99f;
                npc.ai[1] += 1f;

                if (npc.ai[1] >= 200f)
                {
                    npc.netUpdate = true;
                    npc.ai[0] = 2f;
                    npc.ai[1] = 0f;
                    npc.velocity.X = 0f;
                    npc.velocity.Y = 0f;
                }
            }
            else
            {
                if (npc.justHit)
                {
                    npc.ai[0] = 2f;
                    npc.ai[1] = 0f;
                }
                npc.velocity *= 0.99f;
                npc.ai[1] += 1f;
                float num303 = npc.ai[1] / 120f;
                num303 = 0.1f + num303 * 0.4f;
                npc.rotation += num303 * (float)npc.direction;
                if (npc.ai[1] >= 120f)
                {
                    npc.netUpdate = true;
                    npc.ai[0] = 0f;
                    npc.ai[1] = 0f;
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            

            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
            {
                int num437 = 0;
                while ((double)num437 < damage / (double)npc.lifeMax * 50.0)
                {
                    int num438 = Dust.NewDust(npc.position, npc.width, npc.height, 31, 0f, 0f, 0, default(Color), 1.5f);
                    Main.dust[num438].noGravity = true;
                    int num5 = num437;
                    num437 = num5 + 1;
                }
            }
            else
            {
                int num5;
                for (int num439 = 0; num439 < 20; num439 = num5 + 1)
                {
                    int num440 = Dust.NewDust(npc.position, npc.width, npc.height, 31, 0f, 0f, 0, default(Color), 1.5f);
                    Dust dust115 = Main.dust[num440];
                    Dust dust2 = dust115;
                    dust2.velocity *= 2f;
                    Main.dust[num440].noGravity = true;
                    num5 = num439;
                }
                int num441 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 10f), new Vector2((float)Main.rand.Next(-2, 3), (float)Main.rand.Next(-2, 3)), 61, npc.scale);
                Gore gore5 = Main.gore[num441];
                Gore gore2 = gore5;
                gore2.velocity *= 0.5f;
                num441 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 10f), new Vector2((float)Main.rand.Next(-2, 3), (float)Main.rand.Next(-2, 3)), 61, npc.scale);
                gore5 = Main.gore[num441];
                gore2 = gore5;
                gore2.velocity *= 0.5f;
                num441 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 10f), new Vector2((float)Main.rand.Next(-2, 3), (float)Main.rand.Next(-2, 3)), 61, npc.scale);
                gore5 = Main.gore[num441];
                gore2 = gore5;
                gore2.velocity *= 0.5f;
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            base.NPCLoot();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            base.PostDraw(spriteBatch, drawColor);
        }
    }
}
