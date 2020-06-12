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
    public class JeweledGauntlet : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jeweled Gauntlet");
            Tooltip.SetDefault("5% increased damage" +
                "\n8% increased crit chance");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 300000;
            item.rare = ItemRarityID.Lime;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.05f;
            player.rangedDamage += 0.05f;
            player.magicDamage += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.05;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BFSword>(), 1);
            recipe.AddIngredient(ItemType<LargeRod>(), 1);
            recipe.AddIngredient(ItemType<BrawlersGlove>(), 1);
            recipe.AddIngredient(ItemID.DestroyerEmblem, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 6);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddIngredient(ItemID.Deathweed, 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new ArcanePrecision();
        }

        public override Passive GetSecondaryPassive()
        {
            return null;
        }

        public override string GetStatText()
        {
            
                return "";
        }
    }
}
