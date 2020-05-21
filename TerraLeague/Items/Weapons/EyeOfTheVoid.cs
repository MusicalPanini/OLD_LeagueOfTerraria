using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Linq;
using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class EyeOfTheVoid : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of the Void");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Fire a lifeform disintegration ray!";
        }

        public override string GetQuote()
        {
            return "Knowledge through... disintegration";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Plasma Fission";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/PlasmaFission";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Fire a projectile that splits in 2 on collision." +
                "\nRecast to split early." +
                "\nHit enemies will take 10% more magic damage.";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return (int)(item.damage * 2);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.MAG)
                    return 80;
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
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (CurrentlyHasSpecialCast(Main.LocalPlayer, type))
                return true;
            else
                return false;
        }

        public override bool CanBeCastWhileCCd(AbilityType type)
        {
            if (CurrentlyHasSpecialCast(Main.LocalPlayer, type))
                return true;
            else
                return false;
        }

        public override bool CurrentlyHasSpecialCast(Player player, AbilityType type)
        {
            if (type == AbilityType.Q && Main.LocalPlayer.ownedProjectileCounts[ProjectileType<Plasma>()] > 0 && player.GetModPlayer<PLAYERGLOBAL>().AbilityCooldowns[0] <= GetCooldown(type) * 60 - 10)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 8;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (CurrentlyHasSpecialCast(Main.LocalPlayer, type))
                {
                    Main.projectile.Where(x => x.type == ProjectileType<Plasma>() && x.owner == player.whoAmI).FirstOrDefault().Kill();
                }
                else if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12);
                    int projType = ProjectileType<Plasma>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 1;

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);

                    SetAnimation(player, 20, 20, position + velocity);
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
            item.damage = 16;
            item.noMelee = true;
            item.magic = true;
            item.channel = true;
            item.mana = 6;
            item.rare = ItemRarityID.Orange;
            item.value = 5400;
            item.width = 28;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 20;
            item.UseSound = new LegacySoundStyle(2, 15);
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 10;
            item.shoot = ProjectileType<EyeLaser>();
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 && player.statMana >= 200)
            {
                return true;
            }
            else if (player.altFunctionUse == 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidFragment>(), 64);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                
                var efx = Main.PlaySound(new LegacySoundStyle(2, 91), player.Center);
                if (efx != null)
                    efx.Pitch = -1;

                efx = Main.PlaySound(new LegacySoundStyle(2, 43), player.Center);
                if (efx != null)
                    efx.Pitch = -0.5f;
            }
        }
    }
}
