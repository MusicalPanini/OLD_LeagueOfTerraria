using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class GreaterSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.ItemNoGravity[item.type] = true;
            DisplayName.SetDefault("Greater Soul");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 32;
            item.height = 32;
            item.uniqueStack = false;
            item.rare = ItemRarityID.Orange;
            item.value = 100;
        }

        public override void PostUpdate()
        {
            ItemID.Sets.ItemIconPulse[item.type] = false;

            Lighting.AddLight(item.Center, Color.DarkSeaGreen.ToVector3() * 0.55f * Main.essScale);
        }
    }
}
