using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Sheen : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sheen");
            Tooltip.SetDefault("Increases maximum mana by 30" +
                "\nAbility cooldown reduced by 10%");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 50000;
            item.rare = 3;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().spellblade = true;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SapphireCrystal>(), 1);
            recipe.AddIngredient(ItemID.SilverBroadsword, 1);
            recipe.AddIngredient(ItemType<ManaBar>(), 8);
            recipe.AddIngredient(ItemID.Sapphire, 3);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemType<SapphireCrystal>(), 1);
            recipe2.AddIngredient(ItemID.TungstenBroadsword, 1);
            recipe2.AddIngredient(ItemType<ManaBar>(), 8);
            recipe2.AddIngredient(ItemID.Sapphire, 3);
            recipe2.AddIngredient(ItemID.FallenStar, 5);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Spellblade(1.5);
        }
    }
}
