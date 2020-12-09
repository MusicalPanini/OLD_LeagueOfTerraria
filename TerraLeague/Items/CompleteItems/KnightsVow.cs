using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class KnightsVow : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Knight's Vow");
            Tooltip.SetDefault("Increases health by 40" +
                "\nIncreases armor by 8" +
                 "\nIncreases ability haste by 10" +
                 "\nAbsorbs 25% of damage done to players on your team" +
                 "\nOnly active above 25% life");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 32;
            item.value = Item.buyPrice(0, 50, 0, 0);
            item.rare = ItemRarityID.Lime;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
               new TheVow(600)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 40;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 8;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.hasPaladinShield = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ChainVest>(), 1);
            recipe.AddIngredient(ItemType<Kindlegem>(), 1);
            recipe.AddIngredient(ItemID.PaladinsShield, 1);
            recipe.AddIngredient(ItemID.HallowedHelmet, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
