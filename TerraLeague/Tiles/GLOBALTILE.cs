using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;
namespace TerraLeague.Tiles
{
    public class GLOBALTILE : GlobalTile
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
    }
}
