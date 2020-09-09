using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class MoonflairSpellblade : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonflair Spellblade");
            Tooltip.SetDefault("4% increased magic and minion damage" +
                "\nIncreases armor by 5" +
                "\nIncreases resist by 4" +
                "\nImmunity to Slow and Chilled");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 20, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage += 0.04f;
            player.magicDamage += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 5;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Chilled] = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Seekers>(), 1);
            recipe.AddIngredient(ItemType<NegatronCloak>(), 1);
            recipe.AddIngredient(ItemType<CelestialBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

