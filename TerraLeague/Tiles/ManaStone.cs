using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Tiles
{
    public class ManaStone : ModTile
    {
        bool pulse = false;
        float bLast = 0.3f;
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true; 
            Main.tileMerge[Type][TileID.Mud] = true;
            Main.tileMerge[TileID.Mud][Type] = true;

            Main.tileLighted[Type] = true; 
            Main.tileBlockLight[Type] = true; 
            
            dustType = 48;
            drop = ItemType<Items.Placeable.ManaStone>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Mana Infused Earth");
            AddMapEntry(new Color(0, 78, 181), name);
            minPick = 65; 
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Main.tileMerge[Type][TileID.JungleGrass] = false;

            Main.tileMerge[TileID.JungleGrass][Type] = false;

            r = 0.0f;
            g = 0.0f;
            b = bLast;

            if (pulse)
            {
                b += 0.00001f;
            }
            else
            {
                b -= 0.00001f;
            }

            if (b <= 0.05)
                pulse = true;
            else if (b >= 0.15)
                pulse = false;
            bLast = b;
        }
    }
}