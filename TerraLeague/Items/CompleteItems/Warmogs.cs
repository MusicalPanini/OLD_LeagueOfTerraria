using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Warmogs : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warmog's Armor");
            Tooltip.SetDefault("Increases maximum life by 50" +
                "\nIncreases life regeneration by 50%" +
                "\nAbility cooldown reduced by 10%"); 
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 350000;
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 50;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<GiantsBelt>(), 1);
            recipe.AddIngredient(ItemType<Kindlegem>(), 1);
            recipe.AddIngredient(ItemType<CrystallineBracer>(), 1);
            recipe.AddIngredient(ItemID.BeetleScaleMail, 1);
            recipe.AddIngredient(ItemID.LifeFruit, 3);
            recipe.AddIngredient(ItemID.Vine, 2);
            recipe.AddIngredient(ItemID.JungleSpores, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new WarmogsHeart();
        }
    }
}
