using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DebugGun : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Debug Gun");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "You probably shouldn't have this";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "";
            else if (type == AbilityType.W)
                return "";
            else if (type == AbilityType.E)
                return "";
            else if (type == AbilityType.R)
                return "";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/Template";
            else if (type == AbilityType.W)
                return "AbilityImages/Template";
            else if (type == AbilityType.E)
                return "AbilityImages/Template";
            else if (type == AbilityType.R)
                return "AbilityImages/Template";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            //if (type == AbilityType.Q)
            //{
            //    return "Fire a laser of light after a delay";
            //}
            //else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
                return (int)(item.damage * 2f);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.RNG)
                    return 120;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 35;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " range damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 7;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            //if (type == AbilityType.Q)
            //{
            //    if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
            //    {
            //        Vector2 position = player.MountedCenter;
            //        Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 8f);
            //        int projType = ProjectileType<LightPistol_PiercingLight>();
            //        int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.RNG);
            //        int knockback = 0;

            //        Projectile proj = Projectile.NewProjectileDirect(position, Vector2.Zero, projType, damage, knockback, player.whoAmI);
            //        proj.rotation = velocity.ToRotation();

            //        SetAnimation(player, 30, 30, position + velocity);
            //        DoEfx(player, type);
            //        SetCooldowns(player, type);
            //    }
            //}
            //else
            {
                base.DoEffect(player, type);
            }
        }

        public override void SetDefaults()
        {
            item.damage = 12;
            item.ranged = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 52;
            item.height = 26;
            item.useAnimation = 16;
            item.reuseDelay = 20;
            item.useTime = 8;
            item.shootSpeed = 8f;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 6000;
            item.rare = ItemRarityID.Orange;
            item.scale = 0.9f;
            item.shoot = ProjectileType<LightPistol_Bullet>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 12);
            item.autoReuse = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            //if (type == AbilityType.Q)
            //    return true;
            return base.GetIfAbilityExists(type);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override void Efx(Player player, AbilityType type)
        {
            //if (type == AbilityType.Q)
            //{
            //    Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 13), player.Center);
            //    if (sound != null)
            //        sound.Pitch = 1f;
            //}
        }
    }
}
