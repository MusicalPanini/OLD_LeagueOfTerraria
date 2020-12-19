using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class Riftwalk : Ability
    {
        public Riftwalk(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Riftwalk";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Riftwalk";
        }

        public override string GetAbilityTooltip()
        {
            return "Blink to the target location, dealing damage to all nearby enemies.";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 2);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 60;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 5;
        }

        public override int GetBaseManaCost()
        {
            return 50;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost()))
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

                        Projectile.NewProjectileDirect(player.MountedCenter, new Vector2(0, 0), ProjectileType<NetherBladeofHorok_RiftwalkHitbox>(), GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG), 0, player.whoAmI, -1);
                        player.CheckMana(GetBaseManaCost(), true);

                        DoEfx(player, type);
                        SetCooldowns(player, type);
                    }
                }
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 19, SoundType.Sound));
        }
    }
}
