using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class StarfireSpellblades : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire Spellblade");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Gains attack speed and damage each half second in combat" +
                "\nAfter 6 seconds, the sword will ascend and fire waves of starfire" +
                "\nThe wave deals " + (int)(item.damage * 0.75) + " + " + TerraLeague.CreateScalingTooltip(DamageType.MEL, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().MEL, 30) + " + " + TerraLeague.CreateScalingTooltip(DamageType.SUM, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().SUM, 50) + " melee damage";
        }

        public override void SetDefaults()
        {
            item.damage = 55;
            item.width = 56;
            item.height = 56;       
            item.melee = true;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = 200000;
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new DivineJudgement(this));
            abilityItem.ChampQuote = "As evil grows, so shall I";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult = 1 + (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks * 0.2f);

            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

        public override void UpdateInventory(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks >= 6)
            {
                byte prefix = item.prefix;
                item.SetDefaults(ItemType<StarfireSpellbladesAscended>());
                item.prefix = prefix;
            }

            base.UpdateInventory(player);
        }

        public override float MeleeSpeedMultiplier(Player player)
        {
            return base.MeleeSpeedMultiplier(player) + (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks * 0.05f);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 2);
            recipe.AddIngredient(ItemID.SoulofLight, 20);
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.AddIngredient(ItemType<FragmentOfTheAspect>(), 1);
            recipe.AddIngredient(ItemType<CelestialBar>(), 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
