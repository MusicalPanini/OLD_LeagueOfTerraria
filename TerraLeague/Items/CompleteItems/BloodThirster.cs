using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class BloodThirster : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Bloodthirster");
            Tooltip.SetDefault("6% increased ranged damage" +
                "\n+1 ranged life steal"/* +
                "\n30% decreased maximum life" +
                "\n30% increased damage taken"*/);
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
                new BloodShield()
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedDamage += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 1;// 0.06;
            //player.GetModPlayer<PLAYERGLOBAL>().healthModifier -= 0.3;
            //player.GetModPlayer<PLAYERGLOBAL>().damageTakenModifier += 0.3;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BFSword>(), 1);
            recipe.AddIngredient(ItemType<VampiricScepter>(), 1);
            recipe.AddIngredient(ItemType<LongSword>(), 1);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddIngredient(ItemID.Ruby, 3);
            recipe.AddIngredient(ItemID.Bone, 50);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
