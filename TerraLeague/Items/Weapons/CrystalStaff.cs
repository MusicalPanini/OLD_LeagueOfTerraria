using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class CrystalStaff : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Staff");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
            Item.staff[item.type] = true;
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "You deny the darkness in your soul. You deny your power!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Baleful Strike";
            else if (type == AbilityType.W)
                return "Dark Matter";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/BalefulStrike";
            else if (type == AbilityType.W)
                return "AbilityImages/DarkMatter";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Fire a piercing bolt of magic";
            }
            else if (type == AbilityType.W)
            {
                return "Call a lump of dark matter to fall from the sky and explode on impact with the ground";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return (int)(2 * item.damage);
            else if (type == AbilityType.W)
                return (int)(3 * item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.MAG)
                    return 60;
            }
            else if (type == AbilityType.W)
            {
                if (dam == DamageType.MAG)
                    return 100;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 5;
            else if (type == AbilityType.W)
                return 6;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q )
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else if (type == AbilityType.W)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 7;
            else if (type == AbilityType.W)
                return 8;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 18f);
                    int projType = ProjectileID.DiamondBolt;
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 0;

                    DoEfx(player, type);
                    SetAnimation(player, item.useTime, item.useAnimation, position + velocity);
                    Projectile.NewProjectileDirect(position, velocity, projType, damage, knockback, player.whoAmI);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = new Vector2(Main.MouseWorld.X, player.position.Y - (Main.screenHeight / 2));
                    Vector2 velocity = new Vector2(0, 25);
                    int projType = ProjectileType<DarkMatter>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 10;

                    DoEfx(player, type);
                    SetAnimation(player, item.useTime, item.useAnimation, position);
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
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
            item.damage = 9;
            item.magic = true;
            item.noMelee = true;
            item.mana = 4;
            item.width = 16;
            item.height = 32;
            item.useTime = 37;
            item.useAnimation = 37;
            item.useStyle = 5;
            item.knockBack = 3.5f;
            item.value = 1000;
            item.rare = 1;
            item.shootSpeed = 6.5f;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = ProjectileID.SapphireBolt;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q || type == AbilityType.W)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            
            if (type == AbilityType.Q)
            {


                Main.PlaySound(new LegacySoundStyle(2, 8), player.Center);
            }
            else if (type == AbilityType.W)
            {
                Main.PlaySound(SoundID.Item20, player.Center);
            }
        }
    }
}
