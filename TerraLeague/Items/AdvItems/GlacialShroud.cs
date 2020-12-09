using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class GlacialShroud : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Shroud");
            Tooltip.SetDefault("Increases maximum mana by 20" +
                "\nIncreases armor by 3" +
                "\nIncreases ability haste by 10" +
                "\nGrants immunity to knockback");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.statManaMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.noKnockback = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SapphireCrystal>(), 1);
            recipe.AddIngredient(ItemType<ClothArmor>(), 1);
            recipe.AddIngredient(ItemID.CobaltShield, 1);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 2);
            recipe.AddIngredient(ItemID.IceBlock, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe shield = new ModRecipe(mod);
            shield.AddRecipeGroup("TerraLeague:Tier1Bar", 10);
            shield.AddIngredient(ItemID.SoulofLight, 10);
            shield.AddTile(TileID.Anvils);
            shield.SetResult(ItemID.CobaltShield);
            shield.AddRecipe();
        }
    }
}
