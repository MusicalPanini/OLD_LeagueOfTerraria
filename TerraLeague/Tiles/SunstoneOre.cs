using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerraLeague.Items;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Tiles
{
    public class SunstoneOre : ModTile
    {
        public override void SetDefaults()
        {
            soundType = 21;

            Main.tileSolid[Type] = true; // Is the tile solid
            Main.tileMerge[TileID.Sandstone][Type] = true;
            Main.tileMerge[TileID.HardenedSand][Type] = true;

            Main.tileLighted[Type] = true; // ???
            Main.tileBlockLight[Type] = true; // Emits Light
            
            dustType = 65;
            drop = ItemType<Sunstone>(); // What item drops after destorying the tile
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Sunstone");
            AddMapEntry(new Color(251, 127, 0), name); // Colour of Tile on Map
            minPick = 45; // Silver Pick
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {

            r = 251/255f/5f;
            g = 127/255f/5f;
            b = 0;
        }
    }
}