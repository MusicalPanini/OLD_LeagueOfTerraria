using TerraLeague.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Placeable
{
    class Ferrospike : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ferrospike Ore");
        }

        public override void SetDefaults()
        {
            item.value = Item.buyPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Green;
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = TileType<FerrospikeOre>();
        }
    }
}
