using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Infernum : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infernum");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Uses 5% Infernum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nYour attacks will create a splash of flame";
        }

        public override string GetQuote()
        {
            return "Cosmic flame will fill the night";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Duskwave";
            else if (type == AbilityType.W)
                return "Phase";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/Duskwave";
            else if (type == AbilityType.W)
                return "AbilityImages/Phase";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Create a ring of flame around you that creates secondary rings after hitting an enemy";
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
                return (int)(item.damage * 1.5f);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.RNG)
                    return 60;
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
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " range damage"
                    + "\nUses 10% Infernum Ammo";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 11;
            else if (type == AbilityType.W)
                return 1;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        public override bool CanCurrentlyBeCast(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (player.GetModPlayer<PLAYERGLOBAL>().infernumAmmo < 10)
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
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 8f);
                    int projType = ProjectileType<Infernum_Flame>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.RNG);
                    int knockback = 0;

                    for (int i = 0; i < 16; i++)
                    {
                        Projectile.NewProjectileDirect(position, new Vector2(16, 0).RotatedBy(MathHelper.TwoPi/16 * i) * 0.75f, projType, damage, knockback, player.whoAmI, 0, 1);
                    }

                    SetAnimation(player, 30, 30, position + velocity);
                    DoEfx(player, type);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type))
                {
                    item.SetDefaults(ItemType<Crescendum>());

                    CombatText.NewText(player.Hitbox, new Color(255, 255, 255), "CRESCENDUM");

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
            item.ranged = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 96;
            item.height = 36;
            item.useAnimation = 26;
            item.useTime = 26;
            item.shootSpeed = 12f;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = 310000 * 5;
            item.rare = ItemRarityID.Purple;
            item.scale = 0.8f;
            item.shoot = ProjectileType<Infernum_Flame>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 45);
            item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().infernumAmmo < 5)
            {
                if (Main.mouseLeftRelease)
                {
                    CombatText.NewText(player.Hitbox, new Color(0, 148, 255), "NO AMMO");
                    Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(12, 0), player.Center);
                    if (sound != null)
                        sound.Pitch = -0.5f;
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.GetModPlayer<PLAYERGLOBAL>().infernumAmmo -= 5;

            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY + 3)) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 16);
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
            return new Vector2(-30, 10);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 45), player.Center);
                if (sound != null)
                    sound.Pitch = -0.5f;
            }
        }
    }
}
