using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Gunblade : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Gunblade");
            Tooltip.SetDefault("6% increased melee, magic and minion damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 50, 0, 0);
            item.rare = ItemRarityID.Lime;
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
            player.magicDamage += 0.06f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.06;
            player.meleeDamage += 0.06f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Cutlass>(), 1);
            recipe.AddIngredient(ItemType<HextechRevolver>(), 1);
            recipe.AddIngredient(ItemType<HextechCore>(), 1);
            recipe.AddIngredient(ItemID.VenusMagnum);
            recipe.AddIngredient(ItemType<BrassBar>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Omniheal(2); // 4
        }

        public override Active GetActive()
        {
            return new LightningBolt(250, 50, 30);
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

