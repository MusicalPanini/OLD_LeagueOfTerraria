using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Cutlass : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bilgewater Cutlass");
            Tooltip.SetDefault("4% increased melee and ranged damage" +
                "\n+1 melee and ranged life steal" /*+
                "\n10% decreased maximum life" +
                "\n10% increased damage taken"*/);
        }
        
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;

            Active = new Damnation(100, 75);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.04f;
            player.rangedDamage += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 1;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VampiricScepter>(), 1);
            recipe.AddIngredient(ItemType<LongSword>(), 1);
            recipe.AddIngredient(ItemType<BrassBar>(), 6);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
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
