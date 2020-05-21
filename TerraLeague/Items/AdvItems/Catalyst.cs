using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Catalyst : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Catalyst of Aeons");
            Tooltip.SetDefault("\nIncreases maximum life by 20" +
                "\nIncreases maximum mana by 20");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 120000;
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddIngredient(ItemType<SapphireCrystal>(), 1);
            recipe.AddIngredient(ItemID.MagicCuffs, 1);
            recipe.AddIngredient(ItemType<VoidFragment>(), 25);
            recipe.AddIngredient(ItemID.Amethyst, 5);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Eternity();
        }
    }
}
