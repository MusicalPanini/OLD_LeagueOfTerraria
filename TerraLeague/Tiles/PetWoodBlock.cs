using Microsoft.Xna.Framework;
using TerraLeague.Items.PetrifiedWood;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Tiles
{
    class PetWoodTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][TileID.WoodBlock] = true;
            Main.tileMerge[TileID.WoodBlock][Type] = true;

            Main.tileMerge[Type][TileID.Shadewood] = true;
            Main.tileMerge[TileID.Shadewood][Type] = true;

            Main.tileMerge[Type][TileID.Ebonwood] = true;
            Main.tileMerge[TileID.Ebonwood][Type] = true;

            Main.tileMerge[Type][TileID.Pearlwood] = true;
            Main.tileMerge[TileID.Pearlwood][Type] = true;

            Main.tileMerge[Type][TileID.PalmWood] = true;
            Main.tileMerge[TileID.PalmWood][Type] = true;

            Main.tileMerge[Type][TileID.SpookyWood] = true;
            Main.tileMerge[TileID.SpookyWood][Type] = true;

            Main.tileMerge[Type][TileID.Grass] = true;
            Main.tileMerge[TileID.Grass][Type] = true;

            Main.tileMerge[Type][TileID.CorruptGrass] = true;
            Main.tileMerge[TileID.CorruptGrass][Type] = true;

            Main.tileMerge[Type][TileID.FleshGrass] = true;
            Main.tileMerge[TileID.FleshGrass][Type] = true;

            Main.tileMerge[Type][TileID.JungleGrass] = true;
            Main.tileMerge[TileID.JungleGrass][Type] = true;

            Main.tileMerge[Type][TileID.HallowedGrass] = true;
            Main.tileMerge[TileID.HallowedGrass][Type] = true;

            Main.tileMerge[Type][TileID.HallowedGrass] = true;
            Main.tileMerge[TileID.HallowedGrass][Type] = true;

            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileMerge[TileID.Stone][Type] = true;

            Main.tileMerge[Type][TileID.Marble] = true;
            Main.tileMerge[TileID.Marble][Type] = true;

            Main.tileMerge[Type][TileID.MarbleBlock] = true;
            Main.tileMerge[TileID.MarbleBlock][Type] = true;

            Main.tileMerge[Type][TileID.SnowBlock] = true;
            Main.tileMerge[TileID.SnowBlock][Type] = true;

            Main.tileLighted[Type] = true; 

            dustType = 51;
            drop = ItemType<PetWood>(); 
            AddMapEntry(new Color(170, 170, 170)); 
            minPick = 0; 
        }
    }
}
