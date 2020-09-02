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
    public class TerrorOfTheVoid : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terror of the Void");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "Your souls will feed the void";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "Feast";
            else if (type == AbilityType.Q)
                return "Rupture";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/Feast";
            else if (type == AbilityType.Q)
                return "AbilityImages/Rupture";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Take a giant bite out of everything around you." +
                    "\nKilling an enemy will take there max life as stacks." +
                    "\nExecute any enemy hit with less health than your stacks." +
                    "\nGain a buff based on the amount of stacks you currently have." +
                    "\nLose all stacks on death." +
                    "\nTier 1: 500 | Tier 2: 2500 | Tier 3: 12500" +
                    "\nCurrent Stacks: " + Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().feastStacks;
            }
            else if (type == AbilityType.Q)
            {
                return "Create a large rupture from the ground underneath you, launching all hit into the air";
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
            else if (type == AbilityType.Q)
                return (int)(item.damage * 2);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.R)
            {
                if (dam == DamageType.MAG)
                    return 70;
            }
            else if (type == AbilityType.Q)
            {
                if (dam == DamageType.MAG)
                    return 100;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.R)
                return 100;
            else if (type == AbilityType.Q)
                return 75;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " + 10% Max Life magic damage";
            else if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return true;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.R)
                return 70;
            else if (type == AbilityType.Q)
                return 12;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = Vector2.Zero;
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG) + player.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep/10;
                    int knockback = 2;

                    PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                    int stacks = 0;
                    if (modPlayer.feast1)
                        stacks = 1;
                    else if (modPlayer.feast2)
                        stacks = 2;
                    else if (modPlayer.feast3)
                        stacks = 3;


                    Projectile.NewProjectileDirect(position, velocity, ProjectileType<TerrorOfTheVoid_FeastTop>(), damage, knockback, player.whoAmI, stacks);
                    Projectile.NewProjectileDirect(position, velocity, ProjectileType<TerrorOfTheVoid_FeastBot>(), damage, knockback, player.whoAmI, stacks);

                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type))
                {
                    Vector2 position = player.MountedCenter;
                    position.Y -= 16;
                    Vector2 velocity = new Vector2(0, 1000);
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 0;

                    for (int i = 0; i < 20; i++)
                    {
                        Projectile.NewProjectileDirect(new Vector2(position.X - (32 * i + 16), position.Y), velocity, ProjectileType<TerrorOfTheVoid_RuptureSpike>(), damage, knockback, player.whoAmI, -1);
                        Projectile.NewProjectileDirect(new Vector2(position.X + (32 * i + 16), position.Y), velocity, ProjectileType<TerrorOfTheVoid_RuptureSpike>(), damage, knockback, player.whoAmI, 1);

                    }

                    SetCooldowns(player, type);
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
            item.damage = 55;
            item.width = 48;
            item.height = 48;
            item.magic = true;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.mana = 40;
            item.value = 35000;
            item.rare = ItemRarityID.Lime;
            item.shoot = ProjectileType<TerrorOfTheVoid_RuptureControl>();
            item.shootSpeed = 1f;
            item.UseSound = SoundID.Item8;
            item.noMelee = true;
        }

        public override bool CanUseItem(Player player)
        {
            //if (player.ownedProjectileCounts[ProjectileType<TerrorOfTheVoid_RuptureSpike>()] > 0)
            if (player.ownedProjectileCounts[ProjectileType<TerrorOfTheVoid_RuptureControl>()] > 0)
                return false;
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (speedX > 0)
                speedX = 3;
            else
                speedX = -3;

            speedY = 0;

            Projectile.NewProjectile(player.Top.X, player.Top.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, player.GetModPlayer<PLAYERGLOBAL>().feast3 ? 1 : 0);

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.R)
                return true;
            //if (type == AbilityType.Q && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().feast3)
            //    return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(player.position, 32, 32, DustID.Blood, 0f, -2f, 100, default(Color), 3f);
                    dust.velocity *= 2;
                }
            }
            else if (type == AbilityType.Q)
            {
                TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 24, -1f);
            }
        }
    }
}
