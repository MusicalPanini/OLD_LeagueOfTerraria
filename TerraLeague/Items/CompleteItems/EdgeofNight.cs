using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class EdgeofNight : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Edge of Night");
            Tooltip.SetDefault("5% increased melee damage" +
                "\nIncreases maximum life by 20" +
                "\nIncreases melee armor penetration by 7");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 25, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;

            Active = new NightsVeil(7, 120, 75);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.05f;
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().meleeArmorPen += 7;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Pickaxe>(), 1);
            recipe.AddIngredient(ItemType<SerratedDirk>(), 1);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddRecipeGroup("TerraLeague:DemonPartGroup", 1);
            recipe.AddIngredient(ItemID.JungleSpores, 1);
            recipe.AddIngredient(ItemID.Bone, 1);
            recipe.AddIngredient(ItemID.Hellstone, 1);
            recipe.AddTile(TileID.DemonAltar);
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
