using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Chalice : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chalice of Harmony");
            Tooltip.SetDefault("Increases resist by 4" +
                "\nIncreases mana regeneration by 30%");
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
                new Harmony(1, 40)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<FaerieCharm>(), 2);
            recipe.AddIngredient(ItemType<NullMagic>(), 1);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 8);
            recipe.AddIngredient(ItemType<ManaBar>(), 4);
            recipe.AddIngredient(ItemType<Petricite>(), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
