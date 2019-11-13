using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

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
            npc.lifeMax = 500;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCHit52;
            animationType = NPCID.CursedSkull;
            npc.value = 500;
            npc.knockBackResist = 0.3f;
            npc.scale = 1f;
            npc.buffImmune[20] = true;
            npc.buffImmune[24] = true;
            npc.buffImmune[39] = true;
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

            return base.PreAI();
        }

        public override bool CheckDead()
        {
            if (base.CheckDead())
            {
                for (int i = 0; i < 3; i++)
                {
                    NPC.NewNPC((int)npc.position.X + (i * 22), (int)npc.Center.Y, NPCType<Mistling>());
                }
            }

            return base.CheckDead();
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
                    int num618 = Dust.NewDust(npc.position, npc.width, npc.height, 16, 0f, 0f, 50, new Color(5, 245, 150), 1.5f);
                    Dust dust = Main.dust[num618];
                    dust.velocity *= 2f;
                    Main.dust[num618].noGravity = true;
                    count++;
                }
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    int num620 = Dust.NewDust(npc.position, npc.width, npc.height, 16, 0f, 0f, 50, new Color(5, 245, 150), 1.5f);
                    Dust dust = Main.dust[num620];
                    dust.velocity *= 2f;
                    Main.dust[num620].noGravity = true;
                }
                int num621 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y - 10f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_1"), npc.scale);
                Gore gore = Main.gore[num621];
                gore.velocity *= 0.3f;
                num621 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)(npc.height / 2) - 15f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_2"), npc.scale);
                gore = Main.gore[num621];
                gore.velocity *= 0.3f;
                num621 = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (float)npc.height - 20f), new Vector2((float)hitDirection, 0f), mod.GetGoreSlot("Gores/MistPuff_3"), npc.scale);
                gore = Main.gore[num621];
                gore.velocity *= 0.3f;
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            base.NPCLoot();
        }
    }
}
