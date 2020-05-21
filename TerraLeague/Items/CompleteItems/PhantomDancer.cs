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
    public class PhantomDancer : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Dancer");
            Tooltip.SetDefault("20% increased melee speed" +
                "\n15% increased melee critical strike chance" +
                "\n10% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 32;
            item.value = 300000;
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeSpeed += 0.20f;
            player.meleeCrit += 15;
            player.moveSpeed += 0.1f;
            player.accRunSpeed *= 1.1f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Dagger>(), 2);
            recipe.AddIngredient(ItemType<Zeal>(), 1);
            recipe.AddIngredient(ItemID.Sickle, 1);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 50);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddIngredient(ItemID.SpectreBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Lifeline(90);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (slot != -1)
            {
                if (modPlayer.lifeLineCooldown > 0 && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
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
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (slot != -1)
            {
                if (modPlayer.lifeLineCooldown > 0 || !Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
