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
    public class StarForgersCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Star Forger");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        string GetWeaponTooltip()
        {
            return "Become the center of the Universe!";
        }

        public override void SetDefaults()
        {
            item.damage = 60;
            item.summon = true;
            item.mana = 20;
            item.width = 40;
            item.height = 42;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 200000;
            item.rare = ItemRarityID.Yellow;
            item.UseSound = new LegacySoundStyle(2, 113);
            item.shoot = ProjectileType<StarForgersCore_ForgedStar>();

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.W, new CelestialExpansion(this));
            abilityItem.ChampQuote = "Now we're playing with star fire!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(BuffType<Buffs.CenterOfTheUniverse>(), 3);
            position = player.MountedCenter;
            Projectile.NewProjectileDirect(position, Vector2.Zero, type, damage, knockBack, player.whoAmI, player.ownedProjectileCounts[type] + 1);

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CelestialBar>(), 20);
            recipe.AddIngredient(ItemType<FragmentOfTheAspect>(), 1);
            recipe.AddIngredient(ItemID.FallenStar, 20);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

