using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class Gravitum : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gravitum");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Uses 5% Gravitum Ammo" +
                "\nEach Lunari gun has its own special ammo that rechages when the gun is not in use." +
                "\nYour attacks mark and slow your target";
        }

        public override string GetQuote()
        {
            return "Darkness will weigh upon them";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Binding Eclipse";
            else if (type == AbilityType.W)
                return "Phase";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/BindingEclipse";
            else if (type == AbilityType.W)
                return "AbilityImages/Phase";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "Expunge all marked enemies, stunning and dealing damage to them";
            }
            else if (type == AbilityType.W)
            {
                return "Swap weapon to Infernum";
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
                return (int)(item.damage * 2f);
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
                return 40;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage"
                    + "\nUses 10% Gravitum Ammo";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 14;
            else if (type == AbilityType.W)
                return 1;
            else
                return base.GetRawCooldown(type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            return false;
        }

        public override bool CanCurrentlyBeCast(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (player.GetModPlayer<PLAYERGLOBAL>().gravitumAmmo < 10)
                {
                    return false;
                }

                for (int i = 0; i < Main.npc.Length - 1; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.HasBuff(BuffType<GravitumMark>()) && npc.active && !npc.immortal)
                    {
                        return true;
                    }
                }
                return false;
            }
            return base.CanCurrentlyBeCast(player, type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type)))
                {
                    player.GetModPlayer<PLAYERGLOBAL>().gravitumAmmo -= 10;
                    player.CheckMana(GetBaseManaCost(type), true);
                    for (int i = 0; i < Main.npc.Length - 1; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.HasBuff(BuffType<GravitumMark>()) && npc.active && !npc.immortal)
                        {
                            Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ProjectileType<Gravitum_BindingEclipse>(), GetAbilityBaseDamage(player, type), 0, player.whoAmI, i);
                            SetCooldowns(player, type);
                        }
                    }
                }
            }
            else if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type))
                {
                    item.SetDefaults(ItemType<Infernum>());

                    CombatText.NewText(player.Hitbox, new Color(0, 148, 255), "INFERNUM");

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
            item.damage = 100;
            item.magic = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 82;
            item.height = 48;
            item.useAnimation = 30;
            item.useTime = 30;
            item.shootSpeed = 8f;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = 310000 * 5;
            item.rare = ItemRarityID.Purple;
            item.scale = 0.8f;
            item.shoot = ProjectileType<Gravitum_Orb>();
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 111);
            item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().gravitumAmmo < 5)
            {
                if (Main.mouseLeftRelease)
                {
                    Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(12, 0), player.Center);
                    if (sound != null)
                        sound.Pitch = -0.5f;
                    CombatText.NewText(player.Hitbox, new Color(200, 37, 255), "NO AMMO");
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            item.shootSpeed = 8f;
            player.GetModPlayer<PLAYERGLOBAL>().gravitumAmmo -= 5;
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX - 20, speedY - 20)) * 20;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, -1);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q)
                return true;
            if (type == AbilityType.W)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-30, -10);
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
