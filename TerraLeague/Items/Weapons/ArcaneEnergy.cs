using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Buffs;

namespace TerraLeague.Items.Weapons
{
    public class ArcaneEnergy : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            DisplayName.SetDefault("Arcane Energy");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Charge longer for strong attacks";
        }

        public override string GetQuote()
        {
            return "Behold my power";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Shocking Orb";
            else if (type == AbilityType.R)
                return "Rite of the Arcane";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/ShockingOrb";
            else if (type == AbilityType.R)
                return "AbilityImages/RightoftheArcane";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Launch an orb of magical energy that explodes on contact, stunning all hit." +
                    "Stun duration and explosion radius are based on travel distance";
            }
            else if (type == AbilityType.R)
            {
                return "Channel for 3 seconds, barraging the area around you with magic artillery";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return (int)(item.damage * 1.5);
            else if (type == AbilityType.R)
                return (int)(item.damage * 0.6);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.E)
            {
                if (dam == DamageType.MAG)
                    return 45;
            }
            else if (type == AbilityType.R)
            {
                if (dam == DamageType.MAG)
                    return 45;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.E)
                return 40;
            else if (type == AbilityType.R)
                return 100;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.E)
                return 13;
            else if (type == AbilityType.R)
                return 80;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 10);
                    int projType = ProjectileType<ArcaneEnergy_ShockingOrb>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 0;

                    SetAnimation(player, item.useTime, item.useAnimation, position + velocity);
                    DoEfx(player, type);
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(type), true))
                {
                    //DoEfx(player, type);
                    player.AddBuff(BuffType<RightoftheArcane>(), GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG));
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
            item.damage = 75;
            item.width = 32;
            item.height = 32;
            item.magic = true;
            item.useAnimation = 32;
            item.useTime = 32;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.knockBack = 5;
            item.value = 40000;
            item.rare = ItemRarityID.LightRed;
            item.shootSpeed = 12f;
            item.shoot = ProjectileType<ArcaneEnergy_PulseControl>();
            item.useTurn = true;
            item.noUseGraphic = true;
            item.mana = 20;
            item.autoReuse = false;
            item.channel = true;
            item.noMelee = true;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            item.noMelee = true;
            return true;
        }

        public override void AddRecipes()
        {
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemType<HextechCore>(), 2);
            //recipe.AddIngredient(ItemID.Bomb, 20);
            //recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 6);
            //recipe.AddTile(TileID.MythrilAnvil);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.E || type == AbilityType.R)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.E || type == AbilityType.R)
                Main.PlaySound(new LegacySoundStyle(2, 19, Terraria.Audio.SoundType.Sound), player.Center);
        }
    }
}
