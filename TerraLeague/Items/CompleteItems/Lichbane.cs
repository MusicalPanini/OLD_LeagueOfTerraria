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
    public class Lichbane : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lich Bane");
            Tooltip.SetDefault("15% increased minion damage" +
                "\n7% increased movement speed" +
                "\nAbility cooldown reduced by 10%" +
                "\nIncreases maximum mana by 30" +
                "\nIncreases your max number of minions");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 80, 0, 0);
            item.rare = ItemRarityID.Red;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.15;
            player.moveSpeed += 0.07f;
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.maxMinions += 1;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Sheen>(), 1);
            recipe.AddIngredient(ItemType<AetherWisp>(), 1);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddIngredient(ItemID.FragmentStardust, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Spellblade(1.5);
        }

        public override Passive GetSecondaryPassive()
        {
            return new SummonedBlade(120);
        }
    }
}
