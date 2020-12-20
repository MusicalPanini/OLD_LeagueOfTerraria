using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class MercuryTreads : LeagueBoot
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mercury Treads");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.accessory = true;
            base.SetDefaults();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
        }

        public override void Tier1Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 2;
            base.Tier1Update(player);
        }

        public override void Tier2Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 3;
            base.Tier1Update(player);
        }

        public override void Tier3Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            base.Tier1Update(player);
        }

        public override void Tier4Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;
            base.Tier1Update(player);
        }

        public override void Tier5Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 6;
            base.Tier1Update(player);
        }

        public override string Tier1Tip()
        {
            return "Increases resist by 2";
        }

        public override string Tier2Tip()
        {
            return "Increases resist by 3";
        }

        public override string Tier3Tip()
        {
            return "Increases resist by 4";
        }

        public override string Tier4Tip()
        {
            return "Increases resist by 5";
        }

        public override string Tier5Tip()
        {
            return "Increases resist by 6";
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BootsOfSpeed>());
            recipe.AddIngredient(ItemType<NullMagic>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
