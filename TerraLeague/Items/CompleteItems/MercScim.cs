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
    public class MercScim : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mercurial Scimitar");
            Tooltip.SetDefault("5% increased ranged damage" +
                "\nIncreases resist by 5" +
                "\n+1 ranged life steal"/* +
                "\n10% decreased maximum life" +
                "\n10% increased damage taken"*/);
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;

            Active = new Quicksilver(6, 60);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedDamage += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 1;//0.04;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;
            //player.GetModPlayer<PLAYERGLOBAL>().healthModifier -= 0.1;
            //player.GetModPlayer<PLAYERGLOBAL>().damageTakenModifier -= 0.1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<QuicksilverSash>(), 1);
            recipe.AddIngredient(ItemType<Pickaxe>(), 1);
            recipe.AddIngredient(ItemType<VampiricScepter>(), 1);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 6);
            recipe.AddTile(TileID.MythrilAnvil);
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
