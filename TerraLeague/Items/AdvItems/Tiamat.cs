using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Tiamat : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiamat");
            Tooltip.SetDefault("3% increased melee damage" +
                "\nIncreases life regeneration by 2");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Cleave(40, CleaveType.Basic)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.03f;
            player.lifeRegen += 2;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<LongSword>(), 2);
            recipe.AddIngredient(ItemType<RejuvBead>(), 1);
            recipe.AddIngredient(ItemID.Spear, 1);
            recipe.AddIngredient(ItemID.Spike, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
