using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Severum : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Severum");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "User 2% Severum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nYour attacks lifesteal";
        }

        public override string GetQuote()
        {
            return "Harvest death for life";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Onslaught";
            else if (type == AbilityType.W)
                return "Phase";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/Onslaught";
            else if (type == AbilityType.W)
                return "AbilityImages/Phase";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Rapidly attack all nearby enemies, lifestealing on hit";
            }
            else if (type == AbilityType.W)
            {
                return "Swap weapon to Gravitum";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
                return (int)(item.damage * 0.2f);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.MEL)
                    return 20;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 40;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MEL) + " melee damage"
                    + "\nUses 10% Severum Ammo";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 20;
            else if (type == AbilityType.W)
                return 1;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            return false;
        }

        public override bool CanCurrentlyBeCast(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (player.GetModPlayer<PLAYERGLOBAL>().severumAmmo < 10)
                {
                    return false;
                }
            }
            return base.CanCurrentlyBeCast(player, type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.GetModPlayer<PLAYERGLOBAL>().severumAmmo -= 10;
                    int damage = GetAbilityBaseDamage(player, type) + (GetAbilityScalingDamage(player, type, DamageType.MEL));
                    player.AddBuff(BuffType<Onslaught>(), damage);
                    //DoEfx(player, type);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type))
                {
                    item.SetDefaults(ItemType<Gravitum>());

                    CombatText.NewText(player.Hitbox, new Color(200, 37, 255), "GRAVITUM");

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
            item.damage = 115;
            item.melee = true;
            item.channel = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 48;
            item.height = 48;
            item.useAnimation = 14;
            item.useTime = 14;
            item.shootSpeed = 80;
            item.knockBack = 2;
            item.value = 310000 * 5;
            item.rare = ItemRarityID.Purple;
            item.scale = 0.7f;
            item.shoot = ProjectileType<Severum_Slash>();
            item.UseSound = null;
            item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().severumAmmo < 2)
            {
                if (Main.mouseLeftRelease)
                {
                    Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(12, 0), player.Center);
                    if (sound != null)
                        sound.Pitch = -0.5f;
                    CombatText.NewText(player.Hitbox, new Color(216, 0, 32), "NO AMMO");
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            else if (type == AbilityType.W)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 10);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 13), player.Center);
                if (sound != null)
                    sound.Pitch = 1f;
            }
        }
    }
}
