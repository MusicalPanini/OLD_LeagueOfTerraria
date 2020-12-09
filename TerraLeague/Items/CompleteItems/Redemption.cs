using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Redemption : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Redemption");
            Tooltip.SetDefault("Increases maximum life by 20" +
                "\nIncreases life regeneration by 2" +
                "\nIncreases mana regeneration by 50%" +
                "\n10% increased healing power" +
                "\nIncreases ability haste by 10");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 30, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;

            Active = new Intervention(50, 120);
            //Active = new Rejuvenate(50, 500, 120);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.5;
            player.lifeRegen += 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CrystallineBracer>(), 1);
            recipe.AddIngredient(ItemType<ForbiddenIdol>(), 1);
            recipe.AddIngredient(ItemID.LifeCrystal, 2);
            recipe.AddIngredient(ItemID.SoulofLight, 3);
            recipe.AddIngredient(ItemID.HealingPotion, 30);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                if (Active.cooldownCount > 0)
                    return (Active.cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Active.cooldownCount > 0 || !Active.currentlyActive)
                return true;
            else
                return false;
        }
    }
}
