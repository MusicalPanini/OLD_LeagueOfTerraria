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
            Tooltip.SetDefault("10% increased melee and ranged damage" +
                "\nIncreases armor by 4" +
                "\nNegates fall damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 250000;
            item.rare = ItemRarityID.Lime;
            item.accessory = true;
        }
        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot >= 3 && slot <= 8)
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot - 3] = (int)(240 * player.GetModPlayer<PLAYERGLOBAL>().Cdr * 60);
            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;
            player.meleeDamage += 0.1f;
            player.rangedDamage += 0.1f;
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

        public override Passive GetPrimaryPassive()
        {
            return new AngelsBlessing(240);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
            {
                if ((int)GetStatOnPlayer(Main.LocalPlayer) > 0 && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                    return ((int)GetStatOnPlayer(Main.LocalPlayer) / 60).ToString();
                else
                    return "";
            }
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
            {
                if ((int)GetStatOnPlayer(Main.LocalPlayer) > 0 || !Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
