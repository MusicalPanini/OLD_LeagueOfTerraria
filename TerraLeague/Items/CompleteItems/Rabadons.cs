using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Rabadons : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rabadon's Deathcap");
            Tooltip.SetDefault("12% increased magic damage" +
                "\n8% increased magic critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 80, 0, 0);
            item.rare = ItemRarityID.Red;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new RawPower()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage *= 1.12f;
            player.magicCrit += 8;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<LargeRod>(), 2);
            recipe.AddIngredient(ItemID.RuneHat, 1);
            recipe.AddIngredient(ItemID.SorcererEmblem, 2);
            recipe.AddIngredient(ItemID.FragmentNebula, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
