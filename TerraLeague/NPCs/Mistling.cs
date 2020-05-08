using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class Mistling : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mistling");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.DungeonSpirit];
        }
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 24;
            npc.aiStyle = 2;
            npc.damage = 35;
            npc.defense = 10;
            npc.lifeMax = 300;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCHit52;
            npc.aiStyle = NPCID.DemonEye;
            animationType = NPCID.DungeonSpirit;
            npc.value = 100;
            npc.knockBackResist = 0.2f;
            npc.scale = 1f;
            npc.buffImmune[20] = true;
            npc.buffImmune[24] = true;
            npc.buffImmune[39] = true;
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(npc.Center, new Color(5, 245, 150).ToVector3());

            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 0f, 0f, 100, new Color(67, 248, 175), Main.rand.Next(1, 3));
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
            npc.rotation = npc.velocity.ToRotation() - MathHelper.PiOver2;
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

                Gore gore = Gore.NewGoreDirect(new Vector2(npc.position.X, npc.position.Y), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_1"), npc.scale);
                gore.velocity *= 0.3f;
            }
            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.width, npc.height, ItemType<DamnedSoul>(), 1);
            base.NPCLoot();
        }
    }
}
