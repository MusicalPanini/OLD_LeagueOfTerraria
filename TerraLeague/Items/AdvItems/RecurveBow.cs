using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class RecurveBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Recurve Bow");
            Tooltip.SetDefault("5 melee and ranged On Hit Damage" +
                "\n10% increased melee and ranged attack speed");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit += 5;
            player.GetModPlayer<PLAYERGLOBAL>().rangedOnHit += 5;
            player.meleeSpeed += 0.10f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed *= 1.10;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Dagger>(), 2);
            recipe.AddIngredient(ItemID.PalmWoodBow, 1);
            recipe.AddIngredient(ItemID.WhiteString, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
