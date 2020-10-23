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
    public class Locket : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Locket of the Iron Solari");
            Tooltip.SetDefault("Increases armor by 4" +
                "\nIncreases resist by 6" +
                "\nGrants immunity to knockback and fire blocks" +
                "\nIncreases length of invincibility after taking damage");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 36;
            item.value = Item.buyPrice(0, 30, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;

            Active = new SolarisProtection(20, 500, 10, 120);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 6;
            player.noKnockback = true;
            player.buffImmune[BuffID.Burning] = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Aegis>(), 1);
            recipe.AddIngredient(ItemType<NullMagic>(), 1);
            recipe.AddIngredient(ItemID.CrossNecklace, 1);
            recipe.AddIngredient(ItemID.CobaltShield, 1);
            recipe.AddIngredient(ItemID.SunplateBlock, 20);
            recipe.AddIngredient(ItemID.LifeCrystal, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                if (Active.cooldownCount > 0)
                    return (Active.cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Active.cooldownCount > 0 || !Active.currentlyActive)
                return true;
            else
                return false;
        }
    }
}
