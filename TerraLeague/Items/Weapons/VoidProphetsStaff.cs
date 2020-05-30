using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class VoidProphetsStaff : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Prophets Staff");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Summon a gate way from the void";
        }

        public override string GetQuote()
        {
            return "Bow to the void! Or be consumed by it!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Malefic Visions";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/MaleficVisions";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Infect an enemy's mind, dealing damage over time." +
                    "\nIf the enemy dies during the visions, it will spread to anpther near by enemy";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return item.damage * 2;
            return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.E)
            {
                if (dam == DamageType.SUM)
                    return 20;
            }
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
            if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.SUM) + " summon damage per half second";
            return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.E)
                return 16;
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
                int npc = TerraLeague.NPCMouseIsHovering();
                if (npc != -1)
                {
                    if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                    {
                        Vector2 position = player.MountedCenter;
                        Vector2 velocity = TerraLeague.CalcVelocityToMouse(player.Center, 4);
                        int projType = ProjectileType<VoidProphetsStaff_MaleficVisions>();
                        int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.SUM);
                        int knockback = 0;

                        Projectile proj = Projectile.NewProjectileDirect(position, new Vector2(0, -10), projType, damage, knockback, player.whoAmI, npc);

                        SetAnimation(player, 30, 30, position + velocity);
                        DoEfx(player, type);
                        SetCooldowns(player, type);
                    }
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
            item.summon = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 48;
            item.height = 48;
            item.useAnimation = 30;
            item.useTime = 30;
            item.mana = 10;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 35000;
            item.rare = ItemRarityID.Lime;
            item.scale = 1f;
            item.shoot = ProjectileType<VoidProphetsStaff_ZzrotPortal>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 113);
            item.autoReuse = false;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.FindSentryRestingSpot(item.shoot, out int xPos, out int yPos, out int yDis);
            Projectile.NewProjectile((float)xPos, (float)(yPos - yDis), 0f, 0f, type, damage, knockBack, player.whoAmI, 10, -1);
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
            recipe.AddIngredient(ItemType<VoidBar>(), 14);
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
