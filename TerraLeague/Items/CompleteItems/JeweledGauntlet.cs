using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.BasicItems;

namespace TerraLeague.Items.CompleteItems
{
    public class JeweledGauntlet : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jeweled Gauntlet");
            Tooltip.SetDefault("8% increased damage" +
                "\n8% increased crit chance" +
                "\nIncreases maximum mana by 40" +
                "\nIncreases ability haste by 10");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.Lime;
            item.accessory = true;

            Passives = new Passive[]
            {
                new ArcanePrecision(),
                new Haste()
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.08f;
            player.rangedDamage += 0.08f;
            player.magicDamage += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.08;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            player.statManaMax2 += 40;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BFSword>(), 1);
            recipe.AddIngredient(ItemType<BrawlersGlove>(), 1);
            recipe.AddIngredient(ItemType<LostChapter>(), 1);
            recipe.AddIngredient(ItemID.DestroyerEmblem, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 6);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
