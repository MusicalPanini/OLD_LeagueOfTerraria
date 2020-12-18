using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerraLeague.Items.Placeable;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Tiles
{
    public class TargonGranite : ModTile
    {
        bool pulse = false;
        float bLast = 0.3f;
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true; 
            Main.tileLighted[Type] = true; 
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;

            soundType = SoundID.Tink;
            dustType = 172;
            drop = ItemType<TargonGraniteBlock>(); 
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Celstial Silicate");
            AddMapEntry(new Color(0, 200, 255), name); 
            minPick = 100; 
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0;
            g = 0;
            b = bLast;

            if (pulse)
            {
                b += 0.000005f;
            }
            else
            {
                b -= 0.000005f;
            }

            if (b <= 0.1)
                pulse = true;
            else if (b >= 0.2)
                pulse = false;
            bLast = b;
        }
    }
}