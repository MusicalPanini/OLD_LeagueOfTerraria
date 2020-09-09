using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class NegatronCloak : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Negatron Cloak");
            Tooltip.SetDefault("Resist increased by 4");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Green;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<NullMagic>(), 1);
            recipe.AddIngredient(ItemType<Petricite>(), 16);
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient(ItemID.TatteredCloth, 1);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
