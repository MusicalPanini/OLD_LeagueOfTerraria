using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class ShadowArtilery : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow Artilery");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Antlion];
        }
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 24;
            npc.aiStyle = -1;
            npc.damage = 10;
            npc.defense = 4;
            npc.lifeMax = 60;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.knockBackResist = 0f;
            npc.behindTiles = true;
            animationType = NPCID.Antlion;
            aiType = NPCID.Antlion;
            npc.value = 100;
            npc.scale = 1f;
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && spawnInfo.player.ZoneDesert && Main.hardMode)
                return SpawnCondition.OverworldNightMonster.Chance;
            else if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && NPC.downedMechBossAny)
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            return 0;
        }

        public override bool PreAI()
        {
            Lighting.AddLight(npc.Center, new Color(5, 245, 150).ToVector3());

            return base.PreAI();
        }

        public override void AI()
        {
            {
                npc.TargetClosest(true);
                float num274 = 12f;
                Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float playerCenterXdist = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - npcCenter.X;
                float playerPositionYdist = Main.player[npc.target].position.Y - npcCenter.Y;
                float absoDist = (float)Math.Sqrt((double)(playerCenterXdist * playerCenterXdist + playerPositionYdist * playerPositionYdist));
                absoDist = num274 / absoDist;
                playerCenterXdist *= absoDist;
                playerPositionYdist *= absoDist;
                if (npc.directionY < 0)
                {
                    npc.rotation = (float)(Math.Atan2((double)playerPositionYdist, (double)playerCenterXdist) + 1.57);
                    if ((double)npc.rotation < -0.6)
                    {
                        npc.rotation = -0.6f;
                    }
                    else if ((double)npc.rotation > 0.6)
                    {
                        npc.rotation = 0.6f;
                    }
                    if (npc.velocity.X != 0f)
                    {
                        npc.velocity.X *= 0.9f;
                        if ((double)npc.velocity.X > -0.1 || (double)npc.velocity.X < 0.1)
                        {
                            npc.netUpdate = true;
                            npc.velocity.X = 0f;
                        }
                    }
                }
                if (npc.ai[0] > 0f)
                {
                    if (npc.ai[0] == 200f)
                    {
                        Main.PlaySound(SoundID.Item5, npc.position);
                    }
                    npc.ai[0] -= 1f;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[0] == 0f)
                {
                    npc.ai[0] = 200f;
                    for (int i = 0; i < 3; i++)
                    {
                        int damage = 16;
                        int type = ProjectileType<Projectiles.ShadowArtillery_LiquidShadow>();
                        int num280 = Projectile.NewProjectile(npcCenter, new Vector2(0,-16).RotatedBy(npc.rotation + MathHelper.Pi * (-1 + i) / 12f), type, damage, 0f, Main.myPlayer, 0f, 0f);
                        Main.projectile[num280].ai[0] = 2f;
                        Main.projectile[num280].timeLeft = 300;
                        Main.projectile[num280].friendly = false;
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num280, 0f, 0f, 0f, 0, 0, 0);
                        npc.netUpdate = true;
                    }

                }
                try
                {
                    int num281 = (int)npc.position.X / 16;
                    int num282 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
                    int num283 = (int)(npc.position.X + (float)npc.width) / 16;
                    int num284 = (int)(npc.position.Y + (float)npc.height) / 16;
                    if (Main.tile[num281, num284] == null)
                    {
                        Tile[,] tile15 = Main.tile;
                        int num285 = num281;
                        int num286 = num284;
                        Tile tile16 = new Tile();
                        tile15[num285, num286] = tile16;
                    }
                    if (Main.tile[num282, num284] == null)
                    {
                        Tile[,] tile17 = Main.tile;
                        int num287 = num281;
                        int num288 = num284;
                        Tile tile18 = new Tile();
                        tile17[num287, num288] = tile18;
                    }
                    if (Main.tile[num283, num284] == null)
                    {
                        Tile[,] tile19 = Main.tile;
                        int num289 = num281;
                        int num290 = num284;
                        Tile tile20 = new Tile();
                        tile19[num289, num290] = tile20;
                    }
                    if (Main.tile[num281, num284].nactive() && Main.tileSolid[Main.tile[num281, num284].type] ||
                        Main.tile[num282, num284].nactive() && Main.tileSolid[Main.tile[num282, num284].type] ||
                        Main.tile[num283, num284].nactive() && Main.tileSolid[Main.tile[num283, num284].type])
                    {
                        npc.noGravity = true;
                        npc.noTileCollide = true;
                        npc.velocity.Y = -0.2f;
                    }
                    else
                    {
                        npc.noGravity = false;
                        npc.noTileCollide = false;
                    }
                }
                catch
                {
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

                Gore gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y - 10f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_1"), npc.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 15f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_2"), npc.scale);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)npc.height - 20f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_3"), npc.scale);
                gore.velocity *= 0.3f;
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.width, npc.height, ItemType<DamnedSoul>(), 1);

            if (Main.rand.Next(0, 3) == 0)
            {
                int item = Item.NewItem(npc.position, npc.width, npc.height, ItemID.AntlionMandible, Main.rand.Next(1,3));
                Main.item[item].color = new Color(100, 100, 100);
            }

            base.NPCLoot();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/ShadowArtilery_Body");

            spriteBatch.Draw(texture, new Vector2(npc.Center.X - Main.screenPosition.X, npc.position.Y - Main.screenPosition.Y + (float)npc.height + 10f), new Rectangle(0, 0, texture.Width, texture.Height), drawColor, (0f - npc.rotation) * 0.3f, new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2)), 1f, SpriteEffects.None, 0f);

            base.PostDraw(spriteBatch, drawColor);
        }
    }
}
