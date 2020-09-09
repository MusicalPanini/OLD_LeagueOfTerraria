using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Talisman : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Talisman of Ascension");
            Tooltip.SetDefault("Increases maximum life by 20" +
                "\nIncreases life regeneration by 2" +
                "\nIncreases mana regeneration by 50%" +
                "\n10% increased healing power" +
                "\nAbility cooldown reduced by 10%");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 25, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot >= 3 && slot <= 8)
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot - 3] = (int)(90 * player.GetModPlayer<PLAYERGLOBAL>().Cdr * 60);
            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.05f;
            player.lifeRegen += 2;
            player.statLifeMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.5;
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CrystallineBracer>(), 1);
            recipe.AddIngredient(ItemType<ForbiddenIdol>(), 1);
            recipe.AddIngredient(ItemType<Sunstone>(), 10);
            recipe.AddIngredient(ItemID.SunplateBlock, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Active GetActive()
        {
            return new FleetFoot(500, 4, 90);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
            {
                if ((int)GetStatOnPlayer(Main.LocalPlayer) > 0)
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
                if ((int)GetStatOnPlayer(Main.LocalPlayer) > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
