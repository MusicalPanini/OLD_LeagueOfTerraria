using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
namespace TerraLeague.Tiles
{
    public class TerraLeagueTILEGLOBAL : GlobalTile
    {

        public override void SetDefaults()
        {
            AddModTree(TileID.Marble, new PetTree());

            TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.Marble };

            base.SetDefaults();
        }
        public override int SaplingGrowthType(int type, ref int style)
        {
            if (type == TileID.Marble)
            {
                style = 0;
                return TileType<PetTreeSapling>();
            }
            else
            {
                return base.SaplingGrowthType(type, ref style);

            }
        }

        public override bool CanPlace(int i, int j, int type)
        {
            if (Main.tile[i, j].wall == WallType<Walls.TargonStoneWall_Arena>() && !TerraLeagueWORLDGLOBAL.TargonArenaDefeated)
                return false;
            return base.CanPlace(i, j, type);
        }
    }
}
