using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Tiles
{
    public class VoidFragment : ModTile
    {
        bool pulse = false;
        float bLast = 0.3f;
        public override void SetDefaults()
        {
            soundType = 21;

            Main.tileSolid[Type] = true; // Is the tile solid
            Main.tileMergeDirt[Type] = true; // Will tile merge with dirt?

            Main.tileLighted[Type] = true; // ???
            Main.tileBlockLight[Type] = true; // Emits Light
            
            dustType = 65;
            drop = ItemType<Items.VoidFragment>(); // What item drops after destorying the tile
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Void Matter");
            AddMapEntry(new Color(255, 0, 255), name); // Colour of Tile on Map
            minPick = 65; // What power pick minimum is needed to mine this block.
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            dustType = 65;
            r = bLast;
            g = 0.0f;
            b = bLast;

            if (pulse)
            {
                b += 0.00003f;
            }
            else
            {
                b -= 0.00003f;
            }

            if (b <= 0.2)
                pulse = true;
            else if (b >= 0.4)
                pulse = false;
            bLast = b;
        }
    }
}