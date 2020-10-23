using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class InfinityEdge : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infinity Edge");
            Tooltip.SetDefault("10% increased ranged damage" +
                "\n10% increased ranged critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 80, 0, 0);
            item.rare = ItemRarityID.Red;
            item.accessory = true;

            Passives = new Passive[]
            {
                new CriticalEdge(25)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedDamage += 0.10f;
            player.rangedCrit += 10;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BFSword>(), 1);
            recipe.AddIngredient(ItemType<Pickaxe>(), 1);
            recipe.AddIngredient(ItemType<CloakofAgility>(), 1);
            recipe.AddIngredient(ItemID.DestroyerEmblem, 1);
            recipe.AddIngredient(ItemID.TrueExcalibur, 1);
            recipe.AddIngredient(ItemID.FragmentVortex, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            return base.CanEquipAccessory(player, slot);
        }
    }
}
