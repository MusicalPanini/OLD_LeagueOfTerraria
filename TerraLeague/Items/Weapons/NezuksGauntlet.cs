using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class NezuksGauntlet : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ne'Zuk's Gauntlet");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "The gauntlet's for show... the talent's all me.";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.W)
                return "Essence Flux";
            else if (type == AbilityType.E)
                return "Arcane Shift";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.W)
                return "AbilityImages/EssenceFlux";
            else if (type == AbilityType.E)
                return "AbilityImages/ArcaneShift";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.W)
            {
                return "Fire a ring of magic that applies 'Essence Flux'" +
                "\nHitting a Flux'd enemy with an attack from the gauntlet, causes them to take addtional damage and restores 40 mana";
            }
            else if (type == AbilityType.E)
            {
                return "Blink up to 20 blocks away and fire a projectile at the nearest enemy";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return (int)(item.damage * 2.5);
            else if (type == AbilityType.E)
                return (int)(item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.W)
            {
                if (dam == DamageType.RNG)
                    return 60;
                else if (dam == DamageType.MAG)
                    return 90;
            }
            else if (type == AbilityType.E)
            {
                if (dam == DamageType.RNG)
                    return 50;
                else if (dam == DamageType.MAG)
                    return 75;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.W)
                return 30;
            else if (type == AbilityType.E)
                return 50;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage on detonation";
            else if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.RNG) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.W || type == AbilityType.E)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.W)
                return 12;
            else if (type == AbilityType.E)
                return 12;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.Center;
                    Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12);
                    int projType = ProjectileType<EssenceFlux>();
                    int damage = (int)(1);
                    int knockback = 0;

                    SetAnimation(player, item.useTime, item.useAnimation, position + velocity);
                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    DoEfx(player, type);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type)))
                {
                    int distance = 20;

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
                            NetMessage.SendData(65, -1, -1, null, 0, (float)player.whoAmI, teleportPos.X, teleportPos.Y, 1, 0, 0);

                            player.velocity.Y = 0;

                            Projectile.NewProjectileDirect(player.Center, new Vector2(0, 0), ProjectileType<ArcaneShiftProj>(), GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.RNG) + GetAbilityScalingDamage(player, type, DamageType.MAG), 0, player.whoAmI, -1);
                            player.CheckMana(GetBaseManaCost(type), true);
                            
                            DoEfx(player, type);
                            SetCooldowns(player, type);
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
            item.damage = 52;
            item.mana = 7;
            item.ranged = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.width = 32;
            item.height = 34;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            item.knockBack = 4f;
            item.value = 55000;
            item.rare = 4;
            item.shootSpeed = 10f;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 73);
            item.shoot = ProjectileType<MysticShot>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.W || type == AbilityType.E)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.W)
            {

            }
            else if(type == AbilityType.E)
            {

            }
        }
    }

    public class GauntletGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.netID == NPCID.Mimic && Main.rand.Next(0, 5) == 0)
            {
                Item.NewItem(npc.getRect(), ItemType<NezuksGauntlet>(), 1, false, -2);
            }

            base.NPCLoot(npc);
        }
    }
}
