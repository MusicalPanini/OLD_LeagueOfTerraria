using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Sunfire : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunfire Cape");
            Tooltip.SetDefault("Increases maximum life by 30" +
                "\nIncreases armor by 6" +
                "\nImmunity to Bleeding and Poisoned");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new Immolate(500, false)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;

            if (!hideVisual)
                player.GetModPlayer<PLAYERGLOBAL>().immolate = true;

            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ChainVest>(), 1);
            recipe.AddIngredient(ItemType<Bamis>(), 1);
            recipe.AddIngredient(ItemID.MedicatedBandage, 1);
            recipe.AddIngredient(ItemID.MoltenBreastplate, 1);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddIngredient(ItemType<Sunstone>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
