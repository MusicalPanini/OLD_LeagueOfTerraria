using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class HextechRevolver : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Revolver");
            Tooltip.SetDefault("5% increased magic and minion damage");
        }

        public override void SetDefaults()
        {
            item.width = 52;
            item.height = 20;
            item.value = 60000;
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.05;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<AmpTome>(), 2);
            recipe.AddIngredient(ItemID.Revolver, 1);
            recipe.AddIngredient(ItemID.SpaceGun, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new MagicBolt(20, 20, 60);
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
