using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class BootsOfSwiftness : LeagueBoot
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boots of Swiftness");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 26;
            item.accessory = true;
            base.SetDefaults();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
        }

        public override void Tier1Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().swifties = true;
            base.Tier1Update(player);
        }

        public override void Tier2Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().swifties = true;
            base.Tier1Update(player);
        }

        public override void Tier3Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().swifties = true;
            base.Tier1Update(player);
        }

        public override void Tier4Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().swifties = true;
            base.Tier1Update(player);
        }

        public override void Tier5Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().swifties = true;
            base.Tier1Update(player);
        }

        public override string Tier1Tip()
        {
            return "Unremarkably increased sprint speed";
        }

        public override string Tier2Tip()
        {
            return "Slightly increased sprint speed";
        }

        public override string Tier3Tip()
        {
            return "Increased sprint speed";
        }

        public override string Tier4Tip()
        {
            return "Greatly increased sprint speed";
        }

        public override string Tier5Tip()
        {
            return "Insanely increased sprint speed";
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BootsOfSpeed>());
            recipe.AddIngredient(ItemID.SwiftnessPotion, 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
