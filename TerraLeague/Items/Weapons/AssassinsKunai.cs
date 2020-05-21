using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class AssassinsKunai : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Assassin's Kunai");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "Fear the assassin with no master";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Twilight Shroud";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/TwilightShroud";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Drop a smoke bomb that causes you to become obsucured and immune to projectiles while not using items";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return 0;
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 100;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return "";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return true;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 30;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    DoEfx(player, type);
                    int[] order = new int[] { -4, 4, -3, 3, -2, 2, -1, 1, 0};

                    for (int i = 0; i < 9; i++)
                    {
                        Projectile.NewProjectile(player.Center, new Vector2(order[i], 0), ProjectileType<ShroudSmoke>(), 0, 0, player.whoAmI);
                        Projectile.NewProjectile(player.Center, new Vector2(order[i], 0), ProjectileType<ShroudSmoke>(), 0, 0, player.whoAmI);
                        Projectile.NewProjectile(player.Center, new Vector2(order[i], 0), ProjectileType<ShroudSmoke>(), 0, 0, player.whoAmI);
                        Projectile.NewProjectile(player.Center, new Vector2(order[i], 0), ProjectileType<ShroudSmoke>(), 0, 0, player.whoAmI);
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
            item.damage = 40;
            item.width = 18;
            item.height = 36;
            item.magic = true;
            item.useTime = 23;
            item.useAnimation = 23;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 2;
            item.mana = 8;
            item.value = 300000;
            item.rare = ItemRarityID.Pink;
            item.UseSound = new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound);
            item.shoot = ProjectileType<Kunai>();
            item.shootSpeed = 16f;
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
            int numberProjectiles = 5;
            float startingAngle = 20;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(startingAngle));
                Projectile.NewProjectile(position, perturbedSpeed, type, damage, knockBack, player.whoAmI);
                startingAngle -= 10f;
            }

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddIngredient(ItemID.SmokeBomb);
            recipe.AddIngredient(ItemID.MagicDagger);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.W)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                player.itemAnimation = ItemUseStyleID.SwingThrow;
                player.itemAnimationMax = 24;
                player.reuseDelay = 24;
                Main.PlaySound(new LegacySoundStyle(2, 11).WithPitchVariance(-1), player.Center);
            }
        }
    }
}
