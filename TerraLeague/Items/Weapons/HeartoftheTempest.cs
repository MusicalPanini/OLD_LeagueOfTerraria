using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class HeartoftheTempest : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart of the Tempest");

            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Create a Slicing Maelstrom after hitting 3 times";
        }

        public override string GetQuote()
        {
            return "I am the wind!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Lightning Rush";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/LightningRush";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Surround yourself with lightning, dealing damage on contact." +
                    "\nYou are immune to contact damage for the duration";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return (int)(item.damage * 2);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.E)
            {
                if (dam == DamageType.MAG)
                    return 80;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.E)
                return 50;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.E)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.E)
                return 25;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.AddBuff(BuffType<LightningRush>(), 180);
                    Projectile.NewProjectile(player.MountedCenter, Vector2.Zero, ProjectileType<HeartoftheTempest_LightningRush>(), GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG), 0, player.whoAmI);
                    player.GetModPlayer<PLAYERGLOBAL>().lightningRush = true;
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
            item.damage = 60;
            item.width = 30;
            item.height = 30;
            item.melee = true;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 2.5f;
            item.value = 350000;
            item.rare = ItemRarityID.Lime;
            item.UseSound = SoundID.Item1;
            item.shootSpeed = 16f;
            item.shoot = ProjectileType<HeartoftheTempest_Yoyo>();
            item.noMelee = true;
            item.channel = true;
            item.noUseGraphic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<HarmonicBar>(), 16);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.E)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new LegacySoundStyle(3, 53), player.MountedCenter);
                if (sound != null)
                    sound.Pitch = 0.25f;
            }
        }
    }
}
