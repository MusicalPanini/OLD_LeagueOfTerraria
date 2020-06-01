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
    public class NetherBladeofHorok : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nether Blade of Horok");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "You are null and void";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "Riftwalk";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/Riftwalk";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Blink to the target location, dealing damage to all nearby enemies.";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return (int)(item.damage * 2);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.R)
                return 50;
            else
                return base.GetBaseManaCost(type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.R)
            {
                if (dam == DamageType.MAG)
                    return 60;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
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
                return 5;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type)))
                {
                    if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type)))
                    {
                        int distance = 40;

                        float xDis = Main.MouseWorld.X - player.position.X;
                        float yDis = Main.MouseWorld.Y - player.position.Y;

                        if (xDis < -distance * 16)
                            xDis = -distance * 16;
                        else if (xDis > distance * 16)
                            xDis = distance * 16;

                        if (yDis < -distance * 16)
                            yDis = -distance * 16;
                        else if (yDis > distance * 16)
                            yDis = distance * 16;

                        float newX = xDis + player.position.X;
                        float newY = yDis + player.position.Y;

                        Vector2 teleportPos = default(Vector2);
                        teleportPos.X = newX;
                        if (player.gravDir == 1f)
                        {
                            teleportPos.Y = newY - (float)player.height;
                        }
                        else
                        {
                            teleportPos.Y = (float)Main.screenHeight - newY;
                        }
                        teleportPos.X -= (float)(player.width / 2);
                        if (teleportPos.X > 50f && teleportPos.X < (float)(Main.maxTilesX * 16 - 50) && teleportPos.Y > 50f && teleportPos.Y < (float)(Main.maxTilesY * 16 - 50))
                        {
                            int blockX = (int)(teleportPos.X / 16f);
                            int blockY = (int)(teleportPos.Y / 16f);
                            if ((Main.tile[blockX, blockY].wall != 87 || !((double)blockY > Main.worldSurface) || NPC.downedPlantBoss) && !Collision.SolidCollision(teleportPos, player.width, player.height))
                            {
                                player.Teleport(teleportPos, 1, 0);
                                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, (float)player.whoAmI, teleportPos.X, teleportPos.Y, 2, 0, 0);

                                player.velocity.Y = 0;

                                Projectile.NewProjectileDirect(player.MountedCenter, new Vector2(0, 0), ProjectileType<NetherBladeofHorok_RiftwalkHitbox>(), GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG), 0, player.whoAmI, -1);
                                player.CheckMana(GetBaseManaCost(type), true);

                                DoEfx(player, type);
                                SetCooldowns(player, type);
                            }
                        }
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
            item.damage = 74;
            item.width = 40;
            item.height = 40;
            item.melee = true;
            item.useTime = 32;
            item.useAnimation = 32;
            item.scale = 1.3f;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.value = 120000;
            item.rare = ItemRarityID.Lime;
            item.shoot = ProjectileType<NetherBladeofHorok_NullSphere>();
            item.autoReuse = true;
            item.UseSound = new LegacySoundStyle(2, 15);
            item.shootSpeed = 7;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectileDirect(player.MountedCenter, new Vector2(speedX, speedY), type, damage, 0, player.whoAmI, -1);
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, 112, 0,0, 255, new Color(59, 0, 255), 1f);
            dust.noGravity = true;
            dust.noLight = true;

            base.MeleeEffects(player, hitbox);
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.R)
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
            if (type == AbilityType.R)
            {
                //Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new LegacySoundStyle(4, 51).WithPitchVariance(0.8f), player.Center);
                //if (sound != null)
                //    sound.Pitch = -1f;
            }

            base.Efx(player, type);
        }
    }
}
