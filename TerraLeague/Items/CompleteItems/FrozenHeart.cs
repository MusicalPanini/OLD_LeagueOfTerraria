using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class FrozenHeart : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen Heart");
            Tooltip.SetDefault("Increases mana by 40" +
                "\nIncreases armor by 10" +
                 "\nAbility cooldown reduced by 20%" +
                 "\nGrants immunity to knockback");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 32;
            item.value = 150000;
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 40;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 10;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.2;
            player.noKnockback = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Wardens>(), 1);
            recipe.AddIngredient(ItemType<GlacialShroud>(), 1);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new ColdSteel(3, 400);
        }
    }
}
