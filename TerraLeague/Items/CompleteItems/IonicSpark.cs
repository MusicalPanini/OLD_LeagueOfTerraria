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
    public class IonicSpark : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ionic Spark");
            Tooltip.SetDefault("12% increased ranged attack speed" +
                "\nIncreases maximum life by 20");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 25, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Energized(10, 20),
                new Discharge()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.12;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<RecurveBow>(), 1);
            recipe.AddIngredient(ItemType<KircheisShard>(), 1);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddRecipeGroup("TerraLeague:IronGroup", 5);
            recipe.AddIngredient(ItemID.Wire, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString() + "%";
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            return !Passives[0].currentlyActive;
        }
    }
}
