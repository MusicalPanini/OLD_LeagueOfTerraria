using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Zekes : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zeke's Convergence");
            Tooltip.SetDefault("Increases armor by 6" +
                "\nIncreases resist by 4" +
                "\nIncreases maximum mana by 20" +
                "\nAbility cooldown reduced by 10%" +
                "\nGrants immunity to knockback and fire blocks");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 180000;
            item.rare = ItemRarityID.Pink;
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
            player.GetModPlayer<PLAYERGLOBAL>().armor += 6;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.statManaMax2 += 20;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.noKnockback = true;
            player.fireWalk = true;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Aegis>(), 1);
            recipe.AddIngredient(ItemType<GlacialShroud>(), 1);
            recipe.AddIngredient(ItemID.LivingFireBlock, 10);
            recipe.AddIngredient(ItemType<TrueIceChunk>(), 10);
            recipe.AddIngredient(ItemType<HextechCore>(), 1);
            recipe.AddIngredient(ItemID.SoulofSight, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Active GetActive()
        {
            return new FrostfireCovenant(40, 10, 90);
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
