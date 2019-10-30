using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Tiles
{
    public class Limestone : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true; // Is the tile solid
            Main.tileMergeDirt[Type] = true; // Will tile merge with dirt?
            Main.tileMerge[Type][TileID.Marble] = true;
            Main.tileMerge[TileID.Marble][Type] = true;

            Main.tileMerge[Type][TileID.MarbleBlock] = true;
            Main.tileMerge[TileID.MarbleBlock][Type] = true;

            Main.tileMerge[Type][TileType<PetWoodTile>()] = true;
            Main.tileMerge[TileType<PetWoodTile>()][Type] = true;

            Main.tileLighted[Type] = true; // ???
            Main.tileBlockLight[Type] = true; // Emits Light

            soundType = 21;
            dustType = 85;
            drop = ItemType<Items.Placeable.Limestone>(); // What item drops after destorying the tile
            AddMapEntry(new Color(255, 255, 200)); // Colour of Tile on Map
            minPick = 0; // What power pick minimum is needed to mine this block.
        }
    }
}