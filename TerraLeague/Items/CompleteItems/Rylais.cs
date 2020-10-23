using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Rylais : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rylai's Crystal Scepter");
            Tooltip.SetDefault("5% increased magic damage" +
                "\nIncreases health by 30");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 30, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Frosty(2)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage += 0.05f;
            player.statLifeMax2 += 30;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddIngredient(ItemType<AmpTome>(), 1);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 10);
            recipe.AddIngredient(ItemID.FrostStaff, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
