using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Youmuus : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Youmuu's Ghostblade");
            Tooltip.SetDefault("8% increased melee and ranged damage" +
                "\n8% increased movement speed" +
                "\nAbility cooldown reduced by 10%" +
                "\nIncreases melee armor penetration by 15");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 150000;
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot >= 3 && slot <= 8)
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot - 3] = (int)(60 * player.GetModPlayer<PLAYERGLOBAL>().Cdr * 60);

            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.08f;
            player.rangedDamage += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().meleeArmorPen += 7;
            player.moveSpeed += 0.08f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SerratedDirk>(), 1);
            recipe.AddIngredient(ItemType<Warhammer>(), 1);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 8);
            recipe.AddRecipeGroup("TerraLeague:DemonGroup", 5);
            recipe.AddIngredient(ItemID.SoulofLight, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Active GetActive()
        {
            return new PoltergeistsAscension(10, 60);
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
