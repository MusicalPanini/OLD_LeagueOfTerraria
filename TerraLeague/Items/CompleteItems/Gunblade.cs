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

            Active = new LightningBolt(250, 50, 30);
            Passives = new Passive[]
            {
                new Omniheal(2)
            };
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
            recipe.AddIngredient(ItemType<HextechAlternator>(), 1);
            recipe.AddIngredient(ItemType<HextechCore>(), 1);
            recipe.AddIngredient(ItemID.VenusMagnum);
            recipe.AddIngredient(ItemType<BrassBar>(), 8);
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

