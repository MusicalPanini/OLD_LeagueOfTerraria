using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Tiles
{
    public class PetTreeSapling : ModTile
    {

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = true;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Marble, GetInstance<PetrifiedGrass>().Type};
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.DrawFlipHorizontal = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.addTile(Type);
            sapling = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Sapling");
            AddMapEntry(new Color(200, 200, 0), name);
            adjTiles = new int[] { TileID.Saplings };
        }

        public override void RandomUpdate(int i, int j)
        {
            if (WorldGen.genRand.Next(20) == 0)
            {
                bool isPlayerNear = WorldGen.PlayerLOS(i, j);
                bool success = WorldGen.GrowTree(i, j);
                if (success && isPlayerNear)
                {
                    WorldGen.TreeGrowFXCheck(i, j);
                }
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects)
        {
            TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Marble };

            if (i % 2 == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
        }
    }
}
