using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Banner : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Banner of Command");
            Tooltip.SetDefault("Increases armor by 6" +
                "\nIncreases resist by 4" +
                "\nIncreases your max number of minions" +
                "\nIncreases your max number of sentries" +
                "\nIncreases life regeneration by 2");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 32;
            item.value = Item.buyPrice(0, 50, 0, 0);
            item.rare = ItemRarityID.Lime;
            item.accessory = true;

            Active = new Rally(3, 20, 100);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;
            player.maxTurrets += 1;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.lifeRegen += 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Aegis>(), 1);
            recipe.AddIngredient(ItemType<RaptorCloak>(), 1);
            recipe.AddIngredient(ItemID.PygmyNecklace);
            recipe.AddIngredient(ItemType<BrassBar>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
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

