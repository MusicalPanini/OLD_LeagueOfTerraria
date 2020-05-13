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
    public class Stormrazer : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stormrazer");
            Tooltip.SetDefault("6% increased melee and ranged damage" +
                "\n8% increased melee and ranged attack speed");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 80000;
            item.rare = 3;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.06f;
            player.rangedDamage += 0.06f;
            player.meleeSpeed += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.08;
            player.moveSpeed += 0.05f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BFSword>(), 1);
            recipe.AddIngredient(ItemType<LongSword>(), 1);
            recipe.AddIngredient(ItemType<KircheisShard>(), 1);
            recipe.AddIngredient(ItemID.Muramasa, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe Muramasa = new ModRecipe(mod);
            Muramasa.AddIngredient(ItemType<ManaBar>(), 18);
            Muramasa.AddIngredient(ItemID.Bone, 50);
            Muramasa.AddTile(TileID.Anvils);
            Muramasa.SetResult(ItemID.Muramasa);
            Muramasa.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Energized(15, 25);
        }

        public override Passive GetSecondaryPassive()
        {
            return new Storm();
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1 && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                return ((int)GetStatOnPlayer(Main.LocalPlayer)).ToString() + "%";
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);
            if (slot != -1)
            {
                if (!Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
