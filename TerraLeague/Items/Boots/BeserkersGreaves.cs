using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class BeserkersGreaves : LeagueBoot
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beserker's Greaves");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 26;
            item.accessory = true;
            item.material = true;
            base.SetDefaults();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
        }

        public override void Tier1Update(Player player)
        {
            player.meleeSpeed += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.05;
            base.Tier1Update(player);
        }

        public override void Tier2Update(Player player)
        {
            player.meleeSpeed += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.08;
            base.Tier1Update(player);
        }

        public override void Tier3Update(Player player)
        {
            player.meleeSpeed += 0.1f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.1;
            base.Tier1Update(player);
        }

        public override void Tier4Update(Player player)
        {
            player.meleeSpeed += 0.12f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.12;
            base.Tier1Update(player);
        }

        public override void Tier5Update(Player player)
        {
            player.meleeSpeed += 0.15f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.15;
            base.Tier1Update(player);
        }

        public override string Tier1Tip()
        {
            return "5% increased melee and ranged attack speed";
        }

        public override string Tier2Tip()
        {
            return "8% increased melee and ranged attack speed";
        }

        public override string Tier3Tip()
        {
            return "10% increased melee and ranged attack speed";
        }

        public override string Tier4Tip()
        {
            return "12% increased melee and ranged attack speed";
        }

        public override string Tier5Tip()
        {
            return "15% increased melee and ranged attack speed";
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BootsOfSpeed>());
            recipe.AddIngredient(ItemType<Dagger>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
