using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class LordDoms : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lord Dominik's Regards");
            Tooltip.SetDefault("7% increased ranged damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 60, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;

            Passives = new Passive[]
            {
                new CustomItems.Passives.LastWhisper(30, false)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedDamage += 0.07f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<LastWhisper>(), 1);
            recipe.AddIngredient(ItemType<Pickaxe>(), 1);
            recipe.AddIngredient(ItemID.EyeoftheGolem, 1);
            recipe.AddIngredient(ItemID.ShroomiteBar, 8);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
