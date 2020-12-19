using Microsoft.Xna.Framework;
using System;
using TerraLeague.Dusts;
using TerraLeague.Items;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class TargonBossAttack : ModNPC
    {
        const int mainBoss = 0;
        const int positionTimer = 1;
        const int stateTimer = 2;
        const int currentState = 3;

        const int State_Idle = 0;
        const int State_PanthAttack = 1;
        const int State_MorgAttack = 2;
        const int State_KayleAttack = 3;
        const int State_LeonaAttack = 4;
        const int State_DianaAttack = 5;
        const int State_TaricAttack = 6;
        const int State_ZoeAttack = 7;

        int PanthAttackTimer = 120 + 80;
        int MorgAttackTimer = 240 + 120 + 80;
        int KayleAttackTimer = 60 + 80;
        int LeonaAttackTimer = 270 + 80;
        int DianaAttackTimer = 150 + 80;
        int TaricAttackTimer = 60 + 80;
        int ZoeAttackTimer = 30 + 80;

        // Projectile deal 2x damage (4x in expert)
        public static int PanthDamage = 34/2;
        public static int MorgDamage = 30/2;
        public static int LeonaDamage = 24/2;
        public static int DianaDamage = 50/2;
        public static int ZoeDamage = 30/2;

        int starGoreLifeExt = 30;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starlight Catalyst");
        }
        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 32;
            npc.damage = 20;
            npc.defense = 0;
            npc.lifeMax = 5000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.scale = 1f;
            npc.noGravity = true;
            npc.immortal = true;
            npc.dontTakeDamage = true;
            npc.netAlways = true;

            npc.buffImmune[BuffType<Buffs.Stunned>()] = true;
            npc.buffImmune[BuffType<Buffs.TideCallerBubbled>()] = true;

            npc.ai[stateTimer] = 240;

            npc.alpha = 255;
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            if (!Main.npc[(int)npc.ai[mainBoss]].active)
            {
                npc.active = false;
            }

            float scale = 0.5f;
            if (GetState() == State_Idle)
            {
                if (GetStateTimer() < 60)
                    scale += 2.5f * (GetStateTimer() / 60f);
                else
                    scale = 3;
            }

            Color dustColor;
            switch ((int)GetMainBoss().ai[1])
            {
                case 1:
                    dustColor = TargonBoss.PanthColor;
                    break;
                case 2:
                    dustColor = TargonBoss.MorgColor;
                    break;
                case 3:
                    dustColor = TargonBoss.KayleColor;
                    break;
                case 4:
                    dustColor = TargonBoss.LeonaColor;
                    break;
                case 5:
                    dustColor = TargonBoss.DianaColor;
                    break;
                case 6:
                    dustColor = TargonBoss.TaricColor;
                    break;
                case 7:
                    dustColor = TargonBoss.ZoeColor;
                    break;
                default:
                    dustColor = default;
                    break;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 263, 0, 0, 0, dustColor, scale);
                dust.noGravity = true;
                dust.noLight = true;
                dust.fadeIn = 0;
            }
            return base.PreAI();
        }

        public override void AI()
        {
            npc.Center = GetMainBoss().Center + new Vector2(450, 0).RotatedBy(GetRadianTime());
            npc.position.Y -= 16 * (float)Math.Sin(MathHelper.ToRadians(GetMainBoss().ai[2]));

            if (GetState() == State_Idle)
            {
                if (GetStateTimer() < 60)
                    npc.ai[positionTimer] += (2 + (2 - (2 * GetMainBoss().life / (float)GetMainBoss().lifeMax))) * (GetStateTimer() / 60f);
                else
                {
                    npc.ai[positionTimer] += (2 + (2 - (2 * GetMainBoss().life / (float)GetMainBoss().lifeMax)));
                }
                if (npc.ai[positionTimer] >= 360 * 10)
                    npc.ai[positionTimer] -= 360 * 10;
            }
            else if (GetState() == State_PanthAttack)
            {
                if (npc.ai[stateTimer] < PanthAttackTimer - 80 && npc.ai[stateTimer] % 20 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectileDirect(npc.Center, TerraLeague.CalcVelocityToPoint(npc.Center, Main.player[npc.target].MountedCenter, 16), ProjectileType<TargonBoss_Spear>(), PanthDamage, 2);
                    }
                    TerraLeague.PlaySoundWithPitch(npc.Center, 2, 1, -0.5f);
                }
                else
                {
                    DrawPanthStars();
                }
            }
            else if (GetState() == State_KayleAttack)
            {
                if (GetStateTimer() < KayleAttackTimer - 80 && GetStateTimer() % 20 == 1)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 pos = new Vector2((int)npc.position.X + Main.rand.Next(-40, 40), (int)npc.position.Y + Main.rand.Next(-40, 40));
                        NPC.NewNPC((int)pos.X, (int)pos.Y, NPCType<KayleAttack>());
                    }
                    TerraLeague.PlaySoundWithPitch(npc.Center, 2, 113, 0);
                }
                else
                {
                    DrawKayleStars();
                }
            }
            else if (GetState() == State_MorgAttack)
            {
                if (GetStateTimer() <= MorgAttackTimer - 80 && (GetStateTimer() - 40) % 60 == 0 && GetStateTimer() > 180)
                {
                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        if (Main.player[i].active && !Main.player[i].dead)
                        {
                            if (Main.player[i].HasBuff(BuffType<Buffs.InTargonArena>()))
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Vector2 vel = TerraLeague.CalcVelocityToPoint(npc.Center, Main.player[i].MountedCenter, 24);

                                    Projectile proj = Projectile.NewProjectileDirect(npc.Center, vel, ProjectileType<TargonBoss_SoulShackles>(), MorgDamage, 0, 255, npc.whoAmI, -1);
                                    proj.ai[0] = npc.whoAmI;
                                    proj.ai[1] = -1;
                                }
                                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 1, -0.5f);
                            }
                        }
                    }
                }
                else
                {
                    DrawMorgStars();
                }
            }
            else if (GetState() == State_LeonaAttack)
            {
                if (GetStateTimer() == LeonaAttackTimer - 80)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ProjectileType<TargonBoss_SolarFlareControl>(), LeonaDamage, 0);
                    }
                    TerraLeague.PlaySoundWithPitch(npc.Center, 2, 34, 0);
                }
                else
                {
                    DrawLeonaStars();
                }
            }
            else if (GetState() == State_DianaAttack)
            {
                if (npc.ai[stateTimer] == DianaAttackTimer - 80)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ProjectileType<TargonBoss_Moonfall>(), DianaDamage, 2);
                    }
                }
                else
                {
                    DrawDianaStars();
                }
            }
            else if (GetState() == State_TaricAttack)
            {
                if (GetStateTimer() == KayleAttackTimer - 80/* && GetStateTimer() % 20 == 1*/)
                {
                    //Vector2 pos = new Vector2((int)npc.position.X + Main.rand.Next(-240, 240), (int)npc.position.Y + Main.rand.Next(-240, 240));

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC((int)npc.Center.X - 8, (int)npc.Center.Y, NPCType<TargonBoss_Gem>(), 0, 1);
                    }
                    TerraLeague.PlaySoundWithPitch(npc.Center, 2, 4, -0.5f);
                }
                else
                {
                    DrawTaricStars();
                }
            }
            else if (GetState() == State_ZoeAttack)
            {
                if (GetStateTimer() == ZoeAttackTimer - 80)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            int speed = Main.rand.Next(6, 10);
                            Projectile.NewProjectileDirect(npc.Center, new Vector2(speed, 0).RotatedBy((0.5f + i) * MathHelper.TwoPi / 8f), ProjectileType<TargonBoss_PaddleStar>(), ZoeDamage, 0);
                        }
                    }
                    TerraLeague.PlaySoundWithPitch(npc.Center, 2, 9, 0f);
                }
                else
                {
                    DrawZoeStars();
                }
            }

            npc.ai[stateTimer]--;
            if (npc.ai[stateTimer] <= 0)
            {
                if (GetState() == State_Idle)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int stateToSet = Main.rand.Next(0, 7);
                        switch (stateToSet)
                        {
                            case 0:
                                npc.ai[stateTimer] = PanthAttackTimer;
                                npc.ai[currentState] = State_PanthAttack;
                                break;
                            case 1:
                                npc.ai[stateTimer] = MorgAttackTimer;
                                npc.ai[currentState] = State_MorgAttack;
                                break;
                            case 2:
                                npc.ai[stateTimer] = KayleAttackTimer;
                                npc.ai[currentState] = State_KayleAttack;
                                break;
                            case 3:
                                npc.ai[stateTimer] = LeonaAttackTimer;
                                npc.ai[currentState] = State_LeonaAttack;
                                break;
                            case 4:
                                npc.ai[stateTimer] = DianaAttackTimer;
                                npc.ai[currentState] = State_DianaAttack;
                                break;
                            case 5:
                                npc.ai[stateTimer] = TaricAttackTimer;
                                npc.ai[currentState] = State_TaricAttack;
                                break;
                            case 6:
                                npc.ai[stateTimer] = ZoeAttackTimer;
                                npc.ai[currentState] = State_ZoeAttack;
                                break;
                            default:
                                npc.ai[stateTimer] = DianaAttackTimer;
                                npc.ai[currentState] = State_DianaAttack;
                                break;
                        }

                        npc.TargetClosest();
                        npc.netUpdate = true;
                    }
                }
                else
                {
                    npc.ai[currentState] = State_Idle;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        var bossnpc = GetMainBoss();
                        float healthScale = ((Main.expertMode ? 1.25f : 1.5f) - (1 - (bossnpc.life / (float)bossnpc.lifeMax)));
                        npc.ai[stateTimer] = (int)(Main.rand.Next(140, 240) * healthScale);
                        npc.netUpdate = true;
                    }
                }
            }

            base.AI();
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
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 0f, 0f, 0, new Color(100, 100, 100), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 0f, 0f, 0, new Color(5, 245, 150), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }

                //Gore.NewGore(npc.Center, npc.velocity / 2, mod.GetGoreSlot("Gores/Ghoul_1"), 1f);
                //Gore.NewGore(npc.Top, npc.velocity / 2, mod.GetGoreSlot("Gores/Ghoul_2"), 1f);
                //Gore.NewGore(npc.Center, npc.velocity / 2, mod.GetGoreSlot("Gores/Ghoul_3"), 1f);
                //Gore.NewGore(npc.Bottom, npc.velocity / 2, mod.GetGoreSlot("Gores/Ghoul_4"), 1f);
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            base.NPCLoot();
        }
        

        void DrawKayleStars()
        {
             int star1Time = KayleAttackTimer - 5;
             int star2Time = KayleAttackTimer - 15;
             int star3Time = KayleAttackTimer - 25;
             int star4Time = KayleAttackTimer - 35;
             int star5Time = KayleAttackTimer - 45;
             int star6Time = KayleAttackTimer - 55;
             int lineTime = KayleAttackTimer - 65;

            Vector2 star1Pos = new Vector2(0, -128) + npc.Center;
            Vector2 star2Pos = new Vector2(0, 128) + npc.Center;
            Vector2 star3Pos = new Vector2(-64, -64) + npc.Center;
            Vector2 star4Pos = new Vector2(64, -64) + npc.Center;
            Vector2 star5Pos = new Vector2(24, -64) + npc.Center;
            Vector2 star6Pos = new Vector2(-24, -64) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_" + GetState());

            if (GetStateTimer() == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.5f);
            }
            else if (GetStateTimer() == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.4f);
            }
            else if (GetStateTimer() == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.3f);
            }
            else if (GetStateTimer() == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.2f);
            }
            else if (GetStateTimer() == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.1f);
            }
            else if (GetStateTimer() == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, 0);
            }
            else if (GetStateTimer() == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star1Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star1Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                ChangeBossState();
            }
        }
        void DrawMorgStars()
        {
             int star1Time = MorgAttackTimer - 5;
             int star2Time = MorgAttackTimer - 13;
             int star3Time = MorgAttackTimer - 22;
             int star4Time = MorgAttackTimer - 30;
             int star5Time = MorgAttackTimer - 39;
             int star6Time = MorgAttackTimer - 47;
             int star7Time = MorgAttackTimer - 56;
             int lineTime = MorgAttackTimer - 65;

            Vector2 star1Pos = new Vector2(-96, 0) + npc.Center;
            Vector2 star2Pos = new Vector2(-48, -48) + npc.Center;
            Vector2 star3Pos = new Vector2(-48, 48) + npc.Center;
            Vector2 star4Pos = new Vector2(0, 0) + npc.Center;
            Vector2 star5Pos = new Vector2(48, 48) + npc.Center;
            Vector2 star6Pos = new Vector2(48, -48) + npc.Center;
            Vector2 star7Pos = new Vector2(96, 0) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_" + GetState());

            if (GetStateTimer() == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.6f);
            }
            else if (GetStateTimer() == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.5f);
            }
            else if (GetStateTimer() == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.4f);
            }
            else if (GetStateTimer() == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.3f);
            }
            else if (GetStateTimer() == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.2f);
            }
            else if (GetStateTimer() == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, -0.1f);
            }
            else if (GetStateTimer() == star7Time)
            {
                Gore gore = Gore.NewGorePerfect(star7Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star7Pos, 2, 4, 0);
            }
            else if (GetStateTimer() == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star1Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star5Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star6Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star7Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star7Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                ChangeBossState();
            }
        }
        void DrawPanthStars()
        {
             int star1Time = PanthAttackTimer - 5;
             int star2Time = PanthAttackTimer - 17;
             int star3Time = PanthAttackTimer - 29;
             int star4Time = PanthAttackTimer - 41;
             int star5Time = PanthAttackTimer - 53;
             int lineTime = PanthAttackTimer - 65;

            Vector2 star1Pos = new Vector2(0, -128) + npc.Center;
            Vector2 star2Pos = new Vector2(0, 128) + npc.Center;
            Vector2 star3Pos = new Vector2(-32, -64) + npc.Center;
            Vector2 star4Pos = new Vector2(32, -64) + npc.Center;
            Vector2 star5Pos = new Vector2(0, -16) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_" + GetState());

            if (GetStateTimer() == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.4f);
            }
            else if (GetStateTimer() == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.3f);
            }
            else if (GetStateTimer() == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.2f);
            }
            else if (GetStateTimer() == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.1f);
            }
            else if (GetStateTimer() == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, 0);
            }
            else if (GetStateTimer() == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star1Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star1Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star4Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                ChangeBossState();
            }
        }
        void DrawLeonaStars()
        {
            int star1Time = LeonaAttackTimer - 5;
            int star2Time = LeonaAttackTimer - 12;
            int star3Time = LeonaAttackTimer - 20;
            int star4Time = LeonaAttackTimer - 27;
            int star5Time = LeonaAttackTimer - 35;
            int star6Time = LeonaAttackTimer - 42;
            int star7Time = LeonaAttackTimer - 50;
            int star8Time = LeonaAttackTimer - 57;
            int lineTime = LeonaAttackTimer - 65;

            Vector2 star1Pos = new Vector2(0, 96) + npc.Center;
            Vector2 star2Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 3 / 8f) + npc.Center;
            Vector2 star3Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 6 / 8f) + npc.Center;
            Vector2 star4Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 1 / 8f) + npc.Center;
            Vector2 star5Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 4 / 8f) + npc.Center;
            Vector2 star6Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 7 / 8f) + npc.Center;
            Vector2 star7Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 2 / 8f) + npc.Center;
            Vector2 star8Pos = new Vector2(0, 96).RotatedBy(MathHelper.TwoPi * 5 / 8f) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_" + GetState());

            if (GetStateTimer() == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.7f);
            }
            else if (GetStateTimer() == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.6f);
            }
            else if (GetStateTimer() == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.5f);
            }
            else if (GetStateTimer() == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.4f);
            }
            else if (GetStateTimer() == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.3f);
            }
            else if (GetStateTimer() == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, -0.2f);
            }
            else if (GetStateTimer() == star7Time)
            {
                Gore gore = Gore.NewGorePerfect(star7Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star7Pos, 2, 4, -0.1f);
            }
            else if (GetStateTimer() == star8Time)
            {
                Gore gore = Gore.NewGorePerfect(star8Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star8Pos, 2, 4, 0);
            }
            else if (GetStateTimer() == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star4Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star5Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star6Pos, star7Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star7Pos, star8Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star8Pos, star1Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                ChangeBossState();
            }
        }
        void DrawDianaStars()
        {
            int star1Time = DianaAttackTimer - 5;
            int star2Time = DianaAttackTimer - 12;
            int star3Time = DianaAttackTimer - 20;
            int star4Time = DianaAttackTimer - 27;
            int star5Time = DianaAttackTimer - 35;
            int star6Time = DianaAttackTimer - 42;
            int star7Time = DianaAttackTimer - 50;
            int star8Time = DianaAttackTimer - 57;
            int lineTime = DianaAttackTimer - 65;

            Vector2 star1Pos = new Vector2(0, -96) + npc.Center;
            Vector2 star2Pos = new Vector2(0, -96).RotatedBy(MathHelper.TwoPi * 1 / 8f) + npc.Center;
            Vector2 star3Pos = new Vector2(0, -96).RotatedBy(MathHelper.TwoPi * 2 / 8f) + npc.Center;
            Vector2 star4Pos = new Vector2(0, -96).RotatedBy(MathHelper.TwoPi * 3 / 8f) + npc.Center;
            Vector2 star5Pos = new Vector2(0, -96).RotatedBy(MathHelper.TwoPi * 4 / 8f) + npc.Center;
            Vector2 star6Pos = new Vector2(0, -96).RotatedBy(MathHelper.TwoPi * 5 / 8f) + npc.Center;
            Vector2 star7Pos = new Vector2(0, -32).RotatedBy(MathHelper.TwoPi * 4 / 8f) + npc.Center;
            Vector2 star8Pos = new Vector2(0, -32).RotatedBy(MathHelper.TwoPi * 1 / 8f) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_" + GetState());

            if (GetStateTimer() == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.7f);
            }
            else if (GetStateTimer() == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.6f);
            }
            else if (GetStateTimer() == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.5f);
            }
            else if (GetStateTimer() == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.4f);
            }
            else if (GetStateTimer() == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.3f);
            }
            else if (GetStateTimer() == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, -0.2f);
            }
            else if (GetStateTimer() == star7Time)
            {
                Gore gore = Gore.NewGorePerfect(star7Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star7Pos, 2, 4, -0.1f);
            }
            else if (GetStateTimer() == star8Time)
            {
                Gore gore = Gore.NewGorePerfect(star8Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star8Pos, 2, 4, 0);
            }
            else if (GetStateTimer() == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star4Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star5Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star6Pos, star7Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star7Pos, star8Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star8Pos, star1Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                ChangeBossState();
            }
        }
        void DrawTaricStars()
        {
            int star1Time = TaricAttackTimer - 5;
            int star2Time = TaricAttackTimer - 15;
            int star3Time = TaricAttackTimer - 25;
            int star4Time = TaricAttackTimer - 35;
            int star5Time = TaricAttackTimer - 45;
            int star6Time = TaricAttackTimer - 55;
            int lineTime = TaricAttackTimer - 65;

            Vector2 star1Pos = new Vector2(0, -96) + npc.Center;
            Vector2 star2Pos = new Vector2(80, -80) + npc.Center;
            Vector2 star3Pos = new Vector2(48, 80) + npc.Center;
            Vector2 star4Pos = new Vector2(0, 96) + npc.Center;
            Vector2 star5Pos = new Vector2(-48, 80) + npc.Center;
            Vector2 star6Pos = new Vector2(-80, -80) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_" + GetState());

            if (GetStateTimer() == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.5f);
            }
            else if (GetStateTimer() == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.4f);
            }
            else if (GetStateTimer() == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.3f);
            }
            else if (GetStateTimer() == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.2f);
            }
            else if (GetStateTimer() == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.1f);
            }
            else if (GetStateTimer() == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, 0);
            }
            else if (GetStateTimer() == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star2Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star4Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star5Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star6Pos, star1Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                ChangeBossState();
            }
        }
        void DrawZoeStars()
        {
            int star1Time = ZoeAttackTimer - 5;
            int star2Time = ZoeAttackTimer - 15;
            int star3Time = ZoeAttackTimer - 25;
            int star4Time = ZoeAttackTimer - 35;
            int star5Time = ZoeAttackTimer - 45;
            int star6Time = ZoeAttackTimer - 55;
            int lineTime = ZoeAttackTimer - 65;

            Vector2 star1Pos = new Vector2(-96, 0) + npc.Center;
            Vector2 star2Pos = new Vector2(96, 0) + npc.Center;
            Vector2 star3Pos = new Vector2(-40, 0) + npc.Center;
            Vector2 star4Pos = new Vector2(0, -40) + npc.Center;
            Vector2 star5Pos = new Vector2(40, 0) + npc.Center;
            Vector2 star6Pos = new Vector2(0, 40) + npc.Center;

            int goreType = mod.GetGoreSlot("Gores/Star_" + GetState());

            if (GetStateTimer() == star1Time)
            {
                Gore gore = Gore.NewGorePerfect(star1Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star1Pos, 2, 4, -0.5f);
            }
            else if (GetStateTimer() == star2Time)
            {
                Gore gore = Gore.NewGorePerfect(star2Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star2Pos, 2, 4, -0.4f);
            }
            else if (GetStateTimer() == star3Time)
            {
                Gore gore = Gore.NewGorePerfect(star3Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star3Pos, 2, 4, -0.3f);
            }
            else if (GetStateTimer() == star4Time)
            {
                Gore gore = Gore.NewGorePerfect(star4Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star4Pos, 2, 4, -0.2f);
            }
            else if (GetStateTimer() == star5Time)
            {
                Gore gore = Gore.NewGorePerfect(star5Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star5Pos, 2, 4, -0.1f);
            }
            else if (GetStateTimer() == star6Time)
            {
                Gore gore = Gore.NewGorePerfect(star6Pos, Vector2.Zero, goreType, Main.rand.NextFloat(1f, 2));
                gore.timeLeft = GetStateTimer() + starGoreLifeExt;
                TerraLeague.PlaySoundWithPitch(star6Pos, 2, 4, 0);
            }
            else if (GetStateTimer() == lineTime)
            {
                TerraLeague.DustLine(star1Pos, star2Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star3Pos, star4Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star4Pos, star5Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star5Pos, star6Pos, 261, 0.5f, 2);
                TerraLeague.DustLine(star6Pos, star3Pos, 261, 0.5f, 2);
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, 0);

                ChangeBossState();
            }
        }

        void ChangeBossState()
        {
            TerraLeague.DustRing(263, GetMainBoss(), GetAttackColor);
            GetMainBoss().ai[1] = GetState();
            GetMainBoss().netUpdate = true;
        }

        Color GetAttackColor
        {
            get
            {
                switch (GetState())
                {
                    case 1:
                        return TargonBoss.PanthColor;
                    case 2:
                        return TargonBoss.MorgColor;
                    case 3:
                        return TargonBoss.KayleColor;
                    case 4:
                        return TargonBoss.LeonaColor;
                    case 5:
                        return TargonBoss.DianaColor;
                    case 6:
                        return TargonBoss.TaricColor;
                    case 7:
                        return TargonBoss.ZoeColor;
                    default:
                        return Color.White;
                }
            }
        }

        NPC GetMainBoss()
        {
            return Main.npc[(int)npc.ai[mainBoss]];
        }
        int GetState()
        {
            return (int)npc.ai[currentState];
        }
        float GetRadianTime()
        {
            return npc.ai[positionTimer] * (MathHelper.Pi / 180f);
        }
        int GetStateTimer()
        {
            return (int)npc.ai[stateTimer];
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
    }
}
