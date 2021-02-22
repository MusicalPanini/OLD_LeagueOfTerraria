using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;
using System;
using TerraLeague.Items.SummonerSpells;

namespace TerraLeague.NPCs
{
    public class TerraLeagueNPCsGLOBAL : GlobalNPC
    {
        internal NPCPacketHandler PacketHandler = new NPCPacketHandler(2);
        public float initialSpeed = 1;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        int timer = 60;
        public int baseDefence = 0;
        public int requiemDamage = 0;
        public int requiemTime = 0;
        public int vesselTarget = -1;
        public int vesselTimer = 420;
        public bool requiem = false;
        public bool bubbled = false;
        public bool slowed = false;
        public bool umbralTrespass = false;
        public bool torment = false;
        public bool abyssalCurse = false;
        public bool grievousWounds = false;
        public bool ignited = false;
        public bool exhaused = false;
        public bool weakSunfire = false;
        public bool sunfire = false;
        public bool OrgDest = false;
        public bool essenFlux = false;
        public bool frozen = false;
        public bool ablaze = false;
        public bool illuminated = false;
        public bool seeded = false;
        public bool vessel = false;
        public bool harbingersInferno = false;
        public bool doomed = false;
        public bool maleficVisions = false;
        public bool calibrumMark = false;
        public bool gravitumMark = false;
        public bool infernumMark = false;
        public bool icebornSubjugation = false;
        public int icebornSubjugationOwner = -1;

        public bool snared = false;
        public bool stunned = false;

        public int CleavedStacks = 0;
        public bool cleaved = false;

        public int HemorrhageStacks = 0;
        public bool hemorrhage = false;

        public int DeadlyVenomStacks = 0;
        public bool deadlyVenom = false;

        public bool CausticWounds = false;
        public int CausticStacks = 0;

        public int PoxStacks = 0;
        public bool pox = false;


        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight || npc.type == NPCID.GolemHead ||
                npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail)
            {
                npc.buffImmune[BuffType<Stunned>()] = true;
                npc.buffImmune[BuffType<TideCallerBubbled>()] = true;

            }
            base.SetDefaults(npc);
        }

        public override void ResetEffects(NPC npc)
        {
            if (bubbled)
            {
                npc.velocity.Y = 0;
            }
            requiem = false;
            bubbled = false;
            if (!slowed)
                icebornSubjugation = false;
            slowed = false;
            umbralTrespass = false;
            torment = false;
            abyssalCurse = false;
            grievousWounds = false;
            ignited = false;
            exhaused = false;
            weakSunfire = false;
            sunfire = false;
            OrgDest = false;
            essenFlux = false;
            frozen = false;
            ablaze = false;
            illuminated = false;
            seeded = false;
            harbingersInferno = false;
            doomed = false;
            maleficVisions = false;
            calibrumMark = false;
            gravitumMark = false;
            infernumMark = false;

            snared = false;
            stunned = false;

            if (!cleaved)
                CleavedStacks = 0;
            cleaved = false;

            if (!hemorrhage)
                HemorrhageStacks = 0;
            hemorrhage = false;

            if (!deadlyVenom)
                DeadlyVenomStacks = 0;
            deadlyVenom = false;

            if (!CausticWounds)
                CausticStacks = 0;
            CausticWounds = false;

            if (!pox)
                PoxStacks = 0;
            pox = false;

            npc.defense = npc.defDefense;
            npc.damage = npc.defDamage;
        }


        public override bool PreAI(NPC npc)
        {
            // Dust Effects
            Dust dust;
            if (slowed)
            {
                if (Main.rand.Next(0, 8) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 113, 0f, 0f, 100, default(Color));
                    dust.velocity *= 0.2f;
                    dust.scale *= 1.2f;
                    dust.alpha *= 200;
                }
            }
            if (umbralTrespass)
            {
                int num = Main.rand.Next(0, 10);
                if (num == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 186, 0, -1, 150);
                    dust.velocity.X *= 0.3f;
                    dust.color = new Color(255, 0, 0);
                    dust.noGravity = false;
                }
                else if (num == 2)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 186, 0, -1, 150);
                    dust.velocity.X *= 0.3f;

                    dust.noGravity = false;
                }

            }
            if (requiem)
            {
                Color color = Main.rand.NextBool() ? new Color(0, 255, 140) : new Color(0, 255, 0);
                dust = Dust.NewDustDirect(new Vector2(npc.Center.X - 4, npc.position.Y - 320 + npc.height / 3f), 1, 300 + npc.height / 2, 186, 0f, 2f, 197, color, 2f);
                dust.noGravity = true;
                dust.velocity.X *= 0.1f;
                dust.fadeIn = 2.6f;
            }
            if (torment)
            {
                if (Main.rand.Next(0, 8) == 0)
                {
                    Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color));
                }
            }
            if (abyssalCurse)
            {
                if (Main.rand.Next(0, 4) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height / 2, 14, 0f, 0f, 100, default(Color));
                    dust.color = new Color(255, 0, 255);
                    dust.alpha = 150;
                    dust.scale = 1f;
                    dust.velocity.X = 0;
                    dust.velocity.Y = -0.5f;
                }
            }
            if (OrgDest)
            {
                if (Main.rand.Next(0, 3) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 112, 0f, 0f, 255, new Color(59, 0, 255));
                    dust.alpha = 150;
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
            if (essenFlux)
            {
                for (int i = 0; i < 2; i++)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X - 3, npc.position.Y + (npc.height / 2)), npc.width + 6, 4, 159, 0f, 0f, 50, default(Color));
                    dust.noGravity = true;
                }
            }
            if (grievousWounds)
            {
                if (Main.rand.Next(0, 8) == 0)
                {
                    dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 14, 0f, 0f, 100, default(Color), 1f);
                    dust.color = new Color(255, 0, 0);
                    dust.velocity.X = 0;
                    dust.velocity.Y = 0.5f;
                }
            }
            if (ignited)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, -12f, 100, default(Color), 6);
                    dust.noGravity = true;
                }
            }
            if (sunfire || weakSunfire)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 2f);
                    dust.noGravity = true;
                }

                if (Main.rand.Next(0, 4) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 1f);
                    dust.velocity.Y = -Math.Abs(dust.velocity.Y * 2f);
                }
            }
            if (CausticWounds)
            {
                if (Main.rand.Next(0, 8) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 112, 0f, 0f, 100, new Color(59, 0, 255));
                    dust.noGravity = true;
                    dust.velocity *= 0;
                }
            }
            if (ablaze)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 0, new Color(255, 0, 0), 4);
                    dust.noGravity = true;
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 0, new Color(255, 0, 0), 1);
                }
            }
            if (infernumMark)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 88, 0f, -2f, 0, default(Color), 2);
                    dust.noGravity = true;
                    dust.velocity.X *= 0.1f;
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 88, 0f, -3f, 0, default(Color), 0.5f);
                    dust.velocity.X *= 0.2f;
                }
            }
            if (doomed)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 110, 0f, 0f, 0, new Color(0, 255, 201), 2);
                    dust.noGravity = true;
                    dust.velocity *= 0;

                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 110, 0f, 0f, 0, new Color(0, 255, 201), 1);
                    dust.noGravity = true;
                    dust.velocity *= 0;
                }
            }
            if (harbingersInferno)
            {
                dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 0, default(Color), 3);
                dust.noGravity = true;
                dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 0, default(Color), 1);
            }
            if (cleaved)
            {
                int num = Main.rand.Next(0, 8);
                if (num == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 186, 0, 1, 200, new Color(50, 0, 20));
                    dust.velocity.X *= 0.1f;
                    dust.velocity.Y -= 3f;
                    dust.noGravity = false;
                }
            }
            if (deadlyVenom)
            {
                int num = Main.rand.Next(0, 4);
                if (num == 0)
                {
                    dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 167, 0f, -1f, 125, new Color(0, 192, 255), 1f);
                    dust.velocity.X *= 0.1f;
                    dust.velocity.Y = -System.Math.Abs(dust.velocity.Y);
                    dust.noGravity = true;
                    dust.fadeIn = 1.5f;
                }
            }
            if (hemorrhage)
            {
                int num = Main.rand.Next(0, 4);
                if (num == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 5, 0, 0, 0, default(Color), 1.25f);
                    dust.velocity.X *= 0f;
                    dust.velocity.Y = System.Math.Abs(dust.velocity.Y);
                }
            }
            if (pox)
            {
                int num = Main.rand.Next(0, 4);
                if (num == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 167, 0, 0, 0, default(Color), 1.25f);
                    dust.velocity.X *= 0f;
                    dust.velocity.Y = System.Math.Abs(dust.velocity.Y);
                    dust.noGravity = true;
                }
            }
            if (illuminated)
            {
                dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 228, 0f, 0f, 0, default(Color), 1);
                dust.noGravity = true;
                dust.velocity *= 1.3f;
            }
            if (vessel)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 59, 0f, -2f, 200, new Color(0, 255, 201), 3f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 2;

                    dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 59, 0f, -1f, 200, new Color(0, 255, 201), 3f);
                    dust.noGravity = true;
                    dust.velocity.Y -= 2;
                }
            }
            if (calibrumMark)
            {
                dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 111, 0, -2, 100, default(Color), 1f);
                dust.noGravity = true;
                dust.velocity.X *= 0;
            }
            if (gravitumMark)
            {
                dust = Dust.NewDustDirect(new Vector2(npc.Center.X - 16, npc.position.Y - 16), 32, 32, 71, 0f, 0f, 100, new Color(0, 0, 0), 1f);
                dust.noGravity = true;
                dust.fadeIn = 1.2f;
                dust.velocity = (dust.position - npc.Center) * -0.05f;
            }

            npc.defense = npc.defDefense;
            npc.damage = npc.defDamage;

            if (bubbled)
            {
                npc.velocity.Y = 0;
                npc.position.X = npc.oldPosition.X;
                npc.position.Y = npc.oldPosition.Y - 1;
            }
            if (slowed)
            {
                npc.damage = (int)(npc.damage * 0.7);
                npc.position = new Vector2((npc.oldPosition.X + npc.position.X) / 2, (npc.oldPosition.Y + npc.position.Y) / 2);
            }
            if (cleaved)
                npc.defense -= (int)(npc.defense * 0.06 * (CleavedStacks + 1));


            timer--;
            if (timer <= 0)
                timer = 60;

            if (vessel)
            {
                vesselTimer--;

                if (vesselTimer <= 0 || !Main.npc[vesselTarget].active)
                {
                    npc.life = 0;
                }
            }

            if (stunned || bubbled || vessel)
            {
                npc.frameCounter = 0;
                npc.velocity = Vector2.Zero;
                if (vessel)
                {
                    npc.position = vesselTimer == 419 ? npc.position : npc.oldPosition;
                    npc.SpawnedFromStatue = true;
                }

                return false;
            }

            return base.PreAI(npc);
        }
        public override void AI(NPC npc)
        {
            
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            PLAYERGLOBAL modPlayer = spawnInfo.player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.zoneSurfaceMarble)
            {
                pool.Remove(NPCID.GreenSlime);
                pool.Remove(NPCID.BlueSlime);

                pool.Add(NPCType<MarbleSlime>(), SpawnCondition.OverworldDaySlime.Chance);

                if (!Main.dayTime)
                    pool.Add(NPCID.GreekSkeleton, SpawnCondition.OverworldNightMonster.Chance);

                if (Main.hardMode)
                    pool.Add(NPCID.Medusa, 0.2f);

            }
            

            base.EditSpawnPool(pool, spawnInfo);
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (deadlyVenom)
            {
                npc.lifeRegen -= (int)(2 * (DeadlyVenomStacks + 1));

                if (damage < (DeadlyVenomStacks + 1))
                {
                    damage = (DeadlyVenomStacks + 1);
                }
            }
            if (hemorrhage)
            {
                npc.lifeRegen -= (int)(4 * (HemorrhageStacks + 1));

                if (damage < (HemorrhageStacks + 1))
                {
                    damage = (HemorrhageStacks + 1);
                }
            }
            if (harbingersInferno)
            {
                npc.lifeRegen -= 40;

                if (damage < 2)
                {
                    damage = 2;
                }
            }
            if (ablaze)
            {
                npc.lifeRegen -= 24;

                if (damage < 3)
                {
                    damage = 3;
                }
            }
            if (infernumMark)
            {
                npc.lifeRegen -= 300;

                if (damage < 100)
                {
                    damage = 100;
                }
            }
            if (sunfire)
            {
                npc.lifeRegen -= 100;

                if (damage < 50)
                {
                    damage = 50;
                }
            }
            else if (weakSunfire)
            {
                npc.lifeRegen -= 20;

                if (damage < 10)
                {
                    damage = 10;
                }
            }
            if (ignited)
            {
                npc.lifeRegen -= IgniteRune.GetDOTDamage();
                if (damage < 10)
                    damage = 10;
                //int regen;
                //if (NPC.downedGolemBoss)
                //{
                //    regen = 144;
                //    if (damage < 50)
                //        damage = 50;
                //}
                //else if (NPC.downedPlantBoss)
                //{
                //    regen = 64;
                //    if (damage < 25)
                //        damage = 25;
                //}
                //else if (NPC.downedMechBossAny)
                //{
                //    regen = 38;
                //    if (damage < 10)
                //        damage = 10;
                //}
                //else if (Main.hardMode)
                //{
                //    regen = 24;
                //    if (damage < 5)
                //        damage = 5;
                //}
                //else if (NPC.downedBoss2)
                //{
                //    regen = 12;
                //    if (damage < 2)
                //        damage = 2;
                //}
                //else
                //{
                //    regen = 4;
                //    if (damage < 1)
                //        damage = 1;
                //}
                //npc.lifeRegen -= regen * 4;
            }
            if (torment)
            {
                int regen;
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }

                if (npc.boss)
                {
                    regen = 500;
                }
                else
                {
                    regen = (int)(npc.lifeMax * 0.1f);
                    if (regen > 500)
                    {
                        regen = 500;
                    }
                }

                if (slowed)
                {
                    regen *= 2;
                }

                if (regen < 30)
                {
                    regen = 30;
                }

                npc.lifeRegen -= regen;

                if (damage < 10)
                {
                    damage = 10;
                }
            }
            if (maleficVisions)
            {
                npc.lifeRegen -= 20;

                if (damage < 2)
                {
                    damage = 2;
                }
            }

            if (grievousWounds && npc.lifeRegen < 0)
            {
                npc.lifeRegen = (int)(npc.lifeRegen * 1.5);
            }
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.targonArena)
            {
                maxSpawns = 0;
            }
            if (modPlayer.zoneBlackMist)
            {
                maxSpawns =(int)(maxSpawns * 2);
                if (spawnRate > 300)
                {
                    spawnRate = 300;
                }
                    spawnRate = (int)(spawnRate * 0.35);
            }
            if (modPlayer.Disruption)
            {
                spawnRate = (int)(spawnRate * 2);
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (calibrumMark)
            {
                damage = (int)(damage * 1.5);
            }

            vesselStriked(player.whoAmI, damage, crit);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (calibrumMark)
            {
                damage = (int)(damage * 1.5);
            }

            if (OrgDest)
                damage = (int)(damage * 1.1);

            if (projectile.owner != -1 || projectile.owner != 255)
                vesselStriked(projectile.owner, damage, crit);

            if (projectile.type == ProjectileType<EyeofGod_TestofSpirit>())
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    PacketHandler.SendCreateVessel(-1, projectile.owner, npc.whoAmI, projectile.owner);
                }
                else
                {
                    Player player = Main.player[projectile.owner];
                    int vessel = NPC.NewNPC((int)player.Bottom.X + (64 * player.direction), (int)player.Bottom.Y, npc.type);
                    Main.npc[vessel].life = npc.life;
                    Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vesselTarget = npc.whoAmI;
                    Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vessel = true;
                    Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vesselTimer = 420;
                }
                //Main.npc[vessel].AddBuff(ModContent.BuffType<Vessel>(), 60 * 7);
            }

            base.ModifyHitByProjectile(npc, projectile, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool CheckDead(NPC npc)
        {
            return base.CheckDead(npc);
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.ArmsDealer && NPC.downedBoss2)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Revolver);
                nextSlot++;
            }

            if (type == NPCID.Dryad)
            {
                shop.item[nextSlot].SetDefaults(ItemType<Items.Placeable.PetSeeds>());
                nextSlot++;
            }
        }

        public override bool PreNPCLoot(NPC npc)
        {
            if (vessel)
                return false;

            return base.PreNPCLoot(npc);
        }

        public override void NPCLoot(NPC npc)
        {
            if (icebornSubjugation)
            {
                for (int i = 0; i < 20; i++)
                {
                    int type;
                    switch (Main.rand.Next(0, 3))
                    {
                        case 0:
                            type = ProjectileType<DarkIceTome_IceShardSmallA>();
                            break;
                        case 1:
                            type = ProjectileType<DarkIceTome_IceShardSmallB>();
                            break;
                        default:
                            type = ProjectileType<DarkIceTome_IceShardSmallC>();
                            break;
                    }

                    Projectile.NewProjectileDirect(npc.Center, new Vector2(0, Main.rand.NextFloat(9, 12)).RotatedByRandom(MathHelper.TwoPi), type, npc.lifeMax/5, 1, icebornSubjugationOwner, npc.whoAmI);
                }
            }
            if (seeded)
            {
                if (Main.rand.Next(0, 4) == 0)
                {
                    int x = Main.rand.Next(-5, 4);
                    x *= Main.rand.Next(0, 2) == 0 ? 1 : -1;
                    Projectile.NewProjectile(npc.Center, new Vector2(x, -8), ProjectileType<StrangleThornsTome_Seed>(), 0, 0, 255);
                }
            }

            base.NPCLoot(npc);
        }

        public bool CheckJungle(int X, int Y)
        {
            int JGrass = 0;
            int mud = 0;

            for (int x = X - 5; X < X + 5; x++)
            {
                for (int y = Y - 5; y < Y + 5; y++)
                {
                    if (Framing.GetTileSafely(x, y).type == TileID.Mud)
                    {
                        mud++;
                        return true;
                    }
                    if (Framing.GetTileSafely(x, y).type == TileID.JungleGrass)
                    {
                        JGrass++;
                    }
                }
            }

            if (JGrass > 0 || mud > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (ignited || ablaze)
            {
                drawColor = new Color(150, 0, 0);
            }
            if (exhaused)
            {
                drawColor = new Color(199, 160, 14);
            }
            if (deadlyVenom)
            {
                drawColor = new Color(0, 160, 0);
            }
            if (stunned && !frozen)
            {
                drawColor = new Color(255, 255, 0);
            }
            if (OrgDest)
            {
                drawColor = new Color(255, 0, 255);
            }
            if (vessel)
            {
                drawColor = new Color(0, 255, 144);
            }
            if (icebornSubjugation)
            {
                drawColor = new Color(0, 144, 255);
            }

            base.DrawEffects(npc, ref drawColor);
        }

        public double OnHitDamage(NPC npc, Player player, int Damage, float knockBack = 0, int hitDirection = 0, bool crit = false, bool noEffect = false, bool fromNet = false)
        {
            if (!npc.active || npc.life <= 0)
            {
                return 0.0;
            }
            double num = (double)Damage;
            int num2 = npc.defense;
            if (npc.ichor)
            {
                num2 -= 20;
            }
            if (npc.betsysCurse)
            {
                num2 -= 40;
            }
            if (num2 < 0)
            {
                num2 = 0;
            }
            if (NPCLoader.StrikeNPC(npc, ref num, num2, ref knockBack, hitDirection, ref crit))
            {
                num = Main.CalculateDamage((int)num, num2);
                if (crit)
                {
                    num *= 1.5;
                }
                if (npc.takenDamageMultiplier > 1f)
                {
                    num *= (double)npc.takenDamageMultiplier;
                }
            }
            if ((npc.takenDamageMultiplier > 1f || Damage != 9999) && npc.lifeMax > 1)
            {
                if (npc.friendly)
                {
                    Color color = crit ? Color.Purple : Color.MediumPurple;
                    CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y + 40, npc.width, npc.height), color, (int)num, false, false);
                }
                else
                {
                    Color color2 = crit ? Color.Purple : Color.MediumPurple;
                    
                    CombatText.NewText(new Rectangle((int)npc.position.X + 32, (int)npc.position.Y, npc.width, npc.height), color2, (int)num, false, false);
                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    PacketHandler.SendBattleText(-1, player.whoAmI, (int)num, npc.whoAmI, crit);
                }
            }
            if (num >= 1.0)
            {
                if (!npc.immortal)
                {
                    if (npc.realLife >= 0)
                    {
                        Main.npc[npc.realLife].life -= (int)num;
                        npc.life = Main.npc[npc.realLife].life;
                        npc.lifeMax = Main.npc[npc.realLife].lifeMax;
                    }
                    else
                    {
                        npc.life -= (int)num;
                    }
                }
                if (npc.realLife >= 0)
                {
                    Main.npc[npc.realLife].checkDead();
                }
                else
                {
                    npc.HitEffect(hitDirection, num);
                    npc.checkDead();
                }
                return num;
            }
            return 0.0;
        }

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (target.HasBuff(BuffType<UmbralTrespassing>()) || vessel || stunned)
                return false;
            else
                return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            if (vessel)
            {
                Texture2D texture = mod.GetTexture("Gores/VesselLink");

                Vector2 position = npc.Center + new Vector2(0, 6);
                Vector2 center = Main.npc[vesselTarget].Center;
                float num1 = (float)texture.Height;
                Vector2 vector2_4 = center - position;
                bool flag = true;
                if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                    flag = false;
                if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                    flag = false;
                while (flag)
                {
                    if ((double)vector2_4.Length() < (double)num1 + 1.0)
                    {
                        flag = false;
                    }
                    else
                    {
                        Vector2 vector2_1 = vector2_4;
                        vector2_1.Normalize();
                        position += vector2_1 * num1;
                        vector2_4 = center - position;

                        if (Main.rand.Next(0, 6) == 0)
                        {
                            Dust dust = Dust.NewDustPerfect(position, 59, null, 200, new Color(0, 255, 201), 3f);
                            dust.noGravity = true;
                        }

                    }
                }
            }

            return base.PreDraw(npc, spriteBatch, drawColor);
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            if (frozen)
            {
                Texture2D texture = mod.GetTexture("Gores/FrozenEffect");
                Color color = Color.White;
                color.A = 200;
                spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        npc.position.X - Main.screenPosition.X + npc.width * 0.5f,
                        npc.position.Y - Main.screenPosition.Y + npc.height * 0.5f
                    ),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    color,
                    0,
                    texture.Size() * 0.5f,
                    ((npc.width > npc.height ? npc.width : npc.height) / 56f) + 0.2f,
                    SpriteEffects.None,
                    0f
                );
            }

            if (CausticWounds)
            {
                Texture2D texture;
                int timeLeft = 0;
                if (npc.HasBuff(BuffType<CausticWounds>()))
                    timeLeft = npc.buffTime[npc.FindBuffIndex(BuffType<CausticWounds>())];

                switch (CausticStacks)
                {
                    case 1:
                        texture = mod.GetTexture("Gores/Caustic1");
                        break;
                    case 2:
                        texture = mod.GetTexture("Gores/Caustic2");
                        break;
                    case 3:
                        texture = mod.GetTexture("Gores/Caustic3");
                        break;
                    case 4:
                        texture = mod.GetTexture("Gores/Caustic4");
                        break;
                    case 5:
                        texture = mod.GetTexture("Gores/Caustic5");
                        break;
                    default:
                        texture = mod.GetTexture("Gores/Caustic5");
                        break;
                }

                if (CausticStacks < 5 || (CausticStacks >= 5 && timeLeft > 210))
                {
                    spriteBatch.Draw
                    (
                        texture,
                        new Vector2
                        (
                            npc.position.X - Main.screenPosition.X + npc.width * 0.5f,
                            npc.position.Y - Main.screenPosition.Y + npc.height * 1.25f
                        ),
                        new Rectangle(0, 0, texture.Width, texture.Height),
                        new Color(255, 0, 255, 255),
                        0,
                        texture.Size() * 0.5f,
                        (npc.width / 128f) + (CausticStacks >= 5 ? (timeLeft > 210 ? ((timeLeft - 210) / 30f) : 0f) : 0.25f),
                        SpriteEffects.None,
                        0f
                    ); ;
                }
            }

            if (deadlyVenom)
            {
                Texture2D texture;

                switch (DeadlyVenomStacks)
                {
                    case 0:
                        texture = mod.GetTexture("Gores/DeadlyVenom1");
                        break;
                    case 1:
                        texture = mod.GetTexture("Gores/DeadlyVenom2");
                        break;
                    case 2:
                        texture = mod.GetTexture("Gores/DeadlyVenom3");
                        break;
                    case 3:
                        texture = mod.GetTexture("Gores/DeadlyVenom4");
                        break;
                    case 4:
                        texture = mod.GetTexture("Gores/DeadlyVenom5");
                        break;
                    default:
                        texture = mod.GetTexture("Gores/DeadlyVenom1");
                        break;
                }

                spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        npc.position.X - Main.screenPosition.X + npc.width * 0.5f,
                        npc.position.Y - Main.screenPosition.Y - npc.height * 0.8f
                    ),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White,
                    0,
                    texture.Size() * 0.5f,
                    (npc.width / 128f) + 0.5f,
                    SpriteEffects.None,
                    0f
                );
            }

            if (pox)
            {
                Texture2D texture = mod.GetTexture("Gores/Pox");

                for (int i = 0; i < PoxStacks + 1; i++)
                {
                    spriteBatch.Draw
                    (
                        texture,
                        new Vector2
                        (
                            npc.position.X - Main.screenPosition.X + (npc.width * 1.5f) + 4,
                            npc.position.Y - Main.screenPosition.Y + (npc.height * 1.2f) * (i * 0.2f)
                        ),
                        new Rectangle(0, 0, texture.Width, texture.Height),
                        Color.White,
                        0,
                        texture.Size() * 0.5f,
                        0.4f,
                        SpriteEffects.None,
                        0f
                    );
                }
            }

            if (hemorrhage)
            {
                Texture2D texture = mod.GetTexture("Gores/Hemorrhage");

                for (int i = 0; i < HemorrhageStacks + 1; i++)
                {
                    spriteBatch.Draw
                    (
                        texture,
                        new Vector2
                        (
                            npc.position.X - Main.screenPosition.X + npc.width * 1.5f,
                            npc.position.Y - Main.screenPosition.Y + (npc.height * 1.2f) * (i * 0.2f)
                        ),
                        new Rectangle(0, 0, texture.Width, texture.Height),
                        Color.White,
                        0,
                        texture.Size() * 0.5f,
                        0.4f,
                        SpriteEffects.None,
                        0f
                    );
                }
            }

            if (cleaved)
            {
                Texture2D texture;

                switch (CleavedStacks)
                {
                    case 0:
                        texture = mod.GetTexture("Gores/Cleaved1");
                        break;
                    case 1:
                        texture = mod.GetTexture("Gores/Cleaved2");
                        break;
                    case 2:
                        texture = mod.GetTexture("Gores/Cleaved3");
                        break;
                    case 3:
                        texture = mod.GetTexture("Gores/Cleaved4");
                        break;
                    case 4:
                        texture = mod.GetTexture("Gores/Cleaved5");
                        break;
                    case 5:
                        texture = mod.GetTexture("Gores/Cleaved6");
                        break;
                    default:
                        texture = mod.GetTexture("Gores/Cleaved1");
                        break;
                }

                spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        npc.position.X - Main.screenPosition.X - (npc.width * 0.5f) - (System.Math.Min((System.Math.Min(npc.width, npc.height) / 28f) - 0.2f, 1.2f) / 2),
                        npc.position.Y - Main.screenPosition.Y
                    ),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White,
                    0,
                    texture.Size() * 0.5f,
                    System.Math.Min((System.Math.Min(npc.width, npc.height) / 28f) - 0.2f, 1.2f),
                    SpriteEffects.None,
                    0f
                );
            }

            if (grievousWounds)
            {
                Texture2D texture = mod.GetTexture("Gores/GrievousWounds");

                spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        npc.position.X - Main.screenPosition.X - (npc.width * 0.5f) - (System.Math.Min((System.Math.Min(npc.width, npc.height) / 28f) - 0.3f, 1.2f) / 2),
                        npc.position.Y - Main.screenPosition.Y + npc.height * 0.7f
                    ),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    Color.White,
                    0,
                    texture.Size() * 0.5f,
                    System.Math.Min((System.Math.Min(npc.width, npc.height) / 28f) - 0.1f, 1.2f),
                    SpriteEffects.None,
                    0f
                );
            }

            
            base.PostDraw(npc, spriteBatch, drawColor);
        }

        public void vesselStriked(int attacker, int damage, bool crit)
        {
            if (vessel && vesselTarget != -1)
            {
                Main.player[attacker].ApplyDamageToNPC(Main.npc[vesselTarget], (int)(damage * 0.5), 0, 0, crit);
            }
        }

        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (CausticWounds)
            {
                position = new Vector2(position.X, (npc.position.Y + npc.height * 1.25f) + (50 * ((npc.width / 128f) + 0.25f) ));
                return true;
            }

            return base.DrawHealthBar(npc, hbPosition, ref scale, ref position);
        }
    }
}
