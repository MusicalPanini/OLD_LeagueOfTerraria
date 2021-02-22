using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Crescendum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crescendum");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "Uses 1% Crescendum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nThrow up to 5 + " + TerraLeague.CreateScalingTooltip(TerraLeague.MINIONMAXColor, "MINIONS", Main.LocalPlayer.maxMinions, 100) + " returning chakrams";
        }

        public override void SetDefaults()
        {
            item.damage = 100;
            item.summon = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 48;
            item.height = 48;
            item.useAnimation = 7;
            item.useTime = 7;
            item.shootSpeed = 16f;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = 310000 * 5;
            item.rare = ItemRarityID.Purple;
            item.shoot = ProjectileType<Crescendum_Proj>();
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noUseGraphic = true;

            
            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.Q, new Sentry(this));
            abilityItem.SetAbility(AbilityType.W, new Phase(this, LunariGunType.Cre));
            abilityItem.ChampQuote = "An orbit of blades";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<Crescendum_Proj>()] < player.maxMinions + 5 )
            {
                if (player.GetModPlayer<PLAYERGLOBAL>().crescendumAmmo < 1)
                {
                    if (Main.mouseLeftRelease)
                    {
                        TerraLeague.PlaySoundWithPitch(player.MountedCenter, 12, 0, -0.5f);
                        CombatText.NewText(player.Hitbox, Color.White, "NO AMMO");
                    }
                    return false;
                }
                return base.CanUseItem(player);
            }

            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.GetModPlayer<PLAYERGLOBAL>().crescendumAmmo -= 1;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }
    }
}
