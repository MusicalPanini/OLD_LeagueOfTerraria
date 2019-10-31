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
    public class TimekeepersFury : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Timekeeper's Wrath");
            Tooltip.SetDefault("5% increased magic and minion damage" +
                "\nIncreases maximum life by 20" +
                "\nIncreases maximum mana by 20" +
                "\n[c/007399:TOUCH OF DEATH's magic pen is based on IMPENDULUM]");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 180000;
            item.rare = 5;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;
            player.magicDamage += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.05;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Orb>(), 1);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddRecipeGroup("TerraLeague:EvilPartGroup", 10);
            recipe.AddIngredient(ItemID.SoulofLight, 6);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddIngredient(ItemID.SoulofMight, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Impendulum(1, 1);
        }

        public override Passive GetSecondaryPassive()
        {
            int[] stat = new int[]
            {
                4,5,6,7,8,9,10,11,12,11,10,9,8,7,6,5,4,3,2,1,0,1,2,3
            };

            return new TouchOfDeath(stat[(int)((1800 + Main.time) / 3600)]);
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
