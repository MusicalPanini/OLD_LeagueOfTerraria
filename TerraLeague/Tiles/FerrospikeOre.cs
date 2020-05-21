using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Placeable;
using Terraria.ID;

namespace TerraLeague.Tiles
{
    public class FerrospikeOre : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true; 
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 420;
            Main.tileLighted[Type] = true;
            soundType = SoundID.Tink;
            dustType = 96;
            drop = ItemType<Ferrospike>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Ferrospike");
            AddMapEntry(new Color(25, 25, 50), name);
            minPick = 65; 
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}