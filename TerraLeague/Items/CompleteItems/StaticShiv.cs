using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class StaticShiv : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Statikk Shiv");
            Tooltip.SetDefault("12% increased melee and ranged attack speed" +
                "\n6% increased melee and ranged critical strike chance" +
                "\n5% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 30, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Energized(35, 25),
                new Discharge()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeCrit += 6;
            player.rangedCrit += 6;
            player.meleeSpeed += 0.15f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.12;
            player.moveSpeed += 0.05f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Zeal>(), 1);
            recipe.AddIngredient(ItemType<KircheisShard>(), 1);
            recipe.AddIngredient(ItemID.IronBroadsword, 1);
            recipe.AddIngredient(ItemID.NimbusRod, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemType<Zeal>(), 1);
            recipe2.AddIngredient(ItemType<KircheisShard>(), 1);
            recipe2.AddIngredient(ItemID.LeadBroadsword, 1);
            recipe2.AddIngredient(ItemID.NimbusRod, 1);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString() + "%";
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            return !Passives[0].currentlyActive;
        }
    }
}
