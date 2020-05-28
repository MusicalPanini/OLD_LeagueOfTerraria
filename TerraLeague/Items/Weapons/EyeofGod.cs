using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class EyeofGod : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of God");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Summon a tentacle of Nagakabouros to fight for you";
        }

        public override string GetQuote()
        {
            return "There are kind and gentle gods. Mine isn't one of those.";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Test of Spirit";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/TestofSpirit";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Reach out and pull the spirit of an enemy." +
                    "\nThis spirit can be attacked to deal 50% of the damage back to the owner of the spirit.";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.E)
                return 40;
            return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.E)
                return 14;
            return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12f);
                    int projType = ProjectileType<EyeofGod_TestofSpirit>();
                    int damage = 1;
                    int knockback = 0;

                    Projectile proj = Projectile.NewProjectileDirect(position, velocity, projType, damage, knockback, player.whoAmI);

                    SetAnimation(player, 30, 30, position + velocity);
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
            item.damage = 15;
            item.summon = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 32;
            item.height = 32;
            item.useAnimation = 30;
            item.useTime = 30;
            item.mana = 10;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 3500;
            item.rare = ItemRarityID.Green;
            item.scale = 1f;
            item.shoot = ProjectileType<EyeofGod_Tentacle>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 8);
            item.autoReuse = false;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.FindSentryRestingSpot(item.shoot, out int xPos, out int yPos, out int yDis);
            Projectile.NewProjectile((float)xPos, (float)(yPos - yDis) - 28, 0f, 0f, type, damage, knockBack, player.whoAmI, 10, -1);
            player.UpdateMaxTurrets();

            return false;
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.E)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BrassBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 8), player.Center);
                if (sound != null)
                    sound.Pitch = -0.25f;
            }
        }
    }
}
