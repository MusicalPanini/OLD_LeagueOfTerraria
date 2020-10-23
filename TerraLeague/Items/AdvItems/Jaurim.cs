using System.Collections.Generic;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Jaurim : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jaurim's Fist");
            Tooltip.SetDefault("5% increased melee damage" +
                "\nIncreases maximum life by 10");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new Strengthen(20, 1)
            };
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 10;
            player.meleeDamage += 0.05f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<LongSword>(), 1);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddIngredient(ItemID.MeteoriteBar, 10);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 10);
            recipe.AddIngredient(ItemID.Spike, 5);
            recipe.AddTile(TileID.Anvils);
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
