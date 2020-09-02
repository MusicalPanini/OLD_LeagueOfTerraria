using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Crescendum : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crescendum");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Uses 1% Crescendum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nThrow up to " + Main.LocalPlayer.maxMinions + " + 5 returning chakrams";
        }

        public override string GetQuote()
        {
            return "An orbit of blades";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Sentry";
            else if (type == AbilityType.W)
                return "Phase";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/Sentry";
            else if (type == AbilityType.W)
                return "AbilityImages/Phase";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Create a sentry that fires " + Main.LocalPlayer.maxMinions + " + 5 returning chakrams";
            }
            else if (type == AbilityType.W)
            {
                return "Swap weapon to Calibrum";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
                return (int)(item.damage / 3);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.SUM)
                    return 100;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 40;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " x " + player.maxTurrets + " (Max Turrets) + " + GetScalingTooltip(player, type, DamageType.SUM) + " minion damage"
                    + "\nUses 10% Severum Ammo";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 14;
            if (type == AbilityType.W)
                return 1;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            return false;
        }

        public override bool CanCurrentlyBeCast(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (player.GetModPlayer<PLAYERGLOBAL>().crescendumAmmo < 10)
                {
                    return false;
                }
            }
            return base.CanCurrentlyBeCast(player, type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.GetModPlayer<PLAYERGLOBAL>().crescendumAmmo -= 10;
                    int projType = ProjectileType<Crescendum_Sentry>();
                    int damage = (GetAbilityBaseDamage(player, type) * player.maxTurrets) + GetAbilityScalingDamage(player, type, DamageType.RNG);
                    int knockback = 2;

                    player.FindSentryRestingSpot(projType, out int xPos, out int yPos, out int yDis);
                    Projectile.NewProjectile((float)xPos, (float)(yPos - yDis) - 3, 0f, 0f, projType, damage, knockback, player.whoAmI, -1);

                    //DoEfx(player, type);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type))
                {
                    item.SetDefaults(ItemType<Calibrum>());

                    CombatText.NewText(player.Hitbox, new Color(141, 252, 245), "CALIBRUM");

                    DoEfx(player, type);
                    SetCooldowns(player, type);
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
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
            return true;
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

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            if (type == AbilityType.W)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 13), player.Center);
                if (sound != null)
                    sound.Pitch = 1f;
                TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 13, 1f);
            }
        }
    }
}
