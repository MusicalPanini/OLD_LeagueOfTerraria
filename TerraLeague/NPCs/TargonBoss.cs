using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using TerraLeague.Buffs;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    [AutoloadBossHead]
    public class TargonBoss : ModNPC
    {
        public static Color PanthColor { get { return new Color(255, 0, 0); } }
        public static Color MorgColor { get { return new Color(132, 0, 193); } }
        public static Color KayleColor { get { return new Color(255, 228, 96); } }
        public static Color LeonaColor { get { return new Color(255, 109, 0); } }
        public static Color DianaColor { get { return new Color(255, 255, 255); } }
        public static Color TaricColor { get { return new Color(0, 42, 255); } }
        public static Color ZoeColor { get { return new Color(246, 0, 255); } }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Celestial Gate Keeper");
            Main.npcFrameCount[npc.type] = 7;
        }
        public override void SetDefaults()
        {
            npc.width = 128;
            npc.height = 128;
            npc.damage = 20;
            npc.defense = 10;
            npc.lifeMax = 5500;
            npc.HitSound = new Terraria.Audio.LegacySoundStyle(3, 5);
            npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 7);
            npc.scale = 1f;
            npc.noGravity = true;
            npc.boss = true;
            npc.knockBackResist = 0;
            npc.netAlways = true;
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            if (WORLDGLOBAL.TargonCenterX != 0)
            {
                npc.ai[2] += 1f;
                if (npc.ai[2] > 360)
                {
                    npc.ai[2] -= 360;
                    npc.netUpdate = true;
                }
                npc.Center = new Vector2(WORLDGLOBAL.TargonCenterX * 16, (float)(Main.worldSurface + 50) * 16);
                npc.position.Y += 16 * (float)System.Math.Sin(MathHelper.ToRadians(npc.ai[2]));

            }

            if (NPC.CountNPCS(NPCType<TargonBoss_Gem>()) > 0)
            {
                if (Main.time % 2 == 0)
                    TerraLeague.DustBorderRing(128, npc.Center, 263, TaricColor, 2);
                npc.dontTakeDamage = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }

            if ((int)npc.ai[0] == 1 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.localAI[1] == 10)
                {
                    int npcAtk = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, NPCType<TargonBossAttack>(), npc.whoAmI);
                    Main.npc[npcAtk].ai[0] = npc.whoAmI;
                    Main.npc[npcAtk].ai[2] = 300;
                    npc.ai[0] = 1;
                    npc.ai[1] = 0;
                    npc.netUpdate = true;
                }

                npc.localAI[1]++;
            }
            else
            {
                npc.TargetClosest();
                if (!Main.player[npc.target].HasBuff(BuffType<Buffs.InTargonArena>()))
                {
                    npc.localAI[0]++;
                    if (npc.localAI[0] > 10)
                        npc.active = false;
                }
                else
                {
                    npc.ai[0] = 1;
                }
            }


            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                WORLDGLOBAL.targonBossActive = true;
            }
            return base.PreAI();
        }

        public override void AI()
        {
            int npcType;
            float starMultiplier;
            switch ((int)npc.ai[1])
            {
                case 1:
                    npcType = NPCType<Star_Panth>();
                    starMultiplier = 0.8f;
                    break;
                case 2:
                    npcType = NPCType<Star_Morg>();
                    starMultiplier = 1.25f;
                    break;
                case 3:
                    npcType = NPCType<Star_Kayle>();
                    starMultiplier = 1.5f;
                    break;
                case 4:
                    npcType = NPCType<Star_Leona>();
                    starMultiplier = 1.25f;
                    break;
                case 5:
                    npcType = NPCType<Star_Diana>();
                    starMultiplier = 1f;
                    break;
                case 6:
                    npcType = NPCType<Star_Taric>();
                    starMultiplier = 2.5f;
                    break;
                case 7:
                    npcType = NPCType<Star_Zoe>();
                    starMultiplier = 1;
                    break;
                default:
                    npcType = -1;
                    starMultiplier = 1f;
                    break;
            }

            Lighting.AddLight(npc.Center, GetAttackColor.ToVector3());

            if (npc.ai[3] <= 0 && npcType != -1)
            {
                int count = 1;

                if (Main.netMode == NetmodeID.Server)
                {
                    int numOfPlayers = 0;

                    for (int i = 0; i < 255; i++)
                    {
                        if (Main.player[i].active)
                        {
                            if (Main.player[i].HasBuff(BuffType<InTargonArena>()))
                            {
                                numOfPlayers++;
                            }
                        }
                    }
                    count = Main.expertMode ? numOfPlayers : 1;
                }

                for (int i = 0; i < count; i++)
                {
                    int X = Main.rand.NextBool() ? Main.rand.Next((int)npc.Center.X - 500, (int)npc.Center.X - 172) : Main.rand.Next((int)npc.Center.X + 172, (int)npc.Center.X + 500);
                    int Y = Main.rand.NextBool() ? Main.rand.Next((int)npc.Center.Y - 500, (int)npc.Center.Y - 172) : Main.rand.Next((int)npc.Center.Y + 172, (int)npc.Center.Y + 500);

                    NPC.NewNPC(X, Y, npcType);
                }

                if (Main.expertMode)
                    npc.ai[3] = (int)(30 + (int)((npc.life / (float)npc.lifeMax) * 120) * starMultiplier);
                else
                    npc.ai[3] = (int)(60 + (int)((npc.life / (float)npc.lifeMax) * 120) * starMultiplier);
            }

            if (npc.ai[3] > 0)
                npc.ai[3]--;

            if ((int)npc.ai[1] != 0)
                npc.frame.Y = (int)(npc.ai[1] - 1) * 130;
            else
                npc.frame.Y = 4 * 130;

            base.AI();
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
            {
                int count = 0;
                while ((double)count < damage / (double)npc.lifeMax * 50.0)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 263, 0f, 0f, 0, GetAttackColor, 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 263, 0f, 0f, 0, GetAttackColor, 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            NPC dropnpc = Main.npc.FirstOrDefault(x => x.type == NPCType<TargonSigil>());
            if (dropnpc != null)
            {
                int choice = Main.rand.Next(10);
                if (choice == 0)
                    Item.NewItem(dropnpc.getRect(), ItemType<Items.Placeable.TargonBossTrophy>());

                if (Main.expertMode)
                {
                    dropnpc.DropBossBags();
                }
                else
                {
                    choice = Main.rand.Next(10);
                    if (choice == 0)
                    {
                        Item.NewItem(dropnpc.getRect(), ItemType<Items.Placeable.TargonMonolith>());
                    }
                    Item.NewItem(dropnpc.getRect(), ItemType<Items.CelestialBar>(), Main.rand.Next(2, 8));
                }
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
            base.NPCLoot();
        }

        Color GetAttackColor
        {
            get
            {
                switch ((int)npc.ai[1])
                {
                    case 1:
                        return PanthColor;
                    case 2:
                        return MorgColor;
                    case 3:
                        return KayleColor;
                    case 4:
                        return LeonaColor;
                    case 5:
                        return DianaColor;
                    case 6:
                        return TaricColor;
                    case 7:
                        return ZoeColor;
                    default:
                        return DianaColor;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            spriteBatch.Draw
            (
                texture,
                npc.Center - Main.screenPosition,
                npc.frame,
                new Color(255, 255, 255, 255/*AltAlpha*/),
                MathHelper.ToRadians((float)Main.time),
                npc.frame.Size() * 0.5f,
                npc.scale,
                SpriteEffects.None,
                0f
            );

            spriteBatch.Draw
            (
                texture,
                npc.Center - Main.screenPosition,
                npc.frame,
                new Color(255, 255, 255, 255/*AltAlpha*/),
                MathHelper.ToRadians((float)Main.time) + MathHelper.PiOver4,
                npc.frame.Size() * 0.5f,
                npc.scale,
                SpriteEffects.None,
                0f
            );

            return base.PreDraw(spriteBatch, drawColor);
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            if (Main.netMode == NetmodeID.Server)
                npc.lifeMax = (int)((double)npc.lifeMax * 0.75 * (double)numPlayers);
            else
                npc.lifeMax = (int)((double)npc.lifeMax * 0.75);
            base.ScaleExpertStats(numPlayers, bossLifeScale);
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool CheckDead()
        {
            WORLDGLOBAL.TargonArenaDefeated = true;

            return base.CheckDead();
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            base.BossLoot(ref name, ref potionType);
        }
    }
}
