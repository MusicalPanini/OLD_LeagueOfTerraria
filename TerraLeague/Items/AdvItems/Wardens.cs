using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Wardens : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warden's Mail");
            Tooltip.SetDefault("Armor increased by 4");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new ColdSteel(2, 250)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ClothArmor>(), 2);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 6);
            recipe.AddRecipeGroup("TerraLeague:IronGroup", 8);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 4);
            recipe.AddIngredient(ItemID.Obsidian, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
