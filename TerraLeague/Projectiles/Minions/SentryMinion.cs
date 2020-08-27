using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;

namespace TerraLeague.Projectiles.Minions
{
    public abstract class SentryMinion : BaseMinion
    {
        protected float viewDist = 400f;
        protected int shootCool = 90;
        protected int shootCountdown = 90;
        protected NPC target = null;
        protected float shootSpeed;
        protected int shoot;
        protected double angle = 0;
        protected LegacySoundStyle shootSound;
        

        public virtual void CreateDust()
        {
        }

        public virtual void SelectFrame()
        {
        }

        public override void Behavior()
        {
            Player player = Main.player[projectile.owner];
            float shortestDistance = viewDist;

            

            projectile.velocity.X = 0f;
            projectile.velocity.Y = projectile.velocity.Y + 0.2f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }


            if (player.HasMinionAttackTargetNPC)
            {
                NPC checkTarget = Main.npc[player.MinionAttackTargetNPC];

                float checkX = checkTarget.position.X + (float)checkTarget.width * 0.5f - projectile.Center.X;
                float checkY = checkTarget.position.Y + (float)checkTarget.height * 0.5f - projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(checkX * checkX + checkY * checkY));

                if (distance < shortestDistance && !checkTarget.friendly && checkTarget.life > 5 && Collision.CanHit(projectile.position, projectile.width, projectile.height/2, checkTarget.position, checkTarget.width, checkTarget.height))
                {
                    shortestDistance = distance;
                    target = checkTarget;

                    double xDis = target.Center.X - projectile.Center.X;
                    double yDis = target.Center.Y - projectile.Center.Y;
                    angle = Math.Atan(yDis / xDis);
                }
            }

            if (target != null)
            {
                if (target.whoAmI == player.MinionAttackTargetNPC)
                {

                }
                else if (target != null && target.active && Collision.CanHit(projectile.position, projectile.width, projectile.height / 2, target.position, target.width, target.height))
                {
                    if (Vector2.Distance(target.Center, projectile.position) < viewDist)
                    {
                        double xDis = target.Center.X - projectile.Center.X;
                        double yDis = target.Center.Y - projectile.Center.Y;
                        angle = Math.Atan(yDis / xDis);
                    }
                    else
                    {
                        target = null;
                    }
                }
                else
                {
                    target = null;
                }
            }

            if (target == null)
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC checkTarget = Main.npc[i];

                    float checkX = checkTarget.position.X + (float)checkTarget.width * 0.5f - projectile.Center.X;
                    float checkY = checkTarget.position.Y + (float)checkTarget.height * 0.5f - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(checkX * checkX + checkY * checkY));

                    if (distance < shortestDistance && !checkTarget.friendly && checkTarget.lifeMax > 5 && !checkTarget.immortal && checkTarget.active && Collision.CanHit(projectile.position, projectile.width, projectile.height / 2, checkTarget.position, checkTarget.width, checkTarget.height))
                    {
                        shortestDistance = distance;
                        target = checkTarget;
                    }
                }
                if (target != null)
                {
                    double xDis = target.Center.X - projectile.Center.X;
                    double yDis = target.Center.Y - projectile.Center.Y;
                    angle = Math.Atan(yDis / xDis);
                }
            }

            if (target != null)
            {
                if (shootCountdown == 0 && Collision.CanHit(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height))
                {
                    Vector2 velocity;
                    if (target.Center.X < projectile.position.X)
                        velocity = new Vector2(-shootSpeed, 0).RotatedBy(angle);
                    else
                        velocity = new Vector2(shootSpeed, 0).RotatedBy(angle);

                    if (target.Center.Y - projectile.Center.Y < 0)
                    {
                        velocity.Y = (float)-Math.Sqrt(Math.Pow(velocity.Y, 2));
                    }
                    else
                    {
                        velocity.Y = (float)Math.Sqrt(Math.Pow(velocity.Y, 2));
                    }

                    Projectile.NewProjectileDirect(projectile.Center, velocity, shoot, (int)(projectile.damage), projectile.knockBack, projectile.owner);
                    Main.PlaySound(shootSound, projectile.position);

                    shootCountdown = shootCool;
                }
            }
            else
            {
                angle = MathHelper.ToRadians(0);
            }
            if (shootCountdown > 0)
            {
                shootCountdown--;
            }
            SelectFrame();
            CreateDust();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public virtual void ChangeAnimation(double angle)
        {
        }
    }
}
