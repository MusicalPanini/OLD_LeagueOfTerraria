using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class TheFallenCelestialsDarkMagic_SoulShackles : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Shackles");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.penetrate = 3;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.timeLeft = 210;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            NPC npc = Main.npc[(int)projectile.ai[0]];
            Player player = Main.player[projectile.owner];

            if (!npc.active || projectile.Distance(player.Center) > Items.Weapons.Abilities.SoulShackles.range)
            {
                ChainBreak(player.Center);
                projectile.Kill();
            }
            else
            {
                if (projectile.timeLeft < 180)
                {
                    if ((int)projectile.ai[1] == 0)
                    {
                        projectile.friendly = true;
                    }
                    projectile.Center = npc.Center;

                    Dust dust = Dust.NewDustDirect(new Vector2(npc.position.X, npc.Bottom.Y - npc.height / 4f), npc.width, npc.height / 4, 248, 0, -2, 0, new Color(159, 0, 255), 1.5f);
                    dust.noGravity = true;

                    if (projectile.timeLeft == 1)
                    {
                        projectile.friendly = true;
                        DustChain(player, (int)projectile.Distance(player.Center) / 4, 2f);

                        for (int i = 0; i < 20; i++)
                        {
                            dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 248, 0, 0, 0, new Color(159, 0, 255), 1.5f);
                            dust.noGravity = true;
                        }

                        TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 54, -0.5f);
                        var sound = TerraLeague.PlaySoundWithPitch(projectile.Center, 3, 4, -1f);
                        if (sound != null)
                            sound.Volume = sound.Volume / 3f;
                    }

                    if (projectile.soundDelay == 0)
                    {
                        projectile.soundDelay = 25;
                        TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 15, 0.5f - (projectile.timeLeft / 180f));
                    }
                }
                else
                {
                    Vector2 npcPos = npc.Center;
                    Vector2 playerPos = player.MountedCenter;

                    float projectileX = ((npcPos.X - playerPos.X) * (-(projectile.timeLeft - 210) / 30f)) + playerPos.X;
                    float projectileY = ((npcPos.Y - playerPos.Y) * (-(projectile.timeLeft - 210) / 30f)) + playerPos.Y;

                    projectile.Center = new Vector2(projectileX, projectileY);
                }
            }

            float timepassed = ((210 - projectile.timeLeft) / 210f);
            DustChain(player, (int)(projectile.Distance(player.Center) * (1 + timepassed)) / 64, 1 + (timepassed * 0.5f));
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if ((int)projectile.ai[1] == 0)
            {
                projectile.ai[1] = 1;
                projectile.friendly = false;
                target.AddBuff(BuffType<Slowed>(), 180);
            }
            else
            {
                target.AddBuff(BuffType<Stunned>(), 240);
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        void DustChain(Player player, int loops, float scale)
        {
            Vector2 ChainLine = projectile.position - player.Center;
            ChainLine.Normalize();

            for (int i = 0; i < loops; i++)
            {
                int distance = Main.rand.Next((int)projectile.Distance(player.Center));
                Vector2 dustPoint = ChainLine * distance;

                Dust dust = Dust.NewDustDirect(dustPoint + player.Center, 1, 1, 248, 0, 0, 100, new Color(159, 0, 255), scale);
                dust.noGravity = true;
            }
        }

        public void ChainBreak(Vector2 source)
        {
            Vector2 ChainLine = projectile.position - source;
            ChainLine.Normalize();
            int links = (int)projectile.Distance(source) / 32;

            for (int i = 0; i < links; i++)
            {
                int distance = 32 * i;
                Vector2 gorePoint = ChainLine * distance;

                int gore = Gore.NewGore(gorePoint + source, Vector2.Zero, mod.GetGoreSlot("Gores/SoulShackleGoreA"), 1f);

                Main.gore[gore].timeLeft /= 10;

                gorePoint = ChainLine * (distance + 16);
                gore = Gore.NewGore(gorePoint + source, Vector2.Zero, mod.GetGoreSlot("Gores/SoulShackleGoreB"), 1f);
                Main.gore[gore].timeLeft /= 15;
            }
        }

        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[0] == target.whoAmI && projectile.friendly)
                return true;
            else
                return false;
        }


        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/TheFallenCelestialsDarkMagic_SoulShackleChain");

            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            mountedCenter.Y += 4;
            Rectangle? sourceRectangle = new Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    Lighting.AddLight(position, 178 / 255f, 0, 1);
                    vector2_4 = mountedCenter - position;
                    Color color2 = Color.White;
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }

            texture = mod.GetTexture("Projectiles/TheFallenCelestialsDarkMagic_SoulShackleBorder");
            origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            Vector2 BorderVector =  position - mountedCenter;
            BorderVector.Normalize();
            BorderVector = BorderVector * Items.Weapons.Abilities.SoulShackles.range;
            BorderVector += mountedCenter;
            Main.spriteBatch.Draw(texture, BorderVector - Main.screenPosition, sourceRectangle, Color.White, rotation + (float)Math.PI/2f, origin, 1f, SpriteEffects.None, 0.0f);

            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
