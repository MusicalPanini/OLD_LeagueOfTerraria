using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class MortalReminder : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mortal Reminder");
            Tooltip.SetDefault("5% increased ranged damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;

            Passives = new Passive[]
            {
                new CustomItems.Passives.LastWhisper(20, false),
                new Executioner(3)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedDamage += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Executioners>(), 1);
            recipe.AddIngredient(ItemType<AdvItems.LastWhisper>(), 1);
            recipe.AddIngredient(ItemID.BlackLens, 1);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 8);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
