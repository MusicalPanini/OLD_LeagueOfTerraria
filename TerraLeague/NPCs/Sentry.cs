using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class Sentry : ModNPC
    {

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 23;
            DisplayName.SetDefault("Blue Sentry");
        }

        private const int State_Idle = 0;
        private const int State_Chasing = 1;
        private const int State_Charging = 2;

        private const int AI_State_Slot = 0;
        private const int Attack_Timer_Slot = 1;



        public override void SetDefaults()
        {
            npc.scale = 1.5f;
            npc.rarity = 2;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.width = 36;
            npc.height = 40;
            npc.aiStyle = -1;
            npc.npcSlots = 5;

            if (NPC.downedPlantBoss)
            {
                npc.damage = 70;
                npc.defense = 40;
                npc.lifeMax = 2000;
            }
            else if (NPC.downedMechBossAny)
            {
                npc.damage = 48;
                npc.defense = 22;
                npc.lifeMax = 500;
            }
            else
            {
                npc.damage = 35;
                npc.defense = 16;
                npc.lifeMax = 250;
            }
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath44;
            npc.value = 12000;
            npc.knockBackResist = 0;
            npc.buffImmune[BuffID.OnFire] = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //if (spawnInfo.spawnTileX < Main.mapMaxX / 3 || spawnInfo.spawnTileX > (Main.mapMaxX * 2) / 3)   
            //    return SpawnCondition.OverworldDaySlime.Chance * 0.03f;
            //else
                return 0;
        }

        public float AI_State
        {
            get
            {
                return npc.ai[AI_State_Slot];
            }
            set
            {
                npc.ai[AI_State_Slot] = value;
            }
        }

        public float Attack_Timer
        {
            get
            {
                return npc.ai[Attack_Timer_Slot];
            }
            set
            {
                npc.ai[Attack_Timer_Slot] = value;
            }
        }

        public override void AI()
        {
            if (npc.life < npc.lifeMax && AI_State == State_Idle)
            {
                npc.netUpdate = true;
                npc.noTileCollide = true;
                AI_State = State_Chasing;
            }

            if (AI_State == State_Idle)
            {
                if ((int)(npc.Center.Y / 16) == FindGround() + 2)
                {
                    npc.velocity = Vector2.Zero;
                }
                else
                {
                    npc.velocity = new Vector2(0, 1);
                }
            }

            if (AI_State == State_Chasing)
            {
                npc.TargetClosest();

                if (npc.Distance(Main.player[npc.target].Center) > 450)
                {
                    float spd = npc.velocity.Length();

                    if (spd == 0)
                        spd = 0.5f;
                    else if (spd > 3.5)
                        spd *= 0.98f;
                    else if (spd < 3.5)
                        spd *= 1.02f;

                    npc.velocity = TerraLeague.CalcVelocityToPoint(npc.Center, Main.player[npc.target].Top, spd);
                }
                else if (npc.Distance(Main.player[npc.target].Center) > 200)
                {
                    float spd = npc.velocity.Length();

                    if (spd == 0)
                        spd = 0.5f;
                    else if (spd > 2)
                        spd *= 0.98f;
                    else if (spd < 2)
                        spd *= 1.02f;

                    npc.velocity = TerraLeague.CalcVelocityToPoint(npc.Center, Main.player[npc.target].Top, spd);
                }
                else
                {
                    float spd = npc.velocity.Length();

                    if (spd != 0)
                        spd *= 0.98f;

                    npc.velocity = TerraLeague.CalcVelocityToPoint(npc.Center, Main.player[npc.target].Top, spd);
                }

                if (npc.Distance(Main.player[npc.target].Center) < 450)
                {
                    Attack_Timer++;

                    if ((Attack_Timer == 60) || (Attack_Timer == 30 && Main.expertMode))
                    {
                        npc.netUpdate = true;
                        Attack_Timer = 0;
                        AI_State = State_Charging;
                    }

                    if (Attack_Timer > 60)
                    {
                        Attack_Timer = 0;
                    }
                }
            }

            if (AI_State == State_Charging)
            {
                if (Math.Round(npc.frameCounter, 1) <= 20)
                    Lighting.AddLight(npc.Center, 0, 0, (float)((npc.frameCounter - 12) / 10));

                float spd = npc.velocity.Length();

                if (spd != 0)
                    spd *= 0.98f;

                npc.velocity = TerraLeague.CalcVelocityToPoint(npc.Center, Main.player[npc.target].Top, spd);

                if (Math.Round(npc.frameCounter,1) == 20)
                {
                    if (Main.hardMode)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Projectile proj = Projectile.NewProjectileDirect(npc.Center, TerraLeague.CalcVelocityToPoint(npc.Center, Main.player[npc.target].Center, (16 * (i + 1)) / 3), ProjectileID.SapphireBolt, npc.damage / 4, 2);
                            proj.friendly = false;
                            proj.hostile = true;
                        }
                    }
                    else
                    {
                        Projectile proj = Projectile.NewProjectileDirect(npc.Center, TerraLeague.CalcVelocityToPoint(npc.Center, Main.player[npc.target].Center, 16), ProjectileID.SapphireBolt, npc.damage / 4, 2);
                        proj.friendly = false;
                        proj.hostile = true;
                    }
                    
                    Main.PlaySound(new LegacySoundStyle(2, 10), npc.Center);
                }

                if (npc.frameCounter > 22.8)
                {
                    npc.netUpdate = true;
                    AI_State = State_Chasing;
                }

            }

            npc.rotation = (npc.velocity.X / 8);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y - 15), 1, 1, 56, 0, 0, 0, default(Color), 1.2f);
                }

                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SentryTop"), 1.5f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SentryLeft"), 1.5f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SentryRight"), 1.5f);
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.Hitbox, ItemType<SmallBlueBuffOrb>());
            
            base.NPCLoot();
        }

        private int FindGround()
        {
            for (int i = (int)(npc.Center.Y/16); i < Main.mapMaxY; i++)
            {
                if (Main.tile[(int)(npc.Center.X/16), i].active())
                {
                    return i;
                }
            }

            return Main.mapMaxY;
        }

        public override void FindFrame(int frameHeight)
        {
            if (AI_State == State_Charging)
            {
                if ((int)npc.frameCounter > 22 || (int)npc.frameCounter < 12)
                    npc.frameCounter = 12;
            }
            else
            {
                if ((int)npc.frameCounter > 11 || (int)npc.frameCounter < 0)
                    npc.frameCounter = 0;
            }

            npc.frame.Y = (int)npc.frameCounter * frameHeight;
            npc.frameCounter += 0.2d;
        }
    }
}
