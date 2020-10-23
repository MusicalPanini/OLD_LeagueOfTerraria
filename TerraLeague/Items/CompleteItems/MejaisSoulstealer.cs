using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.StartingItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class MejaisSoulstealer : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mejai's Soulstealer");
            Tooltip.SetDefault("Increases maximum mana by 30");
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
                new Dread(25, 6, 1.5)
            };
        }
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 30;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DarkSeal>(), 1);
            recipe.AddIngredient(ItemID.SoulofNight, 20);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(ItemID.MagicPowerPotion, 5);
            recipe.AddTile(TileID.CrystalBall);
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
