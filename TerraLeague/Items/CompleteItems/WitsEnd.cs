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
    public class WitsEnd : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wit's End");
            Tooltip.SetDefault("24 melee On hit Damage" +
                "\n15% increased melee speed" +
                "\nIncreases resist by 4" +
                "\nIncreases your max number of minions");
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
                new Absorption()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeSpeed += 0.15f;
            player.maxMinions += 1;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit += 24;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<RecurveBow>(), 1);
            recipe.AddIngredient(ItemType<NegatronCloak>(), 1);
            recipe.AddIngredient(ItemType<Dagger>(), 1);
            recipe.AddIngredient(ItemID.Cutlass, 1);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 8);
            recipe.AddIngredient(ItemID.SoulofSight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return (Passives[0].passiveStat).ToString();
            else
                return "";
        }
    }
}
