using TerraLeague.Tiles;
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
            item.value = 3500;
            item.rare = 2;
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = TileType<FerrospikeOre>();
        }
    }
}
