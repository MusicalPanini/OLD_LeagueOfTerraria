using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class VoidStaff : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Staff");
            Tooltip.SetDefault("15% increased magic damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 80, 0, 0);
            item.rare = ItemRarityID.Red;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Dissolve(40)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage += 0.15f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddIngredient(ItemType<AmpTome>(), 1);
            recipe.AddIngredient(ItemID.StaffofRegrowth, 1);
            recipe.AddIngredient(ItemType<VoidBar>(), 24);
            recipe.AddIngredient(ItemID.FragmentNebula, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
