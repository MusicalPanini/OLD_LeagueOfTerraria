using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Bork : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade of the Ruined King");
            Tooltip.SetDefault("5% increased melee and ranged damage" +
                "\n12% increased melee and ranged attack speed" +
                "\n+1 melee and ranged life steal"/* +
                "\n12% decreased maximum life" +
                "\n12% increased damage taken"*/);
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 50, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;

            Active = new Damnation(250, 60);
            Passives = new Passive[]
            {
                 new SoulTaint(2, 20, 50)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.05f;
            player.rangedDamage += 0.05f;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 1;//0.03;
            player.GetModPlayer<PLAYERGLOBAL>().lifeStealRange += 1;//0.03;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.12f;
            player.meleeSpeed += 0.12f;
            //player.GetModPlayer<PLAYERGLOBAL>().healthModifier -= 0.12;
            //player.GetModPlayer<PLAYERGLOBAL>().damageTakenModifier += 0.12;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Cutlass>(), 1);
            recipe.AddIngredient(ItemType<RecurveBow>(), 1);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 50);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
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
