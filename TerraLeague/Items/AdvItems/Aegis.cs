using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Aegis : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aegis of the Legion");
            Tooltip.SetDefault("Increases armor by 3" +
                "\nIncreases resist by 3" +
                "\nGrants immunity to fire blocks");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 3;
            player.fireWalk = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<NullMagic>(), 1);
            recipe.AddIngredient(ItemType<ClothArmor>(), 1);
            recipe.AddIngredient(ItemID.ObsidianSkull, 1);
            recipe.AddIngredient(ItemID.SunplateBlock, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
