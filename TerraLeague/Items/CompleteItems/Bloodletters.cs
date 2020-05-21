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
    public class Bloodletters : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloodletter's Veil");
            Tooltip.SetDefault("8% increased magic and minion damage" +
                "\nIncreases maximum life by 20");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 80000;
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot >= 3 && slot <= 8)
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot - 3] = (int)(75 * player.GetModPlayer<PLAYERGLOBAL>().Cdr * 60);
            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.06f;
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().meleeArmorPen += 7;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Orb>(), 1);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddIngredient(ItemType<BrassBar>(), 12);
            recipe.AddIngredient(ItemID.Bone, 10);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Active GetActive()
        {
            return new NightsVeil(7, 120, 75);
        }

        public override Passive GetPrimaryPassive()
        {
            return new TouchOfDeath(10);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
            {
                if ((int)GetStatOnPlayer(Main.LocalPlayer) > 0 && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ActivesAreActive[slot])
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
                if ((int)GetStatOnPlayer(Main.LocalPlayer) > 0 || !Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ActivesAreActive[slot])
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
