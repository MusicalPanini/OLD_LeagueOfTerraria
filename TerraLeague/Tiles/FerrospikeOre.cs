using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Placeable;

namespace TerraLeague.Tiles
{
    public class FerrospikeOre : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileSolid[Type] = true; // Is the tile solid
            Main.tileMergeDirt[Type] = true; // Will tile merge with dirt?
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 420;
            Main.tileLighted[Type] = true; // ???
            soundType = 21;
            dustType = 96;
            drop = ItemType<Ferrospike>(); // What item drops after destorying the tile
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Ferrospike");
            AddMapEntry(new Color(25, 25, 50), name); // Colour of Tile on Map
            minPick = 65; // What power pick minimum is needed to mine this block.
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}