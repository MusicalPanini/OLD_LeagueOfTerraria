using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class EternalFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eternal Flame");
            Tooltip.SetDefault("Emit small flames to light your way");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.damage = 0;
            item.width = 22;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Green;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 45);
            item.buffType = BuffType<Buffs.CandlePet>();
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600);
            }
        }
    }

    public class EternalFlameGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            float rnd = Main.rand.NextFloat();
            if (NPCType<TheUndying_1>() == npc.type|| NPCType<TheUndying_2>() == npc.type || NPCType<TheUndying_Archer>() == npc.type || NPCType<TheUndying_Necromancer>() == npc.type || NPCType<Mistwraith>() == npc.type || NPCType<UnleashedSpirit>() == npc.type)
                if (rnd <= 0.0067f || (Main.expertMode && rnd <= 0.0133f))
                    Item.NewItem(npc.getRect(), ItemType<EternalFlame>());

            base.NPCLoot(npc);
        }
    }
}
