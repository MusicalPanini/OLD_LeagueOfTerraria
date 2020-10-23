using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Thornmail : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thornmail");
            Tooltip.SetDefault("Increases maximum life by 20" +
                "\nIncreases armor by 8");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new Thorns(),
                new ColdSteel(2, 350)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 8;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Bramble>(), 1);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddIngredient(ItemType<Wardens>(), 1);
            recipe.AddIngredient(ItemID.Spike, 10);
            recipe.AddIngredient(ItemID.TurtleScaleMail, 1);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
