using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class CrystalineVoidEnergy : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystaline Void Energy");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            return "Shots apply stacks of 'Caustic Wounds'" +
                "\nAt 5 stacks, the enemy will take 25% of their missing life as magic damage" +
                "\n(Capped at 50 + " + TerraLeague.CreateScalingTooltip(DamageType.MAG, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().MAG, 100) + ")";
        }

        public override string GetQuote()
        {
            return "Exploit their weakness";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Void Seeker";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
                if (modPlayer.magicDamageLastStep < modPlayer.rangedDamageLastStep)
                    return "AbilityImages/VoidSeeker";
                else
                    return "AbilityImages/VoidSeekerM";
            }
               
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Launch a void blast that applies 2 stacks of 'Caustic Wounds' to the hit enemy." +
                    "\nAt [c/" + TerraLeague.MAGColor +":50 MAG], Void Seeker will deal magic damage instead and apply 3 stacks.";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return (int)(item.damage * 3);
            }
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.W)
            {
                if (dam == DamageType.RNG)
                    return 150;
                else if (dam == DamageType.MAG)
                    return 60;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 30;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
                if (modPlayer.MAG >= 50)
                    return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
                else
                    return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " ranged damage";
            }
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 18;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return base.CanBeCastWhileUsingItem(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 8f);
                    int projType = ProjectileType<CrystalineVoidEnergy_VoidSeeker>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.RNG) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 1;

                    PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                    int poweredUp = 0;
                    if (modPlayer.MAG >= 50)
                        poweredUp = 1;

                    Projectile proj = Projectile.NewProjectileDirect(player.Center, velocity, projType, damage, knockback, player.whoAmI, poweredUp);


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
            item.ranged = true;
            item.width = 24;
            item.height = 54;
            item.useAnimation = 35;
            item.useTime = 35;
            item.shootSpeed = 6f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 5400;
            item.rare = ItemRarityID.Orange;
            item.UseSound =  new LegacySoundStyle(2, 75);
            item.autoReuse = true;
            item.shoot = ProjectileType<CrystalineVoidEnergy_VoidEnergy>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position.Y = position.Y + 4;
            
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidFragment>(), 120);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
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
                TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 91, -1f);
                TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 43, -0.5f);
            }

            base.Efx(player, type);
        }
    }
}
