using Microsoft.Xna.Framework;
using TerraLeague.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Placeable
{
    class TargonMonolith : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Targon Monolith");
            Tooltip.SetDefault("'Bring the stars wherever you go'");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 40;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = TileType<Tiles.TargonMonolith>();
            item.value = 50000;
            item.rare = ItemRarityID.Orange;
        }
    }
}
