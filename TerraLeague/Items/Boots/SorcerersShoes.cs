using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class SorcerersShoes : LeagueBoot
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sorcerer's Shoes");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.accessory = true;
            base.SetDefaults();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
        }

        public override void Tier1Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().magicArmorPen += 2;
            base.Tier1Update(player);
        }

        public override void Tier2Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().magicArmorPen += 4;
            base.Tier1Update(player);
        }

        public override void Tier3Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().magicArmorPen += 6;
            base.Tier1Update(player);
        }

        public override void Tier4Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().magicArmorPen += 8;
            base.Tier1Update(player);
        }

        public override void Tier5Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().magicArmorPen += 10;
            base.Tier1Update(player);
        }

        public override string Tier1Tip()
        {
            return "Increases magic armor penetration by 2";
        }

        public override string Tier2Tip()
        {
            return "Increases magic armor penetration by 4";
        }

        public override string Tier3Tip()
        {
            return "Increases magic armor penetration by 6";
        }

        public override string Tier4Tip()
        {
            return "Increases magic armor penetration by 8";
        }

        public override string Tier5Tip()
        {
            return "Increases magic armor penetration by 10";
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BootsOfSpeed>());
            recipe.AddIngredient(ItemID.MagicPowerPotion);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
