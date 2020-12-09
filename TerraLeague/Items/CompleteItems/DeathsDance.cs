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
    public class DeathsDance : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death's Dance");
            Tooltip.SetDefault("7% increased melee and ranged damage" +
                "\nIncreases ability haste by 10");
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
                new Omniheal(2),
                new CauterizedWounds(30)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.07f;
            player.rangedDamage += 0.07f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Warhammer>(), 1);
            recipe.AddIngredient(ItemType<Pickaxe>(), 1);
            recipe.AddIngredient(ItemType<VampiricScepter>(), 1);
            recipe.AddIngredient(ItemType<Sunstone>(), 20);
            recipe.AddIngredient(ItemType<VoidBar>(), 6);
            recipe.AddIngredient(ItemID.Seedler);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Passives[1].currentlyActive)
                return ((int)Passives[1].passiveStat).ToString();
            else
                return "";
        }
    }
}

