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
    public class Zzrot : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zz'Rot Portal");
            Tooltip.SetDefault("Increases armor by 3" +
                "\nIncreases resist by 4" +
                "\nIncreases your max number of sentries" +
                "\nIncreases life regeneration by 2");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
            Active = new VoidCaller(20, 3, 15, 60);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeSpeed += 0.15f;
            player.maxTurrets += 1;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 6;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
            player.lifeRegen += 2;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<RaptorCloak>(), 1);
            recipe.AddIngredient(ItemType<NegatronCloak>(), 1);
            recipe.AddIngredient(ItemType<VoidFragment>(), 100);
            recipe.AddIngredient(ItemID.SoulofSight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                if (Active.cooldownCount > 0)
                    return (Active.cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Active.cooldownCount > 0 || !Active.currentlyActive)
                return true;
            else
                return false;
        }
    }
}
