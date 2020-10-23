using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Morellos : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Morellonomicon");
            Tooltip.SetDefault("5% increased magic damage" +
                "\nIncreases health by 20");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;

            Passives = new Passive[]
            {
                new CursedStrike(),
                new TouchOfDeath(15)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage += 0.05f;
            player.statLifeMax2 += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Orb>(), 1);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 50);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
