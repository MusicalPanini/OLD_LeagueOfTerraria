using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class SpiritVisage : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Visage");
            Tooltip.SetDefault("Increases maximum life by 30" +
                "\nIncreases resist by 6" +
                "\nIncreases life regeneration by 2" +
                "\nIncreases ability haste by 10");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new SpiritualRestoration()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().spiritualRestur = true;
            player.statLifeMax2 += 30;
            player.lifeRegen += 2;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 6;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Kindlegem>(), 1);
            recipe.AddIngredient(ItemType<Cowl>(), 1);
            recipe.AddIngredient(ItemID.ChlorophytePlateMail, 1);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 50);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
