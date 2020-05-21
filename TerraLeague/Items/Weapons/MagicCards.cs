using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class MagicCards : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Card Masters Deck");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Has a chance to throw a special card" +
                "\n[c/0066ff:Blue Card] - Restore 25 mana on hit" +
                "\n[c/ffff4d:Yellow Card] - Applies 'Stunned'" +
                "\n[c/ff1a1a:Red Card] - Explodes on contact";
        }

        public override string GetQuote()
        {
            return "Lady luck is smilin'";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Wild Cards";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/WildCards";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Throw 3 cards in a cone";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return (int)System.Math.Round(item.damage * 1.5);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.MAG)
                {
                    return 65;
                }
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
            if (type == AbilityType.Q)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 6;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 15 * 0.6f);
                    int projType = ProjectileType<GreenCard>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, AbilityType.Q, DamageType.MAG);
                    int knockback = 4;

                    int numberProjectiles = 3;
                    float startingAngle = 30;
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(startingAngle));
                        Projectile.NewProjectile(position, perturbedSpeed, projType, damage, knockback, player.whoAmI, 1);
                        startingAngle -= 30f;
                    }
                    SetAnimation(player, item.useTime, item.useAnimation, position + velocity);
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
            item.damage = 12;
            item.width = 24;
            item.height = 24;
            item.magic = true;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 2;
            item.mana = 6;
            item.value = 3500;
            item.rare = ItemRarityID.Green;
            item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            item.shootSpeed = 15f;
            item.shoot = ProjectileType<GreenCard>();
            item.noMelee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.noUseGraphic = true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.Next(0, 10) == 0)
            {
                switch (Main.rand.Next(0, 3))
                {
                    case 0:
                        type = ProjectileType<RedCard>();
                        knockBack *= 2;
                        damage = (int)(damage * 1.25);
                        break;
                    case 1:
                        type = ProjectileType<BlueCard>();
                        damage = (int)(damage * 1.5);
                        break;
                    case 2:
                        type = ProjectileType<YellowCard>();
                        knockBack = 0;
                        break;
                    default:
                        break;
                }
            }
            return true;
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
            if (type == AbilityType.Q)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                Main.PlaySound(new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound));
        }
    }
}
