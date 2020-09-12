using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Buffs;

namespace TerraLeague.NPCs
{
    public class TargonSigil : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Targon's Peak");
        }
        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.lifeMax = 400;
            npc.defense = 0;
            npc.damage = 0;
            npc.width = 80;
            npc.height = 100;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 0f;
            npc.npcSlots = 0f;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
            npc.netAlways = true;
        }

        public override bool PreAI()
        {
            return base.PreAI();
        }

        public override void AI()
        {
            if (Main.rand.Next(20) == 0)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 261, 0f, 0f, 150, new Color(0, 150, 255), 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.fadeIn = 1f;
            }

            npc.spriteDirection = -1;
            npc.Center = new Vector2(WORLDGLOBAL.TargonCenterX*16, 45*16);
            npc.position.Y += (float)System.Math.Sin(Main.time * 0.1);

            Lighting.AddLight(npc.Center, 0, 0.3f, 1f);
            base.AI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            base.OnHitPlayer(target, damage, crit);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
           
            base.HitEffect(hitDirection, damage);
        }

        public override void NPCLoot()
        {
            NPC.NewNPC(WORLDGLOBAL.TargonCenterX * 16, 45 * 16, NPCType<NPCs.TargonSigil>());
            base.NPCLoot();
        }

        public override string GetChat()
        {
            string text = "From the greater beyond you can hear whispers in a language you do not know, but strangly can understand.";
            if (!NPC.downedBoss1)
            {
                return text + "\n\nThe wispers tell you that you are yet to impress them. Defeat the Eye of Cthulhu to gain their favor.";
            }
            else
            {
                text = "From the greater beyond you can hear whispers in a language you do not know, but strangly can understand.";
                if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown <= 0)
                {
                    return text += "\n\nThe whipsers offer you a blessing.";
                }
                else
                {
                    return text += "\n\nThe whipsers tell you it is not the right time." +
                        "\nThey will inform you when the next blessing is ready.";
                }
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown <= 0)
            {
                button = "Receive Blessing";
            }
            else
            {
                button2 = "Time Remaining: " + Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown % 216000 / 3600 + ":" + Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown % 3600 / 60;
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = false;
                string text;
                int buffTime = 60 * 60 * 60; // Frames * seconds * minutes * gameDays
                switch (Main.rand.Next(8))
                {
                    case 0:
                        Main.LocalPlayer.AddBuff(BuffType<SunBlessing>(), buffTime);
                        text = "You gained the Blessing of The Golden Sister";
                        break;
                    case 1:
                        Main.LocalPlayer.AddBuff(BuffType<MoonBlessing>(), buffTime);
                        text = "You gained the Blessing of The Silver Sister";
                        break;
                    case 2:
                        Main.LocalPlayer.AddBuff(BuffType<StarBlessing>(), buffTime);
                        text = "You gained the Blessing of The Star Child";
                        break;
                    case 3:
                        Main.LocalPlayer.AddBuff(BuffType<WarBlessing>(), buffTime);
                        text = "You gained the Blessing of The Warrior";
                        break;
                    case 4:
                        Main.LocalPlayer.AddBuff(BuffType<ProtectorBlessing>(), buffTime);
                        text = "You gained the Blessing of The Protector";
                        break;
                    case 5:
                        Main.LocalPlayer.AddBuff(BuffType<TwilightBlessing>(), buffTime);
                        text = "You gained the Blessing of The Trickster";
                        break;
                    case 6:
                        Main.LocalPlayer.AddBuff(BuffType<TravelerBlessing>(), buffTime);
                        text = "You gained the Blessing of The Traveler";
                        break;
                    case 7:
                        Main.LocalPlayer.AddBuff(BuffType<JudicatorBlessing>(), buffTime);
                        text = "You gained the Blessing of The Judicator";
                        break;
                    default:
                        text = "You gained nothing... h o w ?";
                        break;
                }
                TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, -1);
                Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown = 60 * 60 * 24 * 7;
                Main.npcChatText = text;
            }
            else
            {
                shop = false;
                Main.npcChatText = GetChat();
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Texture2D texture = mod.GetTexture("NPCs/TargonSigil_Glow");
            //spriteBatch.Draw
            //(
            //    texture,
            //    new Vector2
            //    (
            //        npc.position.X - Main.screenPosition.X + npc.width * 0.5f,
            //        npc.position.Y - Main.screenPosition.Y + npc.height * 0.5f
            //    ),
            //    new Rectangle(0, 0, texture.Width, texture.Height),
            //    Color.White,
            //    npc.rotation,
            //    new Vector2(texture.Width, texture.Width) * 0.5f,
            //    npc.scale,
            //    SpriteEffects.None,
            //    0f
            //);
        }
    }
}
