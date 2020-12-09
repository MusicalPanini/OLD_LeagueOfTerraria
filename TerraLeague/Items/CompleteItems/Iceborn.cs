using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Iceborn : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iceborn Gauntlet");
            Tooltip.SetDefault("Increases armor by 7" +
                "\nIncreases maximum mana by 40" +
                "\nIncreases ability haste by 15" +
                "\nIncreases melee knockback" +
                "\nGrants immunity to knockback");
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
                new Spellblade(1.5),
                new IcyZone()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 7;
            player.statManaMax2 += 40;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;
            player.noKnockback = true;
            player.kbGlove = true;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Sheen>(), 1);
            recipe.AddIngredient(ItemType<GlacialShroud>(), 1);
            recipe.AddIngredient(ItemID.TitanGlove, 1);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 20);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
