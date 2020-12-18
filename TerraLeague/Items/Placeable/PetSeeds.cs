using Microsoft.Xna.Framework;
using TerraLeague.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Placeable
{
    class PetSeeds : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petrified Seeds");
        }

        public override void SetDefaults()
        {
            item.value = 75;
            item.rare = ItemRarityID.White;
            item.width = 16;
            item.height = 16;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = TileType<PetrifiedGrass>();
        }

        public override bool CanUseItem(Player player)
        {
            int X = Main.MouseWorld.ToTileCoordinates().X;
            int Y = Main.MouseWorld.ToTileCoordinates().Y;

            if (Main.tile[X, Y].type == TileID.Dirt && Main.tile[X, Y].active())
            {
                WorldGen.KillTile(X, Y, false, false, true);
                return base.CanUseItem(player);
            }

            return false;
        }
    }
}
