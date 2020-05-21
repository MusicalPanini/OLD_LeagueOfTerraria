using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
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
            item.value = 180000;
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedDamage += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Executioners>(), 1);
            recipe.AddIngredient(ItemType<LastWhisper>(), 1);
            recipe.AddIngredient(ItemID.BlackLens, 1);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 8);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new CustomItems.Passives.LastWhisper(20, false);
        }

        public override Passive GetSecondaryPassive()
        {
            return new CustomItems.Passives.Executioner(3);
        }
    }
}
