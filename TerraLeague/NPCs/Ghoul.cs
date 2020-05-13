using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class Ghoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ghoul");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BloodZombie];
        }
        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 3;
            npc.damage = 12;
            npc.defense = 0;
            npc.lifeMax = 20;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            aiType = NPCID.BloodZombie;
            animationType = NPCID.BloodZombie;
            npc.scale = 1f;
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(npc.Center, new Color(5, 245, 150).ToVector3());
            if (npc.localAI[3] == 0)
            {
                for (int j = 0; j < 50; j++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, 18, 40, 188);
                    dust.noGravity = true;
                    dust.scale = 2;
                }

                npc.localAI[3] = 1;
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

                Gore.NewGore(npc.Center, npc.velocity / 2, mod.GetGoreSlot("Gores/Ghoul_1"), 1f);
                Gore.NewGore(npc.Top, npc.velocity / 2, mod.GetGoreSlot("Gores/Ghoul_2"), 1f);
                Gore.NewGore(npc.Center, npc.velocity / 2, mod.GetGoreSlot("Gores/Ghoul_3"), 1f);
                Gore.NewGore(npc.Bottom, npc.velocity / 2, mod.GetGoreSlot("Gores/Ghoul_4"), 1f);
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            base.NPCLoot();
        }
    }
}
