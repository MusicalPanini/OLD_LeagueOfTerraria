using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Roa : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rod of Ages");
            Tooltip.SetDefault("3% increased magic and minion damage" +
                "\nIncreases maximum life by 20" +
                "\nIncreases maximum mana by 20");
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
                new Impendulum(4, 1),
                new Eternity()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;
            player.magicDamage += 0.03f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.03;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Catalyst>(), 1);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddIngredient(ItemType<VoidFragment>(), 100);
            recipe.AddRecipeGroup("TerraLeague:EvilPartGroup", 10);
            recipe.AddIngredient(ItemID.SoulofLight, 6);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddIngredient(ItemID.SoulofMight, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return (Impendulum.GetStat).ToString();
            else
                return "";
        }
    }
}
