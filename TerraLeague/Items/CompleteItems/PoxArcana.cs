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
    public class PoxArcana : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pox Arcana");
            Tooltip.SetDefault("4% increased magic and minion damage" +
                "\nIncreases mana regeneration by 60%" +
                "\nAbility cooldown reduced by 10%");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 25, 0, 0);
            item.rare = ItemRarityID.Orange;
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
            player.magicDamage += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.04;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.6;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<FaerieCharm>(), 2);
            recipe.AddIngredient(ItemType<Codex>(), 1);
            recipe.AddIngredient(ItemType<AmpTome>(), 1);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 20);
            recipe.AddIngredient(ItemID.Stinger, 5);
            recipe.AddIngredient(ItemID.Bone, 10);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Active GetActive()
        {
            return new DiseaseHarvest(12, 5, 15, 60);
        }

        public override Passive GetPrimaryPassive()
        {
            return new Pox();
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
