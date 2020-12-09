using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.BasicItems;

namespace TerraLeague.Items.CompleteItems
{
    public class Moonstone : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonstone");
            Tooltip.SetDefault("6% increased magic and minion damage" +
                "\nIncreases mana regeneration by 20%" +
                "\nIncreases maximum life by 20" +
                "\nIncreases ability haste by 20" +
                "\nReduces the cooldown of healing potions by 25%");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 60, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;

            Passives = new Passive[]
            {
                new StarlitGrace(12, 25, 4, 4)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.2;
            player.magicDamage += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.06;
            player.moveSpeed += 0.08f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Kindlegem>(), 1);
            recipe.AddIngredient(ItemType<Codex>(), 1);
            recipe.AddIngredient(ItemType<FaerieCharm>(), 1);
            recipe.AddIngredient(ItemType<FragmentOfTheAspect>(), 1);
            recipe.AddIngredient(ItemID.PhilosophersStone, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat / 60).ToString();
            else
                return "";
        }
    }
}
