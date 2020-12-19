using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class LightCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Cannon");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Deals an additional " + TerraLeague.CreateScalingTooltip(DamageType.RNG, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().RNG, 100) + " damage";
        }

        public override void SetDefaults()
        {
            item.damage = 100;
            item.ranged = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 108;
            item.height = 28;
            item.channel = true;
            item.useAnimation = 60;
            item.useTime = 60;
            item.shootSpeed = 10f;
            item.noMelee = true;
            item.knockBack = 7;
            item.value = 160000;
            item.rare = ItemRarityID.Pink;
            item.shoot = ProjectileType<LightCannon_BeamControl>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 13);

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new PiercingDarkness(this));
            abilityItem.ChampQuote = "I don't carry this to compromise";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, type, damage + player.GetModPlayer<PLAYERGLOBAL>().RNG, knockBack, player.whoAmI);
            proj.rotation = new Vector2(speedX, speedY).ToRotation();

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 100);
            recipe.AddIngredient(ItemID.HallowedBar, 16);
            recipe.AddIngredient(ItemID.Marble, 100);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16, 4);
        }
    }
}
