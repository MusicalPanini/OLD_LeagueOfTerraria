using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using TerraLeague.Items;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Tiles
{
    public class DarksteelBarTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Darksteel Bar");
            AddMapEntry(new Color(25, 25, 50), name);
            dustType = 96;
        }

        public override bool Drop(int i, int j)
        {
            Tile t = Main.tile[i, j];
            int style = t.frameX / 18;
            if (style == 0) 
            {
                Item.NewItem(i * 16, j * 16, 16, 16, ItemType<DarksteelBar>());
            }
            return base.Drop(i, j);
        }
    }
}