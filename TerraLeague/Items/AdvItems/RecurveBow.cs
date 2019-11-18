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
            Tooltip.SetDefault("10 melee and ranged On Hit Damage" +
                "\n18% increased melee and ranged attack speed");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 60000;
            item.rare = 3;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit += 10;
            player.GetModPlayer<PLAYERGLOBAL>().rangedOnHit += 10;
            player.meleeSpeed += 0.18f;
            player.GetModPlayer<PLAYERGLOBAL>().ConsumeAmmoChance *= 1.18;
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
