using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class EmperoroftheSands : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Emperor of the Sands");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetWeaponTooltip()
        {
            return "Create a solder of sand to fight for you!";
        }

        public override string GetQuote()
        {
            return "Your emperor shall return";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "Emperor's Divide";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/EmperorsDivide";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Call forward 2 walls of sand shields that knock enemies away";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return (int)(2 * item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.R)
            {
                if (dam == DamageType.SUM)
                    return 80;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.R)
                return 50;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " summon damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.R)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.R)
                return 45;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    DoEfx(player, type);
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = new Vector2(16, 0);
                    int projType = ProjectileType<EmperoroftheSands_EmperorsDivide>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.SUM);
                    int knockback = 30;

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(position, -velocity, projType, damage, knockback, player.whoAmI);
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
            item.damage = 11;
            item.summon = true;
            item.mana = 20;
            item.width = 48;
            item.height = 48;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.UseSound = new LegacySoundStyle(2, 113);
            item.shoot = ProjectileType<EmperoroftheSands_SandSolder>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;

            if (player.altFunctionUse != 2)
            {
                bool pathBlocked = false;
                for (int x = (int)((Main.mouseX + Main.screenPosition.X) / 16) - 1; x < (int)((Main.mouseX + Main.screenPosition.X) / 16) + 1; x++)
                {
                    for (int y = (int)((Main.mouseY + Main.screenPosition.Y) / 16) - 1; y <= (int)((Main.mouseY + Main.screenPosition.Y) / 16) + 1; y++)
                    {
                        if (Main.tile[x, y].collisionType > 0)
                        {
                            pathBlocked = true;
                            break;
                        }
                    }
                }

                if (!pathBlocked)
                {
                    return true;
                }
            }

            return false;
        }

        public override bool UseItem(Player player)
        {

            return base.UseItem(player);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Main.LocalPlayer.MinionNPCTargetAim();
            }
            else
            {
                player.AddBuff(BuffType<SandSolder>(), 2);
            }
            return base.CanUseItem(player);
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
                var sound = Main.PlaySound(new LegacySoundStyle(2, 82), player.Center);
                if (sound != null)
                    sound.Pitch = -0.5f;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Sapphire, 4);
            recipe.AddIngredient(ItemID.SandBlock, 100);
            recipe.AddRecipeGroup("TerraLeague:GoldGroup", 10);
            recipe.AddIngredient(ItemType<Sunstone>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}