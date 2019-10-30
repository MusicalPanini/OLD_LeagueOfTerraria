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
                "\nAbility cooldown reduced by 20%");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 32;
            item.value = 233333;
            item.rare = 8;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.15f;
            player.meleeSpeed += 0.15f;
            player.moveSpeed += 0.07f;
            player.statLifeMax2 += 20;
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.2;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Sheen>(), 1);
            recipe.AddIngredient(ItemType<Phage>(), 1);
            recipe.AddIngredient(ItemType<Stinger>(), 1);
            recipe.AddIngredient(ItemType<VoidBar>(), 3);
            recipe.AddIngredient(ItemID.ShroomiteBar, 3);
            recipe.AddIngredient(ItemID.SpectreBar, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Spellblade(2, 1);
        }

        public override Passive GetSecondaryPassive()
        {
            return new Rage(5);
        }
    }
}
