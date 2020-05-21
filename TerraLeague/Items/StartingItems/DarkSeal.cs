using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.StartingItems
{
    public class DarkSeal : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Dark Seal");
            Tooltip.SetDefault("Increases maximum mana by 20");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 50000;
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BandofStarpower, 1);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 5);
            recipe.AddIngredient(ItemID.MeteoriteBar, 1);
            recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe StarPower = new ModRecipe(mod);
            StarPower.AddIngredient(ItemID.ManaCrystal, 1);
            recipe.AddRecipeGroup("TerraLeague:IronGroup", 10);
            StarPower.AddIngredient(ItemType<ManaBar>(), 5);
            StarPower.AddTile(TileID.Anvils);
            StarPower.SetResult(ItemID.BandofStarpower);
            StarPower.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Dread(10, 4, 1);

        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
                return ((int)GetStatOnPlayer(Main.LocalPlayer)).ToString();
            else
                return "";
        }
    }
}
