using Microsoft.Xna.Framework;
using TerraLeague.Items;
using TerraLeague.Items.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class MountainSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mountain Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Harpy];
        }
        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 30;
            npc.aiStyle = 14;
            npc.damage = 12;
            npc.defense = 4;
            npc.lifeMax = 45;
            npc.value = 20;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            aiType = NPCID.GiantBat;
            animationType = NPCID.Harpy;
            npc.scale = 1f;
            banner = npc.type;
            bannerItem = ItemType<MountainSlimeBanner>();
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<PLAYERGLOBAL>().zoneTargon)
                return SpawnCondition.Overworld.Chance * 1f;
            return 0;
        }

        public override bool PreAI()
        {
            //Lighting.AddLight(npc.Center, new Color(5, 245, 150).ToVector3());
            //if (npc.localAI[3] == 0)
            //{
            //    for (int j = 0; j < 50; j++)
            //    {
            //        Dust dust = Dust.NewDustDirect(npc.position, 18, 40, 188);
            //        dust.noGravity = true;
            //        dust.scale = 2;
            //    }

            //    npc.localAI[3] = 1;
            //}
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
                int num262 = 0;
                while ((double)num262 < damage / (double)npc.lifeMax * 100.0)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)hitDirection, -1f, npc.alpha, Color.RosyBrown, 1f);
                    int num5 = num262;
                    num262 = num5 + 1;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)hitDirection, -1f, npc.alpha, Color.RosyBrown, 1f);
                }
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.Hitbox, ItemID.Gel, Main.rand.Next(1, 3));
            if (Main.rand.NextFloat() < 0.0001)
                Item.NewItem(npc.Hitbox, ItemID.SlimeStaff);
            base.NPCLoot();
        }
    }
}
