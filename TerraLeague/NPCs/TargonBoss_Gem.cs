using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Dusts;
using Terraria.Audio;
using System.Linq;

namespace TerraLeague.NPCs
{
    public class TargonBoss_Gem : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Protector's Gem");
        }
        public override void SetDefaults()
        {
            npc.width = 20;
            npc.height = 42;
            npc.defense = 0;
            npc.lifeMax = 10;

            npc.HitSound = new LegacySoundStyle(3, 5);
            npc.DeathSound = new LegacySoundStyle(2, 27);
            npc.value = 0;
            npc.buffImmune[BuffType<TideCallerBubbled>()] = true;
            npc.buffImmune[BuffType<Stunned>()] = true;
            npc.knockBackResist = 0f;
            npc.SpawnedFromStatue = true;
            npc.noGravity = true;
            npc.alpha = 255;
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override bool PreAI()
        {
            if ((int)npc.ai[0] == 1)
            {
                npc.width = 40;
                npc.height = 84;
                npc.defense = 20;
                npc.lifeMax = 50 * (Main.expertMode ? 2 : 1);
                npc.life = npc.lifeMax;
                npc.netUpdate = true;
                npc.ai[0] = 2;
            }

            if (NPC.CountNPCS(NPCType<TargonBoss>()) <= 0)
            {
                npc.active = false;
            }

            Lighting.AddLight(npc.Center, TargonBoss.TaricColor.ToVector3());


            return base.PreAI();
        }

        public override void AI()
        {
            NPC bossNPC = Main.npc.First(x => x.type == NPCType<TargonBoss>());

            if (Main.time % 2 == 0)
            {
                if (npc.lifeMax > 10)
                    TerraLeague.DustLine(bossNPC.Center + TerraLeague.CalcVelocityToPoint(bossNPC.Center, npc.Center, 128), npc.Center, 263, Main.rand.NextFloat(0.08f, 0.2f), 1.5f, TargonBoss.TaricColor);
                else
                    TerraLeague.DustLine(bossNPC.Center + TerraLeague.CalcVelocityToPoint(bossNPC.Center, npc.Center, 128), npc.Center, 263, Main.rand.NextFloat(0.08f, 0.1f), 1, TargonBoss.TaricColor);
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
                int count = 0;
                while ((double)count < damage / (double)npc.lifeMax * 50.0)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 263, 0f, 0f, 0, TargonBoss.TaricColor, 1.5f);
                    dust.noGravity = true;
                    count++;
                    break;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 263, 0f, 0f, 0, TargonBoss.TaricColor, 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            base.NPCLoot();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            spriteBatch.Draw
            (
                texture,
                npc.Center - Main.screenPosition,
                new Rectangle(0, 0, texture.Width, texture.Height),
                new Color(255, 255, 255, 255),
                npc.rotation,
                texture.Size() * 0.5f,
                npc.lifeMax > 10 ? 2 : 1,
                SpriteEffects.None,
                0f
            );
        }
    }
}
