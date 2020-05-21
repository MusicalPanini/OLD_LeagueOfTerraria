using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.PetrifiedWood
{
    public class PetHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Petrified Wood Hammer");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.damage = 6;
            item.melee = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 29;
            item.useAnimation = 29;
            item.hammer = 25;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5.5f;
            item.value = 100;
            item.rare = ItemRarityID.White;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            base.SetDefaults();
        }

        public override bool UseItem(Player player)
        {
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<PetWood>(), 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
            base.AddRecipes();
        }
    }
}
