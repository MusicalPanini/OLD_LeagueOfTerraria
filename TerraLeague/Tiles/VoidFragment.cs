using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Tiles
{
    public class VoidFragment : ModTile
    {
        bool pulse = false;
        float bLast = 0.3f;
        public override void SetDefaults()
        {
            soundType = SoundID.Tink;

            Main.tileSolid[Type] = true; 
            Main.tileMergeDirt[Type] = true;

            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true; 
            
            dustType = 65;
            drop = ItemType<Items.VoidFragment>(); 
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Void Matter");
            AddMapEntry(new Color(255, 0, 255), name); 
            minPick = 65;
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