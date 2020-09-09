using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.BasicItems
{
    public class LargeRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Needlessly Large Rod");
            Tooltip.SetDefault("4% increased magic and minion damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Green;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.04;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddIngredient(ItemID.Bone, 5);
            recipe.AddIngredient(ItemType<ManaBar>(), 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.anyWood = true;
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
