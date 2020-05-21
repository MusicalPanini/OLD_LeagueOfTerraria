using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.PetrifiedWood
{
    public class PetSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petrified Wood Sword");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.damage = 9;
            item.melee = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5f;
            item.value = 100;
            item.rare = ItemRarityID.White;
            item.UseSound = SoundID.Item1;
            base.SetDefaults();
        }

        public override bool UseItem(Player player)
        {
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<PetWood>(), 7);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
            base.AddRecipes();
        }

    }

}
