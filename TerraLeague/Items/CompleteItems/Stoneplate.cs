using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Stoneplate : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gargoyle's Stoneplate");
            Tooltip.SetDefault("Increases armor by 5" +
                "\nIncreases resist by 5");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;

            Active = new Metallicize(90);
            Passives = new Passive[]
            {
                new StoneSkin(5, 5)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 5;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ChainVest>(), 1);
            recipe.AddIngredient(ItemType<Stopwatch>(), 1);
            recipe.AddIngredient(ItemType<NegatronCloak>(), 1);
            recipe.AddIngredient(ItemType<Petricite>(), 20);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 5);
            recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 5);
            recipe.AddIngredient(ItemID.Marble, 50);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
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
