using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Phage : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phage");
            Tooltip.SetDefault("4% increased melee and ranged damage" +
                "\nIncreases maximum life by 20");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new Rage(3)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
            player.meleeDamage += 0.04f;
            player.rangedDamage += 0.04f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddIngredient(ItemID.WoodenHammer, 1);
            recipe.AddRecipeGroup("TerraLeague:DemonGroup", 5);
            recipe.AddRecipeGroup("TerraLeague:DemonPartGroup", 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
