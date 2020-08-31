using TerraLeague.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Ammo
{
    public class VoidArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Arrow");
            Tooltip.SetDefault("Gains 25% damage on hit");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.shootSpeed = 3f;
            item.shoot = ProjectileType<Arrow_VoidArrow>();
            item.damage = 8;
            item.width = 10;
            item.height = 28;
            item.maxStack = 999;
            item.consumable = true;
            item.ammo = AmmoID.Arrow;
            item.knockBack = 1f;
            item.value = 40;
            item.rare = ItemRarityID.Blue;
            item.ranged = true;
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidFragment>(), 10);
            recipe.AddIngredient(ItemID.WoodenArrow, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();
        }
    }
}
