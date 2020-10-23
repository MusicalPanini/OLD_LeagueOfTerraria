using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class HauntingGuise : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Haunting Guise");
            Tooltip.SetDefault("5% increased minion damage" +
                "\nIncreases health by 20" +
                "\nIncreases your max number of minions by 1");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 25, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Madness(1)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.05;
            player.statLifeMax2 += 20;
            player.maxMinions += 1;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddIngredient(ItemType<AmpTome>(), 1);
            recipe.AddIngredient(ItemID.NecromanticScroll, 1);
            recipe.AddIngredient(ItemID.MimeMask, 1);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString();
            else
                return "";
        }
    }
}
