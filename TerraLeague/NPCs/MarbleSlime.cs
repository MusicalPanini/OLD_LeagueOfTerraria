using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items.Banners;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.NPCs
{
    public class MarbleSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Marble Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }
        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 24;
            npc.aiStyle = 1;
            npc.damage = 12;
            npc.defense = 9;
            npc.lifeMax = 55;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.05f;
            npc.value = 25f;
            aiType = NPCID.BlueSlime;
            animationType = NPCID.BlueSlime;
            base.SetDefaults();
            npc.scale = 1.3f;
            banner = npc.type;
            bannerItem = ModContent.ItemType<MarbleSlimeBanner>();
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(0, 12) == 0 && !Main.expertMode)
            {
                target.AddBuff(BuffID.Stoned, 60);
            }
            else if (Main.rand.Next(0, 6) == 0 && Main.expertMode)
            {
                target.AddBuff(BuffID.Stoned, 60);
            }
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 60; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 4, hitDirection, -2, 150, new Color(50, 50, 50), 1f);
            }

            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            int item = Item.NewItem(npc.position, npc.width, npc.height, ItemID.Gel, Main.rand.Next(1, 4));
            Main.item[item].color = new Color(255, 255, 255);
            base.NPCLoot();
        }
    }
}
