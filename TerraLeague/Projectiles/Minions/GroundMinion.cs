﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraLeague.Projectiles.Minions
{
    public abstract class GroundMinion : ModProjectile
    {
        // Animation Frames
        public int idleFrame = 0;
        public int idleFrameCount = 1;
        public int idleFrameSpeed = 0;
        public int attackFrame = 1;
        public int attackFrameCount = 3;
        public int runFrame = 1;
        public int runFrameCount = 7;
        public float runFrameSpeedMod = 1f; //frames incremented per 2px
        public int flyFrame = 8;
        public int flyFrameCount = 4;
        public int flyFrameSpeed = 6;
        public float flyRotationMod = 0.7f;
        public int fallFrame = 12;
        public int fallFrameCount = 1;
        public int fallFrameSpeed = 0;

        // Code Constants
        public const float separatedDistance = 700f; // Distance before returning
        public const float separatedReturnAccel = 10f; // Max return speed, inceases to player speed
        public const float joinAccel = 0.1f; // Acceleration to catching up
        public const int joinDistance = 200; // Distance at which can stop flying
        public const int joinCloseDistance = 60; // Distance to stay close to

        public const float targetRange = 800f; // Range to find NPCs
        public const float chaseRange = 600f; // Range to begin chasing

        public const int attackAnimationMax = 15; //Time spent in attack state
        public const float attackRange = 30f; // Range to start attack state

        public const float topSpeed = 3f; // Max ground speed, inceases to player speed
        public const float slowStopAccel = 0.1f; // Speedup/Slowing down speed
        public const float quickStopAccel = 0.3f; // Speedup/Slowing down if moving wrong way
        public const float quickStopFastAccel = 0.5f; // Speedup/Slowing down if moving wrong way, if moving faster than normal (see topSpeed)

        /// <summary> Targetting uses player not projectile </summary>
        public bool AIPrioritiseNearPlayer = false;
        /// <summary> Targetting constantly looks for furthest away enemy </summary>
        public bool AIPrioritiseFarEnemies = false;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            // CHeck if the player still uses projectile minion type

            LogicManager(player);

            // Set the frames used by the minion
            ManageFrames(ref projectile.frame, ref projectile.frameCounter);

            // Natural "Friction"
            //projectile.velocity *= 0.99f;

            if (aiState == 0)
            {
                // Apply gravity
                projectile.velocity.Y = Math.Min(projectile.velocity.Y + 0.4f, 10f);
            }
        }

        private int aiState
        {
            get { return (int)projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }
        private int aiAttackAnimation
        {
            get { return (int)projectile.ai[1]; }
            set { projectile.ai[01] = value; }
        }
        private void LogicManager(Player player)
        {
            // Calculate idle positon behind player
            Vector2 goalVector = player.Center;
            goalVector.X -= (15 + player.width / 2) * player.direction;
            goalVector.X -= projectile.minionPos * 40 * player.direction;
            NPC target = null;

            // Find an enemy target
            //!/Main.NewText("SelectTarget Fine");
            if (aiState == 0)
            {
                target = SelectTarget(target);
            }
            // Catch up with players when separated
            //!/Main.NewText("CatchUpWithPlayer Fine");
            if (aiState == 1)
            {
                CatchUpWithPlayer(player);
            }
            // Attacking, or damaging state
            //!/Main.NewText("ExecuteAttackState Fine");
            if (aiState == 2)
            {
                ExecuteAttackState();
            }

            // Manage ai states for attacking and goal vector
            //!/Main.NewText("SetGoalToTargetAndAttack Fine");
            if (target != null)
            {
                goalVector = SetGoalToTargetAndAttack(goalVector, target, player);
            }
            // Manage ai state for being separated from the player
            //!/Main.NewText("CheckSeparated Fine");
            if (aiState == 0)
            {
                if (target == null) CheckSeparated(player);

                projectile.tileCollide = true;

                //!/Main.NewText("ControlMovementPhysics Fine");
                ControlMovementPhysics(player, goalVector);
            }
        }

        #region LogicManager Methods

        private NPC SelectTarget(NPC target)
        {
            // Start with ownerMinion, then cycle through first 200 if not
            NPC npc = projectile.OwnerMinionAttackTargetNPC;
            if (npc != null)
            {
                //!/ Main.NewText("<Fox> Looking at targetted foe!");
                if (npc.CanBeChasedBy(projectile, false))
                {
                    float distance = (npc.Center - projectile.Center).Length();
                    if (distance < targetRange)
                    {
                        target = npc;
                    }
                }
            }

            if (target == null)
            {
                float maxChase = targetRange;
                if (AIPrioritiseFarEnemies) maxChase = 0;
                float distance = targetRange;
                // Try the same but for everything else
                for (int i = 0; i < 200; i++)
                {
                    // Only select if not -1
                    npc = Main.npc[i];
                    if (npc.CanBeChasedBy(projectile, false))
                    {
                        if (AIPrioritiseNearPlayer)
                        {
                            distance = (npc.Center - Main.player[projectile.owner].Center).Length();
                        }
                        else
                        {
                            distance = (npc.Center - projectile.Center).Length();
                        }
                        if (AIPrioritiseFarEnemies)
                        {
                            if (distance > maxChase && distance < targetRange)
                            {
                                target = npc;
                                maxChase = distance;
                            }
                        }
                        else
                        {
                            if (distance < maxChase)
                            {
                                target = npc;
                                maxChase = distance;
                            }
                        }
                    }
                }
            }
            //!/ Main.NewText("<Fox> Found a target!");
            return target;
        }

        private void CatchUpWithPlayer(Player player)
        {
            projectile.tileCollide = false;

            Vector2 vectorToPlayer = (player.Center - projectile.Center);
            float distance = vectorToPlayer.Length();

            // Get the target accel to reach to catch up to player
            float targetAccel = Math.Max(separatedReturnAccel, Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y));

            // Too far away, just teleport
            TeleportIfFarAway(player, distance);

            if (distance < 100 /*joinDistance*/ && // Within Join distance
                /*player.velocity.Y == 0f &&*/ //Player is grounded
                projectile.Center.Y <= player.BottomLeft.Y && // Above the player
                !Collision.SolidCollision(projectile.position, projectile.width, projectile.height) // Not in a tile
                )
            {
                // Update to 0 and stop gaining height past 6 velocity
                aiState = 0;
                projectile.netUpdate = true;
                projectile.velocity.Y = Math.Max(projectile.velocity.Y, -6f);
            }

            // Speed up until close enough, if not within join distance
            if (distance >= joinCloseDistance)
            {
                //!/Main.NewText("<Fox> Too far!");
                vectorToPlayer.Normalize();
                vectorToPlayer *= targetAccel;

                if (projectile.velocity.X < vectorToPlayer.X)
                {
                    projectile.velocity.X = projectile.velocity.X + joinAccel;
                    if (projectile.velocity.X < 0f)
                    {
                        projectile.velocity.X = projectile.velocity.X + joinAccel * 1.5f;
                    }
                }
                if (projectile.velocity.X > vectorToPlayer.X)
                {
                    projectile.velocity.X = projectile.velocity.X - joinAccel;
                    if (projectile.velocity.X > 0f)
                    {
                        projectile.velocity.X = projectile.velocity.X - joinAccel * 1.5f;
                    }
                }
                if (projectile.velocity.Y < vectorToPlayer.Y)
                {
                    projectile.velocity.Y = projectile.velocity.Y + joinAccel;
                    if (projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y = projectile.velocity.Y + joinAccel * 1.5f;
                    }
                }
                if (projectile.velocity.Y > vectorToPlayer.Y)
                {
                    projectile.velocity.Y = projectile.velocity.Y - joinAccel;
                    if (projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y = projectile.velocity.Y - joinAccel * 1.5f;
                    }
                }
            }
        }
        private bool TeleportIfFarAway(Player player, float distance)
        {
            if (distance > 2000f)
            {
                projectile.Center = player.Center;
                return true;
            }
            return false;
        }

        private void ExecuteAttackState()
        {
            projectile.friendly = true;

            // Apply gravity
            projectile.velocity.Y = Math.Min(projectile.velocity.Y + 0.4f, 10f);

            // Count down and end attack state/animation
            aiAttackAnimation--;
            if (aiAttackAnimation <= 0)
            {
                aiAttackAnimation = 0;
                aiState = 0;
                projectile.friendly = false;
                projectile.netUpdate = true;
            }
        }

        private Vector2 SetGoalToTargetAndAttack(Vector2 goalVector, NPC target, Player player)
        {
            // Define chase range, reduced underground for reasons of space/visibility
            float maxChase = chaseRange;
            if (projectile.position.Y > Main.worldSurface * 16.0)
            { maxChase *= 0.7f; };

            // Chase if the enemy is visibly within chase distance
            Vector2 vectorToNPC = (target.Center - projectile.Center);
            float distance = vectorToNPC.Length();

            //// Check projectile can see the npc
            bool canHit = Collision.CanHit(
                projectile.position - new Vector2(0, 4),
                projectile.width, projectile.height,
                target.position, target.width, target.height);
            //// If can't see the npc, but I'm close to player, carry on.
            //!/ Main.NewText("<Fam> " + canHit + " & " + Vector2.Distance(projectile.Center, player.Center) + " < " + separatedDistance);
            if (!canHit)
            {
                // Too far away then go back to player otherwise carry on
                if (!CheckSeparated(player))
                {
                    canHit = true;
                }
            }

            if (distance < maxChase && canHit)
            {
                goalVector = target.Center;
                // Jump if the goal is too high
                JumpToReach(goalVector);
            }
            if (distance < attackRange)
            {
                aiState = 2;
                aiAttackAnimation = attackAnimationMax;
                projectile.netUpdate = true;
            }

            return goalVector;
        }
        private void JumpToReach(Vector2 goalVector)
        {
            float jumpDistance = goalVector.Y - projectile.Center.Y;
            if (projectile.velocity.Y == 0f) // Am grounded
            {
                if (jumpDistance < -10) projectile.velocity.Y = -6f;
                if (jumpDistance < -70) projectile.velocity.Y = -10f;
                if (jumpDistance < -120f) projectile.velocity.Y = -13f;
                if (jumpDistance < -210f) projectile.velocity.Y = -15f;
                if (jumpDistance < -270f) projectile.velocity.Y = -17f;
                if (jumpDistance < -310f) projectile.velocity.Y = -18f;
            }
        }

        private bool CheckSeparated(Player player)
        {
            float distance = Vector2.Distance(projectile.Center, player.Center);

            // Too far away, just teleport
            if (!TeleportIfFarAway(player, distance))
            {
                // Otherwise become seperated, or if flying
                if (distance > separatedDistance
                    || (Math.Abs(projectile.Center.Y - player.Center.Y) > 200f && aiState != 2)
                    /*|| player.rocketDelay2 > 0*/)
                {
                    aiState = 1;
                    projectile.netUpdate = true;
                    projectile.velocity.Y = 0f;
                    return true;
                }
            }
            return false;
        }

        private void ControlMovementPhysics(Player player, Vector2 goalVector)
        {
            // Calculate movement values
            float quickStop = quickStopAccel;
            float maxTopSpeed = Math.Max(topSpeed, Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y));
            if (maxTopSpeed > topSpeed) quickStop = quickStopFastAccel;

            // Used for checking for tiles in the way
            bool pathBlocked = false;

            float diffX = goalVector.X - projectile.Center.X;
            int dirMod = Math.Sign(diffX);
            // Over 5px away
            if (Math.Abs(diffX) > 5f)
            {
                // Moving backwards fast
                if (projectile.velocity.X * dirMod < topSpeed)
                {
                    projectile.velocity.X += quickStop * dirMod;
                }
                else // Not moving backwards fast, or moving forwards
                {
                    projectile.velocity.X += slowStopAccel * dirMod;
                }

                pathBlocked = CheckForPathBlocking(pathBlocked, dirMod);
                //if (pathBlocked) Main.NewText("<Fox> Path is blocked!");

                if (projectile.velocity.X > 5)
                    projectile.velocity.X = 5;
                if (projectile.velocity.X < -5)
                    projectile.velocity.X = -5;

            }
            else //Very close to goal
            {
                // Slowdown, then stop
                projectile.velocity.X *= 0.9f;
                if (Math.Abs(projectile.velocity.X) < quickStop * 2f)
                {
                    projectile.velocity.X = 0f;
                }
            }

            // Step up collision physics
            Collision.StepUp(ref projectile.position, ref projectile.velocity,
                projectile.width, projectile.height, ref projectile.stepSpeed,
                ref projectile.gfxOffY, 1, false, 0);

            // Jump over blocked path if grounded
            TryJumpingOverObstacle(pathBlocked, dirMod);

            // Clamp velocity X
            Math.Min(Math.Max(projectile.velocity.X, maxTopSpeed), -maxTopSpeed);

            // Set direction
            projectile.direction = Math.Sign(projectile.velocity.X);
            if (projectile.velocity.X * dirMod > quickStop * dirMod)
            {
                projectile.direction = dirMod;
            }
        }
        private bool CheckForPathBlocking(bool pathBlocked, int direction)
        {
            // Check ahead to see if there is a tile blocking me
            Point checkTile = Utils.ToTileCoordinates(projectile.Top);
            checkTile.X += direction + (int)projectile.velocity.X;
            for (int y = checkTile.Y; y < checkTile.Y + projectile.height / 16 + 1; y++)
            {
                if (WorldGen.SolidTile(checkTile.X, y))
                {
                    pathBlocked = true;
                    break;
                }
            }

            return pathBlocked;
        }
        private void TryJumpingOverObstacle(bool pathBlocked, int dirMod)
        {
            if (projectile.velocity.Y == 0f && pathBlocked)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 checkPoint = projectile.position;
                    checkPoint.X += i * projectile.width / 2;
                    checkPoint.X += dirMod * 8 + (int)projectile.velocity.X;
                    Point cTile = Utils.ToTileCoordinates(checkPoint);

                    /*
                    if (pathBlocked) Main.NewText("<Fox> Thinking of jumping!");
                    for (int wd = 0; wd < 12; wd++)
                    { Dust d = Main.dust[Dust.NewDust(cTile.ToVector2() * 16f, 16, 16, 16)];
                        d.noGravity = true;
                        d.velocity = Vector2.Zero;
                    }
                    */

                    // If I need to jump...
                    if (WorldGen.SolidTile(cTile.X, cTile.Y) ||
                        Main.tile[cTile.X, cTile.Y].halfBrick() ||
                        Main.tile[cTile.X, cTile.Y].slope() > 0 ||
                        (
                            TileID.Sets.Platforms[(int)Main.tile[cTile.X, cTile.Y].type] &&
                            Main.tile[cTile.X, cTile.Y].active() && !Main.tile[cTile.X, cTile.Y].inActive()
                            ))
                    {
                        try
                        {
                            cTile = Utils.ToTileCoordinates(projectile.Center);
                            cTile.X += dirMod + (int)projectile.velocity.X;
                            if (!WorldGen.SolidTile(cTile.X, cTile.Y - 1) && !WorldGen.SolidTile(cTile.X, cTile.Y - 2))
                            {
                                projectile.velocity.Y = -5.1f;
                            }
                            else if (!WorldGen.SolidTile(cTile.X, cTile.Y - 2))
                            {
                                projectile.velocity.Y = -7.1f;
                            }
                            else if (WorldGen.SolidTile(cTile.X, cTile.Y - 5))
                            {
                                projectile.velocity.Y = -11.1f;
                            }
                            else if (WorldGen.SolidTile(cTile.X, cTile.Y - 4))
                            {
                                projectile.velocity.Y = -10.1f;
                            }
                            else
                            {
                                projectile.velocity.Y = -9.1f;
                            }
                        }
                        catch
                        {
                            projectile.velocity.Y = -9.1f;
                        }
                    }
                }
            }
        }

        #endregion


        private void ManageFrames(ref int frame, ref int frameCounter)
        {
            // Flying
            if (aiState == 1)
            {
                // Face direction of velocity
                if (projectile.velocity.X != 0f)
                {
                    projectile.spriteDirection = Math.Sign(projectile.velocity.X);
                }
                // Rotate in travel direction
                projectile.rotation = projectile.velocity.X * 0.1f * flyRotationMod;

                if (projectile.frame < flyFrame || projectile.frame > flyFrame + flyFrameCount - 1) projectile.frame = flyFrame;

                projectile.frameCounter--;
                if (projectile.frameCounter <= 0 || projectile.frameCounter > flyFrameSpeed)
                {
                    projectile.frame++;
                    projectile.frameCounter = flyFrameSpeed;
                    if (projectile.frame > flyFrame + flyFrameCount - 1)
                    {
                        projectile.frame = flyFrame;
                    }
                }
            }

            // Attacking
            if (aiState == 2)
            {
                projectile.spriteDirection = projectile.direction;
                projectile.rotation = 0f;

                if (projectile.frame < attackFrame || projectile.frame > attackFrame + attackFrameCount - 1) projectile.frame = attackFrame;

                //progress from 0 to 1
                float progress = 1f - (float)aiAttackAnimation / attackAnimationMax;
                if (attackFrameCount > 0)
                {
                    float frameStep = 1f / attackFrameCount;
                    projectile.frame = attackFrame + (int)(progress / frameStep);
                }
            }

            // Movement + Falling
            if (aiState == 0)
            {
                projectile.spriteDirection = projectile.direction;
                projectile.rotation = 0f;

                // Falling, take into account weird slope behaviour
                if (projectile.velocity.Y != 0f && (projectile.oldVelocity.Y != 0f && projectile.velocity.Y != 0.4f))
                {
                    if (projectile.frame < fallFrame || projectile.frame > fallFrame + fallFrameCount - 1) projectile.frame = fallFrame;

                    projectile.frameCounter--;
                    if (projectile.frameCounter <= 0 || projectile.frameCounter > fallFrameSpeed)
                    {
                        projectile.frame++;
                        projectile.frameCounter = fallFrameSpeed;
                        if (projectile.frame > fallFrame + fallFrameCount - 1)
                        {
                            projectile.frame = fallFrame;
                        }
                    }
                }
                else
                {
                    if (projectile.velocity.X == 0f)
                    {
                        if (projectile.frame < idleFrame || projectile.frame > idleFrame + idleFrameCount - 1) projectile.frame = idleFrame;

                        projectile.frame = idleFrame;
                    }
                    else
                    {
                        if (projectile.frame < runFrame || projectile.frame > runFrame + runFrameCount - 1) projectile.frame = runFrame;

                        projectile.frameCounter -= (int)Math.Min(
                            512f,
                            512f * Math.Abs(projectile.velocity.X * runFrameSpeedMod));
                        if (projectile.frameCounter <= 0f)
                        {
                            projectile.frame++;
                            projectile.frameCounter = 1024;
                            if (projectile.frame > runFrame + runFrameCount - 1)
                            {
                                projectile.frame = runFrame;
                            }
                        }
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
    }
}
