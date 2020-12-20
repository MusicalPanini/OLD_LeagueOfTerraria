using Microsoft.Xna.Framework;
using TerraLeague.Items;
using Terraria;
using Terraria.ID;
using TerraLeague.Gores;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Buffs;
using TerraLeague.Items.BossBags;

namespace TerraLeague.NPCs
{
    public class TargonSigil : ModNPC
    {
        int rerolls = 3;
        int currentBlessing = Main.rand.Next(0, 8);

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
            npc.dontTakeDamageFromHostiles = true;
            npc.dontCountMe = true;
            bossBag = ItemType<TargonBossBag>();
        }

        public override bool PreAI()
        {
            npc.homeless = true;
            return base.PreAI();
        }

        public override void AI()
        {
            if (NPC.CountNPCS(NPCType<TargonSigil>()) > 1)
                npc.active = false;

            if (Main.dayTime && Main.time == 0)
                rerolls = 3;

            if (Main.rand.Next(20) == 0)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 261, 0f, 0f, 150, new Color(0, 150, 255), 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.fadeIn = 1f;
            }

            npc.spriteDirection = -1;
            npc.Center = new Vector2(TerraLeagueWORLDGLOBAL.TargonCenterX * 16, 45 * 16);
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
            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.CountNPCS(NPCType<NPCs.TargonSigil>()) == 0)
                NPC.NewNPC(TerraLeagueWORLDGLOBAL.TargonCenterX * 16, 45 * 16, NPCType<NPCs.TargonSigil>());
            base.NPCLoot();
        }

        public override string GetChat()
        {
            string text = "From the greater beyond you can hear whispers in a language you do not know, but strangly can understand.";
            if (!TerraLeagueWORLDGLOBAL.TargonUnlocked)
            {
                return text + "\n\nYou are not worthy of their challenge just yet.";
            }
            else if (!TerraLeagueWORLDGLOBAL.TargonArenaDefeated)
            {
                if (NPC.CountNPCS(NPCType<TargonBoss>()) <= 0)
                {
                    return text + "\n\nThe whispers offer you a challenge with rewards of strength." +
                        "\n\n(Accepting the challenge will teleport all players into the Targon Arena)";
                }
                else
                {
                    return text + "\n\nThe challenge is currently in progress.";
                }
            }
            else
            {
                string tip = Lang.GetBuffDescription(GetBuffID());
                int rare = 0;
                GetModBuff(GetBuffID()).ModifyBuffTip(ref tip, ref rare);
                text = "From the greater beyond you can hear whispers in a language you do not know, but strangly can understand.";
                if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown <= 0)
                {
                    return text + "\n\nThe whipsers offer: The " + Lang.GetBuffName(GetBuffID())
                    + "\n" + tip;
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
            if (TerraLeagueWORLDGLOBAL.TargonUnlocked && !TerraLeagueWORLDGLOBAL.TargonArenaDefeated)
            {
                if (NPC.CountNPCS(NPCType<TargonBoss>()) <= 0)
                    button = "Accept Challenge";
            }
            else if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown <= 0)
            {
                button = "Receive Blessing";
                button2 = "Reroll Blessing (" + rerolls + " remaining)";
            }
            else
            {
                string seconds = "" + Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown % 3600 / 60;
                seconds = seconds.Length == 1 ? "0" + seconds : seconds;
                button = "Time Remaining: " + Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown / 3600 + ":" + seconds;
                button2 = "Challenge Arena Again";
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (NPC.downedBoss1 && !TerraLeagueWORLDGLOBAL.TargonArenaDefeated)
            {
                if (firstButton)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC((int)npc.position.X, (int)npc.position.Y + 64, NPCType<TargonBoss>());
                    }
                    else
                    {
                        npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().PacketHandler.SendSpawnNPC(-1, Main.LocalPlayer.whoAmI, NPCType<TargonBoss>(), new Vector2((int)npc.position.X, (float)(Main.worldSurface * 16) + 64));
                    }
                }
            }
            else if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown <= 0)
            {
                if (firstButton)
                {
                    shop = false;
                    string text = "You gained the " + Lang.GetBuffName(GetBuffID());
                    int buffTime = 60 * 60 * 60; // Frames * seconds * minutes * gameDays = 60 MINUTES
                    Main.LocalPlayer.AddBuff(GetBuffID(), buffTime);
                    TerraLeague.PlaySoundWithPitch(npc.Center, 2, 29, -1);
                    Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().blessingCooldown = 60 * 60 * 120;
                    Main.npcChatText = text;
                    GetRandomBlessing();
                }
                else
                {
                    if (rerolls > 0)
                    {
                        GetRandomBlessing();
                        rerolls--;
                        Main.npcChatText = GetChat();
                    }
                    else
                    {
                        Main.npcChatText = GetChat() + "\n\nCome back tommorrow for more rerolls";
                    }
                }
            }
            else
            {
                if (!firstButton)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        NPC.NewNPC((int)npc.position.X, (int)npc.position.Y + 64, NPCType<TargonBoss>());
                    }
                    else
                    {
                        npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().PacketHandler.SendSpawnNPC(-1, Main.LocalPlayer.whoAmI, NPCType<TargonBoss>(), new Vector2((int)npc.position.X, (float)(Main.worldSurface * 16) + 64));
                    }
                }
                else
                {
                    shop = false;
                    Main.npcChatText = GetChat();
                }
            }
        }

        int GetBuffID()
        {
            switch (currentBlessing)
            {
                case 0:
                    return BuffType<SunBlessing>();
                case 1:
                    return BuffType<MoonBlessing>();
                case 2:
                    return BuffType<StarBlessing>();
                case 3:
                    return BuffType<WarBlessing>();
                case 4:
                    return BuffType<ProtectorBlessing>();
                case 5:
                    return BuffType<TwilightBlessing>();
                case 6:
                    return BuffType<TravelerBlessing>();
                case 7:
                    return BuffType<JudicatorBlessing>();
                case 8:
                    return BuffType<ChargerBlessing>();
                case 9:
                    return BuffType<DestroyerBlessing>();
                case 10:
                    return BuffType<GreatBeyondBlessing>();
                case 11:
                    return BuffType<ImmortalFireBlessing>();
                case 12:
                    return BuffType<MessengerBlessing>();
                case 13:
                    return BuffType<ScourgeBlessing>();
                case 14:
                    return BuffType<SerpentBlessing>();
                case 15:
                    return BuffType<WandererBlessing>();
                default:
                    return BuffType<SunBlessing>();
            }
        }

        void GetRandomBlessing()
        {
            int newBuff = 0;
            while (true)
            {
                newBuff = Main.rand.Next(0, 16);
                if (newBuff != currentBlessing)
                {
                    currentBlessing = newBuff;
                    break;
                }
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return false;
        }

        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            return false;
        }
    }
}
