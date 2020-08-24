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
            soundType = SoundID.Tink;

            Main.tileSolid[Type] = true;
            Main.tileMerge[TileID.Sandstone][Type] = true;
            Main.tileMerge[TileID.HardenedSand][Type] = true;

            Main.tileLighted[Type] = true; 
            Main.tileBlockLight[Type] = true; 
            
            dustType = 64;
            drop = ItemType<Sunstone>(); 
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Sunstone");
            AddMapEntry(new Color(251, 127, 0), name);
            minPick = 45;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            dustType = 64;
            r = 251/255f/5f;
            g = 127/255f/5f;
            b = 0;
        }
    }
}