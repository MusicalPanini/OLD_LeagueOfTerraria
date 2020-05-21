using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerraLeague.Tiles
{
    public class PetrifiedFlora : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = true;
            soundType = SoundID.Grass;
            dustType = 192;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                20
            };
            TileObjectData.newTile.DrawYOffset = -2;
            TileObjectData.newTile.Style = 0;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;

            for (int i = 0; i < 23; i++)
            {
                TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
                TileObjectData.addSubTile(TileObjectData.newSubTile.Style);
            }

            TileObjectData.addTile(Type);
        }

        public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return TileType<PetTreeSapling>();
        }
    }
}