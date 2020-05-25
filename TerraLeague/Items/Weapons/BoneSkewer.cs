using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class BoneSkewer : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bone Skewer");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "There’s plenty of room for everyone at the bottom of the sea...";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "Death From Below";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/DeathFromBelow";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Mark the targeted area with an X then strike after a delay." +
                    "\nKills will allow you to cast again for a short time.";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return (int)(item.damage * 1.5);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.R)
                if (dam == DamageType.MEL)
                return 125;
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                if (CurrentlyHasSpecialCast(Main.LocalPlayer, type))
                    return 0;
                else
                    return 50;
            }
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MEL) + " melee damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.R)
                return 60;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CurrentlyHasSpecialCast(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                if (player.GetModPlayer<PLAYERGLOBAL>().deathFromBelowRefresh)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                if ((CheckIfNotOnCooldown(player, type) || CurrentlyHasSpecialCast(player, type)) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    if (CurrentlyHasSpecialCast(player, type))
                        player.ClearBuff(BuffType<DeathFromBelowRefresh>());
                    else
                        SetCooldowns(player, type);

                    Vector2 position = Main.MouseWorld;
                    Vector2 velocity = Vector2.Zero;
                    int projType = ProjectileType<BoneSkewer_DeathFromBelow>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MEL);

                    int knockback = 0;

                    Projectile.NewProjectile(position + new Vector2(95, 90), velocity, projType, damage, knockback, player.whoAmI, 1);
                    Projectile.NewProjectile(position + new Vector2(-95, 90), velocity, projType, damage, knockback, player.whoAmI, -1);

                    DoEfx(player, type);
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.width = 48;
            item.height = 48;
            item.melee = true;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 2;
            item.value = 3500;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item1;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BrassBar>(), 14);
            recipe.AddTile(TileID.Anvils);
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
            if (type == AbilityType.R)
            {
                Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new LegacySoundStyle(2, 117).WithPitchVariance(0.8f), player.Center);
                if (sound != null)
                    sound.Pitch = -0.5f;
            }
                
        }
    }
}
