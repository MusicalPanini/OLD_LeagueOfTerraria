using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class FrozenMallet : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen Mallet");
            Tooltip.SetDefault("6% increased melee and ranged damage" +
                "\nIncreases health by 40");
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
                new Icy(2)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.06f;
            player.rangedDamage += 0.06f;
            player.statLifeMax2 += 40;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Jaurim>(), 1);
            recipe.AddIngredient(ItemType<GiantsBelt>(), 1);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 10);
            recipe.AddIngredient(ItemID.Pwnhammer, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
