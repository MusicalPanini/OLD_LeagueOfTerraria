using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BasicItems
{
    public class BlastingWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blasting Wand");
            Tooltip.SetDefault("3% increased magic and minion damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 3, 75, 0);
            item.rare = ItemRarityID.Green;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage += 0.03f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.03;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WandofSparking, 1);
            recipe.AddIngredient(ItemID.Bomb, 1);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe WandofSpark = new ModRecipe(mod);
            WandofSpark.AddIngredient(ItemID.Wood, 10);
            WandofSpark.AddIngredient(ItemID.FallenStar, 1);
            WandofSpark.AddIngredient(ItemID.Fireblossom, 1);
            WandofSpark.AddTile(TileID.Anvils);
            recipe.anyWood = true;
            WandofSpark.SetResult(ItemID.WandofSparking);
            WandofSpark.AddRecipe();
        }
    }
}
