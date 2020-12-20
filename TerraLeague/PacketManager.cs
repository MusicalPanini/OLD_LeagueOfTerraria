using Microsoft.Xna.Framework;
using System.IO;
using TerraLeague.Buffs;
using TerraLeague.Items.CustomItems.Actives;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.SummonerSpells;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.Weapons;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using Terraria.Localization;
using TerraLeague.UI;

namespace TerraLeague
{
    internal abstract class PacketHandler
    {
        internal byte HandlerType { get; set; }

        public abstract void HandlePacket(BinaryReader reader, int fromWho);

        protected PacketHandler(byte handlerType)
        {
            HandlerType = handlerType;
        }

        protected ModPacket GetPacket(byte packetType, int fromWho)
        {
            var p = TerraLeague.instance.GetPacket();
            p.Write((byte)HandlerType);
            p.Write((byte)packetType);
            return p;
        }
    }

    internal class ModNetHandler
    {
        public const byte Player = 1;
        public const byte NPC = 2;
        public const byte Projectile = 3;
        public const byte Passive = 4;
        public const byte Active = 5;
        public const byte SummonerSpells = 6;
        public const byte Abilities = 7;
        public const byte World = 8;
        internal static PlayerPacketHandler playerHandler = new PlayerPacketHandler(Player);
        internal static NPCPacketHandler npcHandler = new NPCPacketHandler(NPC);
        internal static ProjectilePacketHandler projectileHandler = new ProjectilePacketHandler(Projectile);
        internal static PassivePacketHandler passiveHandler = new PassivePacketHandler(Passive);
        internal static ActivePacketHandler activeHandler = new ActivePacketHandler(Active);
        internal static SummonerSpellsPacketHandler summonerSpellHandler = new SummonerSpellsPacketHandler(SummonerSpells);
        internal static AbilitiesPacketHandler abilitiesHandler = new AbilitiesPacketHandler(Abilities);
        internal static WorldPacketHandler worldHandler = new WorldPacketHandler(World);
        public static void HandlePacket(BinaryReader r, int fromWho)
        {
            switch (r.ReadByte())
            {
                case Player:
                    playerHandler.HandlePacket(r, fromWho);
                    break;
                case NPC:
                    npcHandler.HandlePacket(r, fromWho);
                    break;
                case Projectile:
                    projectileHandler.HandlePacket(r, fromWho);
                    break;
                case Passive:
                    passiveHandler.HandlePacket(r, fromWho);
                    break;
                case Active:
                    activeHandler.HandlePacket(r, fromWho);
                    break;
                case SummonerSpells:
                    summonerSpellHandler.HandlePacket(r, fromWho);
                    break;
                case Abilities:
                    abilitiesHandler.HandlePacket(r, fromWho);
                    break;
                case World:
                    worldHandler.HandlePacket(r, fromWho);
                    break;
            }
        }
    }

    internal class PlayerPacketHandler : PacketHandler
    {
        #region Values
        public const byte EquipStats = 1;
        public const byte Healing = 2;
        public const byte Manaing = 3;
        public const byte Buff = 4;
        public const byte ShieldBreak = 5;
        public const byte Shield = 6;
        public const byte ShieldTotal = 7;
        public const byte Ascension = 8;
        public const byte NewShield = 9;
        public const byte Biome = 10;
        public const byte NPCRetarget = 11;
        public const byte Shatter = 12;
        public const byte CausticWounds = 13;

        public const byte Stoneplate = 50;
        #endregion

        public PlayerPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (EquipStats):
                    ReceiveEquipData(reader, fromWho);
                    break;
                case (Healing):
                    ReceiveHealing(reader, fromWho);
                    break;
                case (Manaing):
                    ReceiveMana(reader, fromWho);
                    break;
                case (Buff):
                    ReceiveBuff(reader, fromWho);
                    break;
                case (Shield):
                    ReceiveShield(reader, fromWho);
                    break;
                case (ShieldTotal):
                    ReceiveShieldTotal(reader, fromWho);
                    break;
                case (NewShield):
                    ReceiveNewShield(reader, fromWho);
                    break;
                case (Ascension):
                    ReceiveAscension(reader, fromWho);
                    break;
                case (Biome):
                    ReceiveBiome(reader, fromWho);
                    break;
                case (NPCRetarget):
                    ReceiveRetarget(reader, fromWho);
                    break;
                case (Shatter):
                    ReceiveShatterEFX(reader, fromWho);
                    break;
                case (CausticWounds):
                    ReceiveCausticEFX(reader, fromWho);
                    break;
            }
        }

        // Equip Stats
        public void SendEquipData(int toWho, int fromWho, double stat, int statSlot)
        {
            ModPacket packet = GetPacket(EquipStats, fromWho);

            packet.Write(stat);
            packet.Write(statSlot);

            packet.Send(toWho, fromWho);
            TerraLeague.Log("[DEBUG] - Sending Stat (Slot: " + statSlot + " | Stat: " + stat + ")", new Color(100, 200, 100));
        }
        public void ReceiveEquipData(BinaryReader reader, int fromWho)
        {
            double stat = reader.ReadDouble();
            int statSlot = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendEquipData(-1, fromWho, stat, statSlot);
            }
            else
            {
                Player player = Main.player[fromWho];
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                modPlayer.accessoryStat[statSlot] = stat;

                TerraLeague.Log("[DEBUG] - Received Stat from " + player.name + " (Slot: " + statSlot + " | Stat: " + stat + ")", new Color(100, 200, 100));
            }
        }

        // Healing
        public void SendHealing(int toWho, int fromWho, int healAmount, int healTarget)
        {
            ModPacket packet = GetPacket(Healing, fromWho);
            packet.Write(healAmount);
            packet.Write(healTarget);
            packet.Send(toWho, fromWho);
            TerraLeague.Log("[DEBUG] - Sending Healing (" + healAmount + ")", new Color(0, 200, 100));
        }
        private void ReceiveHealing(BinaryReader reader, int fromWho)
        {
            int healAmount = reader.ReadInt32();
            int healTarget = reader.ReadInt32();
            TerraLeague.Log("[DEBUG] - Received Healing (" + healAmount + ")", new Color(200, 0, 100));

            if (Main.netMode == NetmodeID.Server)
            {
                SendHealing(-1, fromWho, healAmount, healTarget);
            }
            else
            {
                if (healTarget == 256)
                    Main.player[1].GetModPlayer<PLAYERGLOBAL>().lifeToHeal += healAmount;
                else
                    Main.player[healTarget].GetModPlayer<PLAYERGLOBAL>().lifeToHeal += healAmount;
            }
        }

        // Mana
        public void SendMana(int toWho, int fromWho, int manaAmount, int manaTarget)
        {
            ModPacket packet = GetPacket(Manaing, fromWho);
            packet.Write(manaAmount);
            packet.Write(manaTarget);
            packet.Send(toWho, fromWho);
            TerraLeague.Log("[DEBUG] - Sending Mana (" + manaAmount + ")", new Color(0, 0, 200));
        }
        private void ReceiveMana(BinaryReader reader, int fromWho)
        {
            int manaAmount = reader.ReadInt32();
            int manaTarget = reader.ReadInt32();
            TerraLeague.Log("[DEBUG] - Received Mana (" + manaAmount + ")", new Color(0, 0, 100));

            if (Main.netMode == NetmodeID.Server)
            {
                SendMana(-1, fromWho, manaAmount, manaTarget);
            }
            else
            {
                Main.player[manaTarget].ManaEffect(manaAmount);
                Main.player[manaTarget].statMana += manaAmount;
            }
        }

        // Buffs
        public void SendBuff(int toWho, int fromWho, int BuffId, int BuffDuration, int BuffTarget)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Buff, fromWho);
                packet.Write(BuffId);
                packet.Write(BuffDuration);
                packet.Write(BuffTarget);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Buff (ID: " + BuffId + " | Duration: " + BuffDuration + ")", Color.Orange);
            }
        }
        private void ReceiveBuff(BinaryReader reader, int fromWho)
        {
            int BuffId = reader.ReadInt32();
            int BuffDuration = reader.ReadInt32();
            int BuffTarget = reader.ReadInt32();
            TerraLeague.Log("[DEBUG] - Received Buff (ID: " + BuffId + " | Duration: " + BuffDuration + ")", new Color(255, 240, 245));

            if (Main.netMode == NetmodeID.Server)
            {
                SendBuff(-1, fromWho, BuffId, BuffDuration, BuffTarget);
            }
            else
            {
                Main.player[BuffTarget].AddBuff(BuffId, BuffDuration);
            }
        }

        // Shield Information
        public void SendShield(int toWho, int fromWho, int shieldAmount, int type, int shieldDuration, int shieldTarget, Color shieldColor)
        {
            ModPacket packet = GetPacket(Shield, fromWho);
            packet.Write(shieldAmount);
            packet.Write(type);
            packet.Write(shieldDuration);
            packet.Write(shieldTarget);
            packet.WriteRGB(shieldColor);
            packet.Send(toWho, fromWho);
            TerraLeague.Log("[DEBUG] - Sending Shield (" + shieldAmount + ")", shieldColor);
        }
        private void ReceiveShield(BinaryReader reader, int fromWho)
        {
            int shieldAmount = reader.ReadInt32();
            int type = reader.ReadInt32();
            int shieldDuration = reader.ReadInt32();
            int shieldTarget = reader.ReadInt32();
            Color shieldColor = reader.ReadRGB();

            TerraLeague.Log("[DEBUG] - Received Shield (" + shieldAmount + ")", shieldColor);

            if (Main.netMode == NetmodeID.Server)
            {
                SendShield(-1, fromWho, shieldAmount, type, shieldDuration, shieldTarget, shieldColor);
            }
            else
            {
                if (shieldTarget == 256)
                    Main.player[1].GetModPlayer<PLAYERGLOBAL>().AddShield(shieldAmount, shieldDuration, shieldColor, (ShieldType)type);
                else
                    Main.player[shieldTarget].GetModPlayer<PLAYERGLOBAL>().AddShield(shieldAmount, shieldDuration, shieldColor, (ShieldType)type);
            }
        }

        // Total Shield
        public void SendShieldTotal(int toWho, int fromWho, int user, int value, int shieldType)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(ShieldTotal, fromWho);
                packet.Write(user);
                packet.Write(value);
                packet.Write(shieldType);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending ShieldTotal (" + value + ")", Color.LightSlateGray);
            }
        }
        private void ReceiveShieldTotal(BinaryReader reader, int fromWho)
        {
            int user = reader.ReadInt32();
            int value = reader.ReadInt32();
            int shieldType = reader.ReadInt32();
            TerraLeague.Log("[DEBUG] - Received ShieldTotal (" + value + ")", new Color(80, 80, 80));
            if (Main.netMode == NetmodeID.Server)
            {
                SendShieldTotal(-1, fromWho, user, value, shieldType);
            }
            else
            {
                if (shieldType == 0)
                    Main.player[user].GetModPlayer<PLAYERGLOBAL>().NormalShield = value;
                else if (shieldType == 1)
                    Main.player[user].GetModPlayer<PLAYERGLOBAL>().MagicShield = value;
                else if (shieldType == 2)
                    Main.player[user].GetModPlayer<PLAYERGLOBAL>().PhysicalShield = value;
            }
        }

        // New Shield
        public void SendNewShield(int toWho, int fromWho, int user, Color color)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(NewShield, fromWho);
                packet.Write(user);
                packet.Write(color.R);
                packet.Write(color.G);
                packet.Write(color.B);
                packet.Write(color.A);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending New Shield", color);
            }
        }
        private void ReceiveNewShield(BinaryReader reader, int fromWho)
        {
            int user = reader.ReadInt32();
            int R = reader.ReadByte();
            int G = reader.ReadByte();
            int B = reader.ReadByte();
            int A = reader.ReadByte();
            TerraLeague.Log("[DEBUG] - Received New Shield", new Color(R, G, B, A));
            if (Main.netMode == NetmodeID.Server)
            {
                SendNewShield(-1, fromWho, user, new Color(R,G,B,A));
            }
            else
            {
                Main.player[user].GetModPlayer<PLAYERGLOBAL>().currentShieldColor = new Color(R, G, B, A);
            }
        }

        //Ascension
        public void SendAscension(int toWho, int fromWho, int user, int value)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Ascension, fromWho);
                packet.Write(user);
                packet.Write(value);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Ascension (" + value + ")", Color.LightSlateGray);
            }
        }
        private void ReceiveAscension(BinaryReader reader, int fromWho)
        {
            int user = reader.ReadInt32();
            int value = reader.ReadInt32();
            int shieldType = reader.ReadInt32();
            TerraLeague.Log("[DEBUG] - Received Ascension (" + value + ")", new Color(80, 80, 0));
            if (Main.netMode == NetmodeID.Server)
            {
                SendAscension(-1, fromWho, user, value);
            }
            else
            {
                Main.player[user].GetModPlayer<PLAYERGLOBAL>().AscensionStacks = value;
            }
        }

        //Biome Check
        public void SendBiome(int toWho, int fromWho, int player, int biome, bool isActive)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Biome, fromWho);
                packet.Write(player);
                packet.Write(biome);
                packet.Write(isActive);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Biome", Color.LightSlateGray);
            }
        }
        private void ReceiveBiome(BinaryReader reader, int fromWho)
        {
            int player = reader.ReadInt32();
            int biome = reader.ReadInt32();
            bool isActive = reader.ReadBoolean();
            TerraLeague.Log("[DEBUG] - Received Ascension", new Color(80, 80, 0));
            if (Main.netMode == NetmodeID.Server)
            {
                switch (biome)
                {
                    case 0:
                        Main.player[player].GetModPlayer<PLAYERGLOBAL>().zoneSurfaceMarble = isActive;
                        break;
                    case 1:
                        Main.player[player].GetModPlayer<PLAYERGLOBAL>().zoneBlackMist = isActive;
                        break;
                    default:
                        break;
                }

                //SendAscension(-1, fromWho, user, value);
            }
            //else
            //{
            //    Main.player[user].GetModPlayer<PLAYERGLOBAL>().AscensionStacks = value;
            //}
        }

        // Retarget
        public void SendRetarget(int toWho, int fromWho, int player)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(NPCRetarget, fromWho);
                packet.Write(player);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Retarget", Color.LightSlateGray);
            }
        }
        private void ReceiveRetarget(BinaryReader reader, int fromWho)
        {
            int player = reader.ReadInt32();
            TerraLeague.Log("[DEBUG] - Received Retarget", new Color(80, 80, 0));
            if (Main.netMode == NetmodeID.Server)
            {
                TerraLeague.ForceNPCStoRetarget(Main.player[player]);
            }
        }

        public void SendShatterEFX(int toWho, int fromWho, int target)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Shatter, fromWho);
                packet.Write(target);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Shatter", Color.LightSlateGray);
            }
        }
        private void ReceiveShatterEFX(BinaryReader reader, int fromWho)
        {
            int target = reader.ReadInt32();
            TerraLeague.Log("[DEBUG] - Received Shatter", new Color(80, 80, 0));
            if (Main.netMode == NetmodeID.Server)
            {
                SendShatterEFX(-1, fromWho, target);
            }
            else
            {
                Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().CausticWoundsEffect(Main.npc[target]);
            }
        }

        public void SendCausticEFX(int toWho, int fromWho, int target)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(CausticWounds, fromWho);
                packet.Write(target);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Caustic", Color.LightSlateGray);
            }
        }
        private void ReceiveCausticEFX(BinaryReader reader, int fromWho)
        {
            int target = reader.ReadInt32();
            TerraLeague.Log("[DEBUG] - Received Caustic", new Color(80, 80, 0));
            if (Main.netMode == NetmodeID.Server)
            {
                SendCausticEFX(-1, fromWho, target);
            }
            else
            {
                Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().CausticWoundsEffect(Main.npc[target]);
            }
        }
    }

    internal class NPCPacketHandler : PacketHandler
    {
        #region Values
        public const byte BattleText = 1;
        public const byte RemoveBuff = 2;
        public const byte SyncStats = 3;
        public const byte CreateVessel = 4;
        public const byte SyncVessel = 5;
        public const byte AddBuff = 6;
        public const byte SpawnNpcOnServer = 7;
        #endregion

        public NPCPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (BattleText):
                    ReceiveBattleText(reader, fromWho);
                    break;
                case (AddBuff):
                    ReceiveAddBuff(reader, fromWho);
                    break;
                case (RemoveBuff):
                    ReceiveRemoveBuff(reader, fromWho);
                    break;
                case (SyncStats):
                    ReceiveSyncStats(reader, fromWho);
                    break;
                case (CreateVessel):
                    ReceiveCreateVessel(reader, fromWho);
                    break;
                case (SyncVessel):
                    ReceiveSyncVessel(reader, fromWho);
                    break;
                case (SpawnNpcOnServer):
                    ReceiveSpawnNPC(reader, fromWho);
                    break;
            }
        }

        public void SendBattleText(int toWho, int fromWho, int text, int target, bool crit)
        {
            ModPacket packet = GetPacket(BattleText, fromWho);
            packet.Write(text);
            packet.Write(target);
            packet.Write(crit);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveBattleText(BinaryReader reader, int fromWho)
        {
            int text = reader.ReadInt32();
            int target = reader.ReadInt32();
            bool crit = reader.ReadBoolean();

            if (Main.netMode == NetmodeID.Server)
            {
                SendBattleText(-1, fromWho, text, target, crit);
                NPC npc = Main.npc[target];

                if (!npc.immortal)
                {
                    if (npc.realLife >= 0)
                    {
                        Main.npc[npc.realLife].life -= (int)text;
                        npc.life = Main.npc[npc.realLife].life;
                        npc.lifeMax = Main.npc[npc.realLife].lifeMax;
                    }
                    else
                    {
                        npc.life -= (int)text;
                    }
                }
                if (npc.realLife >= 0)
                {
                    Main.npc[npc.realLife].checkDead();
                }
                else
                {
                    npc.checkDead();
                }
            }
            else
            {
                NPC npc = Main.npc[target];
                Color color = (crit ? new Color(128, 0, 128, 180) : new Color(147, 112, 219, 180));

                if (!npc.immortal)
                {
                    if (npc.realLife >= 0)
                    {
                        Main.npc[npc.realLife].life -= (int)text;
                        npc.life = Main.npc[npc.realLife].life;
                        npc.lifeMax = Main.npc[npc.realLife].lifeMax;
                    }
                    else
                    {
                        npc.life -= (int)text;
                    }
                }
                if (npc.realLife >= 0)
                {
                    Main.npc[npc.realLife].checkDead();
                }
                else
                {
                    npc.checkDead();
                }
                CombatText.NewText(new Rectangle((int)npc.position.X + 32, (int)npc.position.Y, npc.width, npc.height), color, text, false, false);
            }
        }

        public void SendAddBuff(int toWho, int fromWho, int buffType, int buffTime, int target)
        {
            ModPacket packet = GetPacket(AddBuff, fromWho);
            packet.Write(buffType);
            packet.Write(target);
            packet.Send(toWho, fromWho);
            TerraLeague.Log("[DEBUG] - Sending Add Buff ID " + buffType + " from NPC " + target, Color.White);
        }

        public void ReceiveAddBuff(BinaryReader reader, int fromWho)
        {
            int buffType = reader.ReadInt32();
            int buffTime = reader.ReadInt32();
            int target = reader.ReadInt32();
            NPC npc = Main.npc[target];

            if (Main.netMode == NetmodeID.Server)
            {
                SendAddBuff(-1, fromWho, buffType, buffTime, target);
            }

            npc.AddBuff(buffType, buffTime);
        }

        public void SendRemoveBuff(int toWho, int fromWho, int buffType, int target)
        {
            ModPacket packet = GetPacket(RemoveBuff, fromWho);
            packet.Write(buffType);
            packet.Write(target);
            packet.Send(toWho, fromWho);
            TerraLeague.Log("[DEBUG] - Sending Remove Buff ID " + buffType + " from NPC " + target, Color.White);
        }

        public void ReceiveRemoveBuff(BinaryReader reader, int fromWho)
        {
            int buffType = reader.ReadInt32();
            int target = reader.ReadInt32();
            NPC npc = Main.npc[target];

            npc.DelBuff(npc.FindBuffIndex(buffType));
        }

        public void SendSyncStats(int toWho, int fromWho, int statType, int target, int num)
        {
            ModPacket packet = GetPacket(SyncStats, fromWho);
            packet.Write(statType);
            packet.Write(target);
            packet.Write(num);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveSyncStats(BinaryReader reader, int fromWho)
        {
            int statType = reader.ReadInt32();
            int target = reader.ReadInt32();
            int num = reader.ReadInt32();
            NPC npc = Main.npc[target];

            if (Main.netMode == NetmodeID.Server)
                SendSyncStats(-1, fromWho, statType, target, num);

            switch (statType)
            {
                case 1:
                    npc.AddBuff(BuffType<CausticWounds>(), 240);
                    npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().CausticWounds = true;
                    npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().CausticStacks = num;
                    break;
                case 2:
                    npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().CleavedStacks = num;
                    break;
                case 3:
                    npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().DeadlyVenomStacks = num;
                    break;
                case 4:
                    npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().HemorrhageStacks = num;
                    break;
                case 5:
                    npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().PoxStacks = num;
                    break;
                default:
                    break;
            }

        }

        public void SendCreateVessel(int toWho, int fromWho, int targetNPC, int player)
        {
            ModPacket packet = GetPacket(CreateVessel, fromWho);
            packet.Write(targetNPC);
            packet.Write(player);
            packet.Send(toWho, fromWho);
            TerraLeague.Log("[DEBUG] - Sending Vessel of " + Main.npc[targetNPC].FullName, Color.White);
        }

        public void ReceiveCreateVessel(BinaryReader reader, int fromWho)
        {
            int targetNPC = reader.ReadInt32();
            int player = reader.ReadInt32();

            NPC npc = Main.npc[targetNPC];

            int vessel = NPC.NewNPC((int)Main.player[player].Bottom.X + (64 * Main.player[player].direction), (int)Main.player[player].Bottom.Y, npc.type);
            Main.npc[vessel].life = npc.life;
            Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vesselTarget = npc.whoAmI;
            Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vessel = true;
            Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vesselTimer = 420;

            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, vessel);

            if (Main.netMode == NetmodeID.Server)
                SendSyncVessel(-1, -1, vessel, targetNPC);
        }

        public void SendSyncVessel(int toWho, int fromWho, int vessel, int vesselTarget)
        {
            ModPacket packet = GetPacket(SyncVessel, fromWho);
            packet.Write(vessel);
            packet.Write(vesselTarget);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveSyncVessel(BinaryReader reader, int fromWho)
        {
            int vessel = reader.ReadInt32();
            int vesselTarget = reader.ReadInt32();

            Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vesselTarget = vesselTarget;
            Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vessel = true;
            Main.npc[vessel].GetGlobalNPC<TerraLeagueNPCsGLOBAL>().vesselTimer = 420;

            TerraLeague.Log("[DEBUG] - Recieved Vessel Sync for " + Main.npc[vessel].FullName, Color.White);
        }

        public void SendSpawnNPC(int toWho, int fromWho, int npcType, Vector2 position)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(SpawnNpcOnServer, fromWho);
                packet.Write(npcType);
                packet.WriteVector2(position);
                packet.Send(toWho, fromWho);
            }
        }

        public void ReceiveSpawnNPC(BinaryReader reader, int fromWho)
        {
            int npcType = reader.ReadInt32();
            Vector2 position = reader.ReadVector2();

            NPC.NewNPC((int)position.X, (int)position.Y, npcType);

            TerraLeague.Log("[DEBUG] - Spawned npc type: " + npcType, Color.White);
        }
    }

    internal class ProjectilePacketHandler : PacketHandler
    {
        #region Values
        public const byte Shiv = 1;
        #endregion

        public ProjectilePacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
           
        }
    }

    internal class PassivePacketHandler : PacketHandler
    {
        #region Values
        public const byte passive = 1;
        public const byte targetpassive = 2;
        public const byte lifeline = 3;
        public const byte cleave = 4;
        #endregion

        public PassivePacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (cleave):
                    ReceiveCleave(reader, fromWho);
                    break;
                case (passive):
                    ReceivePassiveEfx(reader, fromWho);
                    break;
                case (targetpassive):
                    ReceiveTargetPassiveEfx(reader, fromWho);
                    break;
            }
        }


        public void SendPassiveEfx(int toWho, int fromWho, int user, int itemID, int passiveArrayPosition)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(passive, fromWho);
                packet.Write(user);
                packet.Write(itemID);
                packet.Write(passiveArrayPosition);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Passive Efx", Color.LightSlateGray);
            }
        }
        private void ReceivePassiveEfx(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Passive Efx", new Color(80, 80, 80));

            int user = reader.ReadInt32();
            int itemID = reader.ReadInt32();
            int passiveArrayPosition = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendPassiveEfx(-1, fromWho, user, itemID, passiveArrayPosition);
            }
            else
            {
                LeagueItem legItem = GetModItem(itemID) as LeagueItem;
                if (legItem != null)
                {
                    if (legItem.Passives != null && passiveArrayPosition >= 0)
                    {
                        if (passiveArrayPosition < legItem.Passives.Length)
                        {
                            legItem.Passives[passiveArrayPosition].Efx(Main.player[user]);
                        }
                    }
                }
            }
        }

        public void SendPassiveEfx(int toWho, int fromWho, int user, int itemID, int passiveArrayPosition, int npc)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(targetpassive, fromWho);
                packet.Write(user);
                packet.Write(itemID);
                packet.Write(passiveArrayPosition);
                packet.Write(npc);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Passive Efx", Color.LightSlateGray);
            }
        }
        private void ReceiveTargetPassiveEfx(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Passive Efx", new Color(80, 80, 80));

            int user = reader.ReadInt32();
            int itemID = reader.ReadInt32();
            int passiveArrayPosition = reader.ReadInt32();
            int npc = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendPassiveEfx(-1, fromWho, user, itemID, passiveArrayPosition, npc);
            }
            else
            {
                LeagueItem legItem = GetModItem(itemID) as LeagueItem;
                if (legItem != null)
                {
                    if (legItem.Passives != null && passiveArrayPosition >= 0)
                    {
                        if (passiveArrayPosition < legItem.Passives.Length)
                        {
                            legItem.Passives[passiveArrayPosition].Efx(Main.player[user], Main.npc[npc]);
                        }
                    }
                }
            }
        }

        // Cleave
        public void SendCleave(int toWho, int fromWho, int type, int user)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(cleave, fromWho);
                packet.Write(user);
                packet.Write(type);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Cleave", Color.LightSlateGray);
            }
        }
        private void ReceiveCleave(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Cleave", new Color(80, 80, 80));

            int user = reader.ReadInt32();
            int type = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendCleave(-1, fromWho, type, user);
            }
            else
            {
                Cleave.Efx(user, (CleaveType)type);
            }
        }
    }

    internal class ActivePacketHandler : PacketHandler
    {
        #region Values
        public const byte active = 1;
        #endregion

        public ActivePacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (active):
                    ReceiveActiveEfx(reader, fromWho);
                    break;
            }
        }

        // Actives
        public void SendActiveEfx(int toWho, int fromWho, int user, int itemID)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(active, fromWho);
                packet.Write(user);
                packet.Write(itemID);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Active Efx", Color.LightSlateGray);
            }
        }
        private void ReceiveActiveEfx(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Active Efx", new Color(80, 80, 80));

            int user = reader.ReadInt32();
            int itemID = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendActiveEfx(-1, fromWho, user, itemID);
            }
            else
            {
                LeagueItem legItem = GetModItem(itemID) as LeagueItem;
                if (legItem != null)
                    legItem.Active.Efx(Main.player[user]);
            }
        }
    }

    internal class SummonerSpellsPacketHandler : PacketHandler
    {
        #region Values
        public const byte Heal = 1;
        public const byte Clarity = 2;
        public const byte Barrier = 3;
        public const byte Ignite = 4;
        public const byte Smite = 5;
        public const byte Ghost = 6;
        public const byte Cleanse = 7;
        public const byte Exhaust = 8;
        public const byte Clairvoyance = 9;
        public const byte Surge = 10;
        public const byte Garrison = 11;
        public const byte Revive = 12;
        public const byte Vanish = 13;
        public const byte Lift = 14;
        public const byte Flash = 15;
        public const byte Teleport = 16;
        public const byte TeleportRequest = 17;
        public const byte TeleportPosition = 18;
        public const byte TeleportRequestPlayer = 19;
        #endregion

        public SummonerSpellsPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (Heal):
                    ReceiveHeal(reader, fromWho);
                    break;
                case (Clarity):
                    ReceiveClarity(reader, fromWho);
                    break;
                case (Barrier):
                    ReceiveBarrier(reader, fromWho);
                    break;
                case (Ignite):
                    ReceiveIgnite(reader, fromWho);
                    break;
                case (Smite):
                    ReceiveSmite(reader, fromWho);
                    break;
                case (Ghost):
                    ReceiveGhost(reader, fromWho);
                    break;
                case (Cleanse):
                    ReceiveCleanse(reader, fromWho);
                    break;
                case (Exhaust):
                    ReceiveExhaust(reader, fromWho);
                    break;
                case (Clairvoyance):
                    ReceiveClairvoyance(reader, fromWho);
                    break;
                case (Surge):
                    ReceiveSurge(reader, fromWho);
                    break;
                case (Garrison):
                    ReceiveGarrison(reader, fromWho);
                    break;
                case (Revive):
                    ReceiveRevive(reader, fromWho);
                    break;
                case (Vanish):
                    ReceiveVanish(reader, fromWho);
                    break;
                case (Lift):
                    ReceiveLift(reader, fromWho);
                    break;
                case (Flash):
                    ReceiveFlash(reader, fromWho);
                    break;
                case (Teleport):
                    ReceiveTeleport(reader, fromWho);
                    break;
                case (TeleportRequest):
                    ReceiveTeleportRequest(reader, fromWho);
                    break;
                case (TeleportPosition):
                    ReceiveTeleportPosition(reader, fromWho);
                    break;
                case (TeleportRequestPlayer):
                    ReceiveTeleportRequestPlayer(reader, fromWho);
                    break;
            }


        }

        // Heal
        public void SendHeal(int toWho, int fromWho, int user, int target = -1)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Heal, fromWho);
                packet.Write(user);
                packet.Write(target);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Heal", Color.LightSlateGray);
            }
        }
        private void ReceiveHeal(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Heal", new Color(80, 80, 80));

            int user = reader.ReadInt32();
            int target = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendHeal(-1, fromWho, user, target);
            }
            else
            {
                HealRune.Efx(Main.player[target]);

                if (target != -1)
                    HealRune.Efx(Main.player[target], false);
            }
        }

        // Smite
        public void SendSmite(int toWho, int fromWho, int target)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Smite, fromWho);
                packet.Write(target);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Smite", Color.LightSlateGray);
            }
        }
        private void ReceiveSmite(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Smite", new Color(80, 80, 80));

            int target = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendSmite(-1, fromWho, target);
            }
            else
            {
                // There is no effect
            }
        }

        // Ghost
        public void SendGhost(int toWho, int fromWho, int target)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Ghost, fromWho);
                packet.Write(target);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Ghost", Color.LightSlateGray);
            }
        }
        private void ReceiveGhost(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Ghost", new Color(80, 80, 80));

            int user = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendGhost(-1, fromWho, user);
            }
            else
            {
                GhostRune.Efx(Main.player[user]);
            }
        }

        // Clarity
        public void SendClarity(int toWho, int fromWho, int user, bool drawRing)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Clarity, fromWho);
                packet.Write(user);
                packet.Write(drawRing);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Clarity", Color.LightSlateGray);
            }
        }
        private void ReceiveClarity(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Clarity", new Color(80, 80, 80));

            int target = reader.ReadInt32();
            bool drawRing = reader.ReadBoolean();

            if (Main.netMode == NetmodeID.Server)
            {
                SendClarity(-1, fromWho, target, drawRing);
            }
            else
            {
                ClarityRune.Efx(Main.player[target], drawRing);
            }
        }

        // Clairvoyance
        public void SendClairvoyance(int toWho, int fromWho, int target)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Clairvoyance, fromWho);
                packet.Write(target);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Clairvoyance", Color.LightSlateGray);
            }
        }
        private void ReceiveClairvoyance(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Clairvoyance", new Color(80, 80, 80));

            int target = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendClairvoyance(-1, fromWho, target);
            }
            else
            {
                ClairvoyanceRune.Efx(Main.player[target]);
            }
        }

        // Surge
        public void SendSurge(int toWho, int fromWho, int user)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Surge, fromWho);
                packet.Write(user);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Surge", Color.LightSlateGray);
            }
        }
        private void ReceiveSurge(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Surge", new Color(80, 80, 80));

            int user = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendSurge(-1, fromWho, user);
            }
            else
            {
                SurgeRune.Efx(Main.player[user]);
            }
        }

        // Garrison
        public void SendGarrison(int toWho, int fromWho, int user)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Garrison, fromWho);
                packet.Write(user);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Garrison", Color.LightSlateGray);
            }
        }
        private void ReceiveGarrison(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Garrison", new Color(80, 80, 80));

            int user = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendGarrison(-1, fromWho, user);
            }
            else
            {
                GarrisonRune.Efx(Main.player[user]);
            }
        }

        // Revive
        public void SendRevive(int toWho, int fromWho, int user)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Revive, fromWho);
                packet.Write(user);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Revive", Color.LightSlateGray);
            }
        }
        private void ReceiveRevive(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Revive", new Color(80, 80, 80));

            int user = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendRevive(-1, fromWho, user);
            }
            else
            {
                ReviveRune.Efx(Main.player[user]);
            }
        }

        // Vanish
        public void SendVanish(int toWho, int fromWho, int user)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Vanish, fromWho);
                packet.Write(user);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Vanish", Color.LightSlateGray);
            }
        }
        private void ReceiveVanish(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Vanish", new Color(80, 80, 80));

            int user = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendVanish(-1, fromWho, user);
            }
            else
            {
                VanishRune.Efx(Main.player[user]);
            }
        }

        // Lift
        public void SendLift(int toWho, int fromWho, int user)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Lift, fromWho);
                packet.Write(user);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Lift", Color.LightSlateGray);
            }
        }
        private void ReceiveLift(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Lift", new Color(80, 80, 80));

            int user = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendLift(-1, fromWho, user);
            }
            else
            {
                LiftRune.Efx(Main.player[user]);
            }
        }

        // Barrier
        public void SendBarrier(int toWho, int fromWho, int user)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Barrier, fromWho);
                packet.Write(user);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Barrier", Color.LightSlateGray);
            }
        }
        private void ReceiveBarrier(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Barrier", new Color(80, 80, 80));

            int user = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendBarrier(-1, fromWho, user);
            }
            else
            {
                BarrierRune.Efx(Main.player[user]);
            }
        }

        // Cleanse
        public void SendCleanse(int toWho, int fromWho, int user)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Cleanse, fromWho);
                packet.Write(user);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Cleanse", Color.LightSlateGray);
            }
        }
        private void ReceiveCleanse(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Cleanse", new Color(80, 80, 80));

            int user = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendCleanse(-1, fromWho, user);
            }
            else
            {
                CleanseRune.Efx(Main.player[user]);
            }
        }

        // Exhaust
        public void SendExhaust(int toWho, int fromWho, int target)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Exhaust, fromWho);
                packet.Write(target);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Exhaust", Color.LightSlateGray);
            }
        }
        private void ReceiveExhaust(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Exhaust", new Color(80, 80, 80));

            int target = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendExhaust(-1, fromWho, target);
            }
            else
            {
                // No effect
            }
        }

        // Ignite
        public void SendIgnite(int toWho, int fromWho, int target)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Ignite, fromWho);
                packet.Write(target);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Ignite", Color.LightSlateGray);
            }
        }
        private void ReceiveIgnite(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Ignite", new Color(80, 80, 80));

            int target = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendIgnite(-1, fromWho, target);
            }
            else
            {
                //No Effect
            }
        }

        // Flash
        public void SendFlash(int toWho, int fromWho, Vector2 start, Vector2 end)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Flash, fromWho);
                packet.WriteVector2(start);
                packet.WriteVector2(end);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Flash", Color.LightSlateGray);
            }
        }
        private void ReceiveFlash(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Flash", new Color(80, 80, 80));

            Vector2 start = reader.ReadVector2();
            Vector2 end = reader.ReadVector2();

            if (Main.netMode == NetmodeID.Server)
            {
                SendFlash(-1, fromWho, start, end);
            }
            else
            {
                FlashRune.Efx(start, end);
            }
        }

        public void SendTeleport(int toWho, int fromWho, Vector2 point)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Teleport, fromWho);
                packet.WriteVector2(point);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Teleport", Color.LightSlateGray);
            }
        }
        private void ReceiveTeleport(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Teleport", new Color(80, 80, 80));

            Vector2 point = reader.ReadVector2();

            if (Main.netMode == NetmodeID.Server)
            {
                SendTeleport(-1, fromWho, point);
            }
            else
            {
                TeleportRune.Efx(point);
            }
        }

        public void SendTeleportRequest(int toWho, int fromWho, int type)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(TeleportRequest, fromWho);
                packet.Write(type);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Teleport Request", Color.LightSlateGray);
            }
        }
        private void ReceiveTeleportRequest(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Request", new Color(80, 80, 80));

            TeleportType type = (TeleportType)reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                Vector2 originalPosition = Main.player[fromWho].position;

                switch (type)
                {
                    case TeleportType.LeftBeach:
                        originalPosition = TeleportRune.LeftBeach();
                        break;
                    case TeleportType.RightBeach:
                        originalPosition = TeleportRune.RightBeach();
                        break;
                    case TeleportType.Dungeon:
                        originalPosition = TeleportRune.Dungeon();
                        break;
                    case TeleportType.Hell:
                        originalPosition = TeleportRune.Hell(Main.player[fromWho]);
                        break;
                    case TeleportType.Random:
                        originalPosition = TeleportRune.RandomTP();
                        break;
                    default:
                        break;
                }

                SendTeleportPosition(-1, -1, originalPosition, fromWho);
            }
        }

        public void SendTeleportRequestPlayer(int toWho, int fromWho, int type)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(TeleportRequestPlayer, fromWho);
                packet.Write(type);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Teleport Request - Player", Color.LightSlateGray);
            }
        }
        private void ReceiveTeleportRequestPlayer(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Request - Player", new Color(80, 80, 80));

            Player player = Main.player[reader.ReadInt32()];

            if (Main.netMode == NetmodeID.Server)
            {
                Vector2 originalPosition = Main.player[fromWho].position;

                originalPosition = player.position;

                SendTeleportPosition(-1, -1, originalPosition, fromWho);
            }
        }

        public void SendTeleportPosition(int toWho, int fromWho, Vector2 position, int player)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(TeleportPosition, fromWho);
                packet.WriteVector2(position);
                packet.Write(player);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Teleport Position", Color.LightSlateGray);
            }
        }
        private void ReceiveTeleportPosition(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Teleport Position", new Color(80, 80, 80));

            Vector2 point = reader.ReadVector2();
            int player = reader.ReadInt32();

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                TeleportRune.DoTP(Main.player[player], point);
            }
        }
    }

    internal class AbilitiesPacketHandler : PacketHandler
    {
        #region Values
        public const byte UmbralTrespassNPC = 1;
        public const byte Requiem = 2;
        public const byte Contaminate = 3;
        public const byte Wish = 4;
        public const byte Efx = 0;
        #endregion

        public AbilitiesPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (Efx):
                    ReceiveEfx(reader, fromWho);
                    break;
                case (Wish):
                    ReceiveWish(reader, fromWho);
                    break;
                case (UmbralTrespassNPC):
                    ReceiveUmbralNPC(reader, fromWho);
                    break;
            }
        }

        // Efx
        public void SendEfx(int toWho, int fromWho, int castItem, int caster, AbilityType abilityType)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Efx, fromWho);
                packet.Write(castItem);
                packet.Write(caster);
                packet.Write((byte)abilityType);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Efx (ItemID: " + castItem + " | Ability Type: "+ abilityType + ")", new Color(50, 200, 0));
            }
        }
        private void ReceiveEfx(BinaryReader reader, int fromWho)
        {
            int CastItem = reader.ReadInt32();
            int Caster = reader.ReadInt32();
            AbilityType AbilityType = (AbilityType)reader.ReadByte();

            if (Main.netMode == NetmodeID.Server)
            {
                SendEfx(-1, fromWho, CastItem, Caster, AbilityType);
            }
            else
            {
                TerraLeague.Log("[DEBUG] - Received Efx (Caster: " + Caster + " | ItemID: " + CastItem + " | Ability Type: " + AbilityType + ")", new Color(200, 200, 0));

                ModItem moditem = GetModItem(CastItem);

                if (moditem != null)
                {
                    moditem.SetDefaults();
                    AbilityItemGLOBAL abilityItem = moditem.item.GetGlobalItem<AbilityItemGLOBAL>();
                    abilityItem.GetAbility(AbilityType).Efx(Main.player[Caster]);
                }
            }
        }

        // Wish
        public void SendWish(int toWho, int fromWho)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(Wish, fromWho);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending Wish", new Color(50, 200, 0));
            }
        }
        private void ReceiveWish(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received Wish", new Color(200, 200, 0));

            if (Main.netMode == NetmodeID.Server)
            {
                SendWish(-1, fromWho);
            }
            else
            {
                for (int i = 0; i < Main.player.Length; i++)
                {
                    if (Main.player[i].active)
                    {
                        for (int j = 0; j < 18; j++)
                        {
                            Dust dust = Dust.NewDustDirect(new Vector2(Main.rand.Next((int)Main.player[i].position.X - 8, (int)Main.player[i].position.X + 8), Main.player[i].position.Y + 16), Main.player[i].width, Main.player[i].height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(0, 255, 0, 0), Main.rand.Next(2, 6));
                            dust.noGravity = true;
                        }
                    }
                }
                Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f));
            }
        }

        // Umbral Trespass NPC
        public void SendUmbralNPC(int toWho, int fromWho, int npc, int applier)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = GetPacket(UmbralTrespassNPC, fromWho);
                packet.Write(npc);
                packet.Write(applier);
                packet.Send(toWho, fromWho);
                TerraLeague.Log("[DEBUG] - Sending NPC", new Color(30, 0, 0));
            }
        }
        private void ReceiveUmbralNPC(BinaryReader reader, int fromWho)
        {
            TerraLeague.Log("[DEBUG] - Received NPC", new Color(50, 0, 0));

            int Npc = reader.ReadInt32();
            int Applier = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                SendUmbralNPC(-1, fromWho, Npc, Applier);
            }
            else
            {
                Main.player[Applier].GetModPlayer<PLAYERGLOBAL>().umbralTaggedNPC = Main.npc[Npc];
            }
        }
    }

    internal class WorldPacketHandler : PacketHandler
    {
        #region Values
        public const byte UpdateBlackMist = 1;
        #endregion

        public WorldPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            switch (reader.ReadByte())
            {
                case (UpdateBlackMist):
                    ReceiveBlackMist(reader, fromWho);
                    break;
            }
        }

        public void SendBlackMist(int toWho, int fromWho, bool active)
        {
            ModPacket packet = GetPacket(UpdateBlackMist, fromWho);
            packet.Write(active);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveBlackMist(BinaryReader reader, int fromWho)
        {
            bool active = reader.ReadBoolean();
            TerraLeague.Log("Recieved Global Black Mist is now set to " + active, Color.SeaGreen);

            TerraLeagueWORLDGLOBAL.BlackMistEvent = active;
        }
    }
}
