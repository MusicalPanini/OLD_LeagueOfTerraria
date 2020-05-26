using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class BloomPendant : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloom Pendant");
            Tooltip.SetDefault("Increases mana regeneration by 1" +
                "\nWhile below 50% life increase mana regeneration by 3" +
                "\nIncreases length of invincibility after taking damage");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 32;
            item.rare = ItemRarityID.Pink;
            item.value = 100000;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.manaRegen += 1;
            player.longInvince = true;
            if (modPlayer.GetRealHeathWithoutShield(true)/2 > modPlayer.GetRealHeathWithoutShield())
            {
                modPlayer.manaRegen += 3;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrossNecklace, 1);
            recipe.AddIngredient(ItemType<Nightbloom>(), 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
