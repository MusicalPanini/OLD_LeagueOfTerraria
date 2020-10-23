using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Randuins : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Randuin's Omen");
            Tooltip.SetDefault("Increases maximum life by 30" +
                "\nIncreases armor by 8" +
                "\nPuts a shell around the owner when below 50% life that reduces damage");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 32;
            item.value = Item.buyPrice(0, 50, 0, 0);
            item.rare = ItemRarityID.Lime;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new PowersBane(100, 30),
                new ColdSteel(3, 350)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;
            player.statLifeMax2 += 30;

            if (player.GetModPlayer<PLAYERGLOBAL>().GetRealHeathWithoutShield() <= player.GetModPlayer<PLAYERGLOBAL>().GetRealHeathWithoutShield(true) / 2)
                player.AddBuff(BuffID.IceBarrier, 10);
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Wardens>(), 1);
            recipe.AddIngredient(ItemType<GiantsBelt>(), 1);
            recipe.AddIngredient(ItemID.FrozenTurtleShell, 1);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 10);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.Ectoplasm, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
