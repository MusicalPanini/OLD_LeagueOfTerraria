using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace TerraLeague.Items.Weapons
{
    public class TheFallenCelestialsDarkMagic : AbilityItem
    {
        public static int range = 550;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Fallen Celestials Dark Magic");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Deals increased damage the lower the enemies life";
        }

        public override string GetQuote()
        {
            return "I am bound, but I will not Break";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Black Shield";
            else if (type == AbilityType.R)
                return "Soul Shackles";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/BlackShield";
            else if (type == AbilityType.R)
                return "AbilityImages/SoulShackles";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Target an ally or yourself and grant them a magic shield that protects them from debuffs";
            }
            else if (type == AbilityType.R)
            {
                return "Unleash celestial chains upon near by enemies." +
                    "\nAfter 3 seconds, if the chain is not broken, the enemy will be restuck and stunned for 4 seconds." +
                    "\nThe chain will break if the target moves too far away";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return (int)(1.5 * item.damage);
            else if (type == AbilityType.R)
                return (int)(4 * item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.E)
            {
                if (dam == DamageType.MAG)
                    return 70;
            }
            else if (type == AbilityType.R)
            {
                if (dam == DamageType.MAG)
                    return 70;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.E)
                return 100;
            else if (type == AbilityType.R)
                return 100;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic shielding";
            else if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.E)
                return 22;
            else if (type == AbilityType.R)
                return 70;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.E)
                return true;
            else
                return false;
        }

        public override bool CanCurrentlyBeCast(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC target = Main.npc[i];
                    if (!target.townNPC && !target.immortal && target.active)
                    {
                        if (player.Distance(target.Center) <= range)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            return base.CanCurrentlyBeCast(player, type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = new Vector2(Main.MouseWorld.X, player.position.Y + 600);
                    Vector2 velocity = new Vector2(0, -1.25f);
                    int projType = ProjectileType<BurningVengance_PillarOfFlame>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 0;

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    DoEfx(player, type);
                    for (int i = 0; i < Main.npc.Length; i++)
                    {
                        NPC target = Main.npc[i];
                        if (!target.townNPC && !target.immortal && target.active)
                        {
                            if (player.Distance(target.Center) <= range)
                            {
                                Vector2 position = player.MountedCenter;
                                Vector2 velocity = Vector2.Zero;
                                int projType = ProjectileType<TheFallenCelestialsDarkMagic_SoulShackles>();
                                int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                                int knockback = 0;

                                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, i);
                            }
                        }
                    }

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
            item.damage = 24;
            item.noMelee = true;
            item.magic = true;
            item.useTime = 80;
            item.useAnimation = 80;
            item.mana = 40;
            item.rare = ItemRarityID.Pink;
            item.value = 300000;
            item.width = 28;
            item.height = 32;
            item.knockBack = 0;
            item.UseSound = SoundID.Item20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ProjectileType<TheFallenCelestialsDarkMagic_TormentedShadow>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectileDirect(Main.MouseWorld, Vector2.Zero, type, damage, knockBack, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 20);
            recipe.AddIngredient(ItemID.Chain, 10);
            recipe.AddIngredient(ItemType<FragmentOfTheAspect>(), 1);
            recipe.AddIngredient(ItemType<CelestialBar>(), 20);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.R)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            SoundEffectInstance sound = Main.PlaySound(new LegacySoundStyle(3, 54, Terraria.Audio.SoundType.Sound), player.Center);
            if (sound != null)
                sound.Pitch = -0.2f;

            for (int i = 0; i < 40; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 248, 0, 0, 0, new Color(159, 0, 255), 1.5f);
                dust.velocity *= 2f;
                dust.noGravity = true;
            }
        }
    }
}
