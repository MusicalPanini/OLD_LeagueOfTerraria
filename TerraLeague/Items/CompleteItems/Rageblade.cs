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
    public class Rageblade : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guinsoo's Rageblade");
            Tooltip.SetDefault("6% increased melee damage" +
                "\n6% increased ranged damage" +
                "\n6% increased minion damage" +
                "\n12% increased melee and ranged attack speed");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 180000;
            item.rare = 5;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.06f;
            player.rangedDamage += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.06;
            player.meleeSpeed += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.06;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<AmpTome>(), 1);
            recipe.AddIngredient(ItemType<RecurveBow>(), 1);
            recipe.AddIngredient(ItemType<Pickaxe>(), 1);
            recipe.AddIngredient(ItemID.FieryGreatsword, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new GuinsoosRage(3);
        }

        public override Passive GetSecondaryPassive()
        {
            return new Afterburn(15, 10, 10);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
                return ((int)GetStatOnPlayer(Main.LocalPlayer)).ToString();
            else
                return "";
        }
    }
}
