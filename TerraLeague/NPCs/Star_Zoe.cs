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

namespace TerraLeague.NPCs
{
    public class Star_Zoe : ModNPC
    {
        const int State_Charging = 0;
        const int State_Attack = 1;

        int CurrentState
        {
            get
            {
                return (int)npc.ai[0];
            }

            set
            {
                npc.ai[0] = value;
            }
        }
        int Charge
        {
            get
            {
                return (int)npc.ai[1];
            }

            set
            {
                npc.ai[1] = value;
            }
        }
        float AltScale
        {
            get
            {
                return npc.localAI[0];
            }
            set
            {
                npc.localAI[0] = value;
            }
        }
        int AltAlpha
        {
            get
            {
                if (npc.localAI[1] <= 255 || npc.localAI[1] >= 0)
                    return (int)npc.localAI[1];
                else if (npc.localAI[1] < 0)
                    return 0;
                else
                    return 255;
            }
            set
            {
                npc.localAI[1] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shimmer of Twilight");
        }
        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 32;
            npc.defense = 0;
            npc.lifeMax = 10;
            npc.HitSound = new LegacySoundStyle(3, 5);
            npc.DeathSound = new LegacySoundStyle(4, 7);
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
            if (NPC.CountNPCS(NPCType<TargonBoss>()) <= 0)
            {
                npc.active = false;
            }

            Lighting.AddLight(npc.Center, TargonBoss.ZoeColor.ToVector3() * (AltAlpha / 255f) * (AltScale / 2f));
            return base.PreAI();
        }

        public override void AI()
        {
            if (CurrentState == State_Charging)
            {
                Charge++;
                AltScale = 2 * Charge / 300f;

                if (Charge < 51)
                {
                    AltAlpha += 5;
                }
                if (Charge == 60 * 4)
                {
                    npc.TargetClosest();
                    npc.netUpdate = true;
                }
                if (Charge == 60 * 5)
                {
                    CurrentState = State_Attack;
                }
            }
            else if (CurrentState == State_Attack)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.Center, new Vector2(8, 0).RotatedByRandom((double)MathHelper.TwoPi), ProjectileType<TargonBoss_PaddleStar>(), TargonBossAttack.ZoeDamage, 0);
                }
                npc.active = false;
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 27, 0);

                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 263, 0, 0, 150, TargonBoss.ZoeColor);
                    dust.noGravity = true;
                    dust.velocity *= 2;
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
                int count = 0;
                while ((double)count < damage / (double)npc.lifeMax * 50.0)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 263, 0f, 0f, 0, TargonBoss.ZoeColor, 1.5f);
                    dust.noGravity = true;
                    count++;
                    break;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 263, 0f, 0f, 0, TargonBoss.ZoeColor, 1.5f);
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
                new Color(255, 255, 255, 255/*AltAlpha*/),
                npc.rotation,
                texture.Size() * 0.5f,
                AltScale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
