using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Redemption : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Redemption");
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
            item.value = 150000;
            item.rare = 4;
            item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot >= 3 && slot <= 8)
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot - 3] = (int)(120 * player.GetModPlayer<PLAYERGLOBAL>().Cdr * 60);

            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.statDefense -= 4;
            //player.GetModPlayer<PLAYERGLOBAL>().armor -= 4;
            //player.GetModPlayer<PLAYERGLOBAL>().resist -= 4;
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.statLifeMax2 += 20;
            //player.manaRegen = (int)(player.manaRegen * 1.5);
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.5;
            player.lifeRegen += 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CrystallineBracer>(), 1);
            recipe.AddIngredient(ItemType<ForbiddenIdol>(), 1);
            recipe.AddIngredient(ItemID.LifeCrystal, 2);
            recipe.AddIngredient(ItemID.SoulofLight, 3);
            recipe.AddIngredient(ItemID.HealingPotion, 30);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Active GetActive()
        {
            return new Rejuvenate(50, 500, 120);
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
