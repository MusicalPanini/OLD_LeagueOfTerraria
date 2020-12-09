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
    public class Zephyr : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zephyr");
            Tooltip.SetDefault("4% increased melee and ranged damage" +
                "\n5% increased melee and ranged attack speed" +
                "\n10% increased movement speed" +
                "\nIncreases ability haste by 10" +
                "\nImmunity to Slow and Chilled");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 20, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;

            Passives = new Passive[]
            {
                new WindPower()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.04f;
            player.rangedDamage += 0.04f;
            player.moveSpeed += 0.1f;
            player.meleeSpeed += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.05;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;

            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Chilled] = true;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BFSword>(), 1);
            recipe.AddIngredient(ItemType<Stinger>(), 1);
            recipe.AddIngredient(ItemType<Dagger>(), 1);
            recipe.AddIngredient(ItemType<ManaBar>(), 12);
            recipe.AddIngredient(ItemID.Cloud, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            return "";
        }
    }
}
