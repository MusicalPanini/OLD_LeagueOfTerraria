using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class StaffOfFlowingWater : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Staff of Flowing Water");
            Tooltip.SetDefault("8% increased magic and minion damage" +
                "\nIncreases mana regeneration by 20%" +
                "\n8% increased healing power");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Rapids()
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.08;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.2;
            player.magicDamage += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.08;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ForbiddenIdol>(), 1);
            recipe.AddIngredient(ItemType<BasicItems.BlastingWand>(), 1);
            recipe.AddIngredient(ItemType<ManaBar>(), 8);
            recipe.AddIngredient(ItemID.SoulofMight, 6);
            recipe.AddIngredient(ItemID.AquaScepter, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
