using TerraLeague.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Placeable
{
    class TargonGraniteBlock : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Targon Granite");
        }

        public override void SetDefaults()
        {
            item.value = 5000;
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = TileType<TargonGranite>();
        }
    }
}
