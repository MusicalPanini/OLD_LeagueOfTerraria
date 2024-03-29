﻿using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    public class MistDevourer_Tail
        : WormClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mist Devourer");
        }
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.SeekerTail);
            npc.aiStyle = -1;
            minLength = 24;
            maxLength = 30;
            headType = NPCType<MistDevourer_Head>();
            bodyType = NPCType<MistDevourer_Body>();
            tailType = NPCType<MistDevourer_Tail>();
            speed = 10f;
            turnSpeed = 0.06f;

            tail = true;

            base.SetDefaults();
        }

        public override bool PreAI()
        {
            Lighting.AddLight(npc.Center, new Color(5, 245, 150).ToVector3());

            return base.PreAI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(0, 4) == 0)
                target.AddBuff(BuffID.Confused, 5*60);
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
            {

                int count = 0;
                while ((double)count < damage / (double)npc.lifeMax * 50.0)
                {
                    int num618 = Dust.NewDust(npc.position, npc.width, npc.height, 54, 0f, 0f, 50, default(Color), 1.5f);
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
                    int num620;
                    if (i > 10)
                        num620 = Dust.NewDust(npc.position, npc.width, npc.height, 54, 0f, 0f, 50, default(Color), 1.5f);
                    else
                        num620 = Dust.NewDust(npc.position, npc.width, npc.height, 16, 0f, 0f, 50, new Color(5, 245, 150), 1.5f);
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

    }
}
