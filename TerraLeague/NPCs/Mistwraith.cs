using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Banners;

namespace TerraLeague.NPCs
{
    public class Mistwraith : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mistwraith");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.CursedSkull];
        }
        public override void SetDefaults()
        {
            npc.width = 44;
            npc.height = 44;
            npc.aiStyle = 10;
            npc.damage = 50;
            npc.defense = 20;
            npc.lifeMax = 1000;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCHit52;
            animationType = NPCID.CursedSkull;
            npc.value = 500;
            npc.knockBackResist = 0.3f;
            npc.npcSlots = 3;
            npc.scale = 1f;
            npc.buffImmune[20] = true;
            npc.buffImmune[24] = true;
            npc.buffImmune[39] = true;
            banner = npc.type;
            bannerItem = ItemType<MistwraithBanner>();
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist && Main.hardMode)
                return SpawnCondition.OverworldNightMonster.Chance * 0.1f;
            return 0;
        }

        public override bool PreAI()
        {
            Lighting.AddLight(npc.Center, new Color(5, 245, 150).ToVector3());
            for (int i = 0; i < 4; i++)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 0f, 0f, 100, new Color(67, 248, 175), Main.rand.Next(1,5));
                dust.alpha = 200;
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += npc.velocity * 0.1f;
                dust.position.X -= npc.velocity.X / 3f * (float)i;
                dust.position.Y -= npc.velocity.Y / 3f * (float)i;
            }


            return base.PreAI();
        }

        public override void AI()
        {
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
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 0f, 0f, 50, new Color(5, 245, 150), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 0f, 0f, 50, new Color(5, 245, 150), 1.5f);
                    dust.velocity *= 2f;
                    dust.noGravity = true;
                }
                Gore gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y - 10f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_1"), npc.scale * 1.5f);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 15f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_2"), npc.scale * 1.5f);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)npc.height - 20f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_3"), npc.scale * 1.5f);
                gore.velocity *= 0.3f;
                gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y - 10f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_1"), npc.scale);
                gore.velocity *= 1.2f;
                gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 15f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_2"), npc.scale);
                gore.velocity *= 1.2f;
                gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y + (float)npc.height - 20f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_3"), npc.scale);
                gore.velocity *= 1.2f;

                for (int i = 0; i < 6; i++)
                {
                    int mistling = NPC.NewNPC((int)npc.position.X + (i * 22), (int)npc.Center.Y, NPCType<Mistling>());
                    Main.npc[mistling].velocity = new Vector2(0,-6).RotatedBy((MathHelper.Pi * (-2.5 + i))/6f);
                }
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            base.NPCLoot();
        }
    }
}
