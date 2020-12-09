using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class TrinityForce : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Trinity Force");
            Tooltip.SetDefault("15% increased melee damage and speed" +
                "\n7% increased movement speed" +
                "\nIncreases maximum life by 20" +
                "\nIncreases maximum mana by 30" +
                "\nIncreases ability haste by 20");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.value = Item.buyPrice(0, 53, 33, 33);
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new Spellblade(2, 1),
                new Rage(5)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.15f;
            player.meleeSpeed += 0.15f;
            player.moveSpeed += 0.07f;
            player.statLifeMax2 += 20;
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Sheen>(), 1);
            recipe.AddIngredient(ItemType<Phage>(), 1);
            recipe.AddIngredient(ItemType<Stinger>(), 1);
            recipe.AddIngredient(ItemType<HarmonicBar>(), 3);
            recipe.AddIngredient(ItemID.ShroomiteBar, 3);
            recipe.AddIngredient(ItemID.SpectreBar, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
