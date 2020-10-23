using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.Accessories;

namespace TerraLeague.Items.CompleteItems
{
    public class HauntedRelic : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Haunted Relic");
            Tooltip.SetDefault("6% increased minion damage" +
                "\nIncreases health by 20" +
                "\nIncreases your max number of minions");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 60, 0, 0);
            item.rare = ItemRarityID.Lime;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Haunted()
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.06;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddIngredient(ItemType<CloakofAgility>(), 1);
            recipe.AddIngredient(ItemType<PossessedSkull>(), 1);
            recipe.AddIngredient(ItemID.EyeoftheGolem, 1);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 50);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
