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
    public class TimekeepersFury : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Timekeeper's Wrath");
            Tooltip.SetDefault("3% increased magic and minion damage" +
                "\nIncreases maximum life by 20" +
                "\nIncreases maximum mana by 20" +
                "\n[c/007399:TOUCH OF DEATH's magic pen is based on IMPENDULUM]");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new Impendulum(2, 1.5),
                new TouchOfDeath(2)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;
            player.magicDamage += 0.03f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.03;

            Passives[1].passiveStat = Impendulum.GetStat;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Orb>(), 1);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddIngredient(ItemType<SapphireCrystal>(), 1);
            recipe.AddIngredient(ItemID.SoulofLight, 6);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddIngredient(ItemID.SoulofMight, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                return Impendulum.GetStat.ToString();
            }
            return "";
        }
    }
}
