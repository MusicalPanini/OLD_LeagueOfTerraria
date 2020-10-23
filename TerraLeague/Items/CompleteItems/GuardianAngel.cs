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
    public class GuardianAngel : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guardian Angel");
            Tooltip.SetDefault("6% increased melee and ranged damage" +
                "\nIncreases armor by 4" +
                "\nNegates fall damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 50, 0, 0);
            item.rare = ItemRarityID.Lime;
            item.accessory = true;

            Passives = new Passive[]
            {
                new AngelsBlessing(240)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;
            player.meleeDamage += 0.06f;
            player.rangedDamage += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BFSword>(), 1);
            recipe.AddIngredient(ItemType<Stopwatch>(), 1);
            recipe.AddIngredient(ItemType<ChainVest>(), 1);
            recipe.AddIngredient(ItemID.AngelWings, 1);
            recipe.AddIngredient(ItemID.Excalibur, 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
            {
                if (Passives[0].cooldownCount > 0)
                    return (Passives[0].cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Passives[0].cooldownCount > 0 || !Passives[0].currentlyActive)
                return true;
            else
                return false;
        }
    }
}
