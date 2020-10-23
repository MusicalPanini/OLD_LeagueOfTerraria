using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class LostChapter : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lost Chapter");
            Tooltip.SetDefault("3% increased magic and minion damage" +
                "\nIncreases maximum mana by 20");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Green;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new Haste()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage += 0.03f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.03;
            player.statManaMax2 += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<AmpTome>(), 2);
            recipe.AddIngredient(ItemType<SapphireCrystal>(), 1);
            recipe.AddIngredient(ItemID.Book, 1);
            recipe.AddIngredient(ItemID.Moonglow, 5);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
