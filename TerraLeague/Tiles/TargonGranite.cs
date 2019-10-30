using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerraLeague.Items.Placeable;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Tiles
{
    public class TargonGranite : ModTile
    {
        bool pulse = false;
        float bLast = 0.3f;
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true; // Is the tile solid
            Main.tileMergeDirt[Type] = true; // Will tile merge with dirt?
            Main.tileLighted[Type] = true; // ???
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;

            soundType = 21;
            dustType = 172;
            drop = ItemType<TargonGraniteBlock>(); // What item drops after destorying the tile
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Targon Granite");
            AddMapEntry(new Color(0, 200, 255), name); // Colour of Tile on Map
            minPick = 100; // What power pick minimum is needed to mine this block.
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0;
            g = 0;
            b = bLast;

            if (pulse)
            {
                b += 0.00001f;
            }
            else
            {
                b -= 0.00001f;
            }

            if (b <= 0.2)
                pulse = true;
            else if (b >= 0.4)
                pulse = false;
            bLast = b;
        }
    }
}