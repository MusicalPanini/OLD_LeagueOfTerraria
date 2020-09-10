using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items;
using TerraLeague.Items.SummonerSpells;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.NPCs
{
    [AutoloadHead]
    public class SummonerNPC : ModNPC
    {
        int currentHelp = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Summoner");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Guide];
            NPCID.Sets.ExtraFramesCount[npc.type] = NPCID.Sets.ExtraFramesCount[NPCID.Guide];
            NPCID.Sets.AttackFrameCount[npc.type] = NPCID.Sets.AttackFrameCount[NPCID.Guide];
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = NPCID.Sets.AttackType[NPCID.Guide];
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            NPCID.Sets.HatOffsetY[npc.type] = -4;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            animationType = NPCID.Guide;
            base.SetDefaults();
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            for (int k = 0; k < Main.player.Length; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }

                foreach (Item item in player.inventory)
                {
                    if (item.type == ItemType<RawMagic>() && item.stack >= 10)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override string TownNPCName()
        {
            switch (WorldGen.genRand.Next(4))
            {
                case 0:
                    return "Marc";
                case 1:
                    return "Merrill";
                case 2:
                    return "Lance";
                default:
                    return "Rito";
            }
        }

        public override string GetChat()
        {
            if (Main.rand.Next(3) == 0)
            {
                if (!NPC.downedBoss1)
                    return "I'm currently working on some new Runes, but some evil force at work preventing them from forming." +
                        "\n\nMy research suggests that some kind of 'Watching Presence' is the cause of this." +
                        "\n\nIf you could slay this 'Presence', I can complete the new Runes!";
                else if (!NPC.downedBoss2)
                    if (WorldGen.crimson)
                        return "I'm currently working on some new Runes, but need to gather research from the Crimson." +
                            "\n\nMy research shows that a 'Gruesome Mind' rules those lands and as such it is" +
                            "\ncurrently increadibly unsafe for me to obtain the nessesary research." +
                            "\n\nIf you could slay this 'Mind', I can complete the new Runes!";
                    else
                        return "I'm currently working on some new Runes, but need to gather research from the Corruption." +
                            "\n\nMy research suggests that a 'Destructive Worm' rules those lands and as such it is" +
                            "\ncurrently increadibly unsafe for me to obtain the nessesary research." +
                            "\n\nIf you could slay this 'Worm', I can complete the new Runes!";
                else if (!NPC.downedBoss3)
                    return "I have discovered some information that suggests The Dungeon contains some powerful Runes" +
                             "\n\nIf you could break the Old Man's Curse and gain us access to the depths of The Dungeon, we may be able to obtain some new Runes!";
            }

            int guide = NPC.FindFirstNPC(NPCID.Guide);
            if (guide >= 0 && Main.rand.NextBool(6))
            {
                return Main.npc[guide].GivenName + " is very knowledgeable, but is he Challenger?";
            }
            int nurse = NPC.FindFirstNPC(NPCID.Nurse);
            if (nurse >= 0 && Main.rand.NextBool(6) && NPC.downedBoss2)
            {
                return Main.npc[nurse].GivenName + " should be able to help you if you accidentally Flash into a wall.";
            }
            int wizard = NPC.FindFirstNPC(NPCID.Wizard);
            if (wizard >= 0 && Main.rand.NextBool(6))
            {
                switch (Main.rand.Next(2))
                {
                    case 0:
                        return "I respect " + Main.npc[wizard].GivenName + "'s craft, but I do wish he would stop turning my sapphire crystals into dirt.";
                    case 1:
                        return "Could you check on " + Main.npc[wizard].GivenName + " for me? He purchased an Ignite Rune and I fear for his room..";
                    default:
                        break;
                }
                
            }
            switch (Main.rand.Next(Main.hardMode ? 8 : 7))
            {
                case 0:
                    return "Salutations, " + Main.LocalPlayer.name + ". How goes your travels?";
                case 1:
                    return "Have you seen a Blue Sentinal recently?\n...\nWhat do you mean YOU want the buff?!";
                case 2:
                    return "Careful how you use your Summoner Spells. They can be powerful, but your body must recharge before using them again.";
                case 3:
                    return "Care to try a new Spell?";
                case 4:
                    return "Do not worry, Summoner Spells are so powerful even the most magically inept person can use them!";
                case 5:
                    return "Did you know enemies afflicted with 'Slowed' also have their damage reduced by 30%!";
                case 6:
                    return "Take care near the beaches of this world during New Moons." +
                        "\nAn evil and twisted Black Mist rolls ashore, bringing with it a damning, undead evil." +
                        "\nIt's even rumored to sometimes travel further inland...";
                case 7:
                    return "If your lucky, you might get a Hex Crystal from a Hextech Chest";
                default:
                    return "Open mid pls";
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = "Game Mechanics Help";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
            else
            {
                string text = "";
                switch (currentHelp)
                {
                    case 0:
                        text = "Summoner Spells are a special kind of magic that requires no mana to cast." +
                            "\nYou can change your Summoner Spells by using the Runes I create. Just bring the necessary Vials of True Magic for their creation.";
                        break;
                    case 1:
                        text = "Your abilities can not crit under most circumstances, though I have heard of an item allowing such power!";
                        break;
                    case 2:
                        text = "Armor decreases contact damage." +
                            "\nResist decreases projectile damage.";
                        break;
                    case 3:
                        //text = "Lifesteal can be very powerful, but it has its drawbacks. In most cases it will reduces your max life and increase the damage you take." +
                        //    "\nUpon taking damage, you will be afflicted with 'Grevious Wounds'. This debuff will negate your lifesteal." +
                        //    "\nProjectiles that pierce or chase down their target also have their lifesteal reduced." +
                        //    "\nYou cannot lifesteal more than " + PLAYERGLOBAL.lifestealMax + " life per attack";
                        text = "When you take damage, you will be afflicted with 'Grevious Wounds'. This debuff will negate your life steal.";
                        break;
                    case 4:
                        text = "On Hit Damage is an additional proc of damage that follows a different rule set." +
                            "\nIt has a 25% chance to be applied" +
                            "\nMelee projectiles deal reduced On Hit Damage.";
                        break;
                    case 5:
                        text = "Actives and Passives of the same name do not stack." +
                            "\nThe accessory you have equipped first will take priority";
                        break;
                    case 6:
                        text = "Grievous Wounds when applied to an enemy will take double damage from all damage over time effects";
                        break;
                    case 7:
                        text = "Heal Power (HEAL) increases all out-going healing." +
                            "\nIt addtionally will increase the amount potions will heal for.";
                        break;
                    case 8:
                        text = "Cooldown Reduction (CDR) reduces the recharge time of Summoner Spells, Abilities and Active Items" +
                            "\nThis reduction is capped at 40%.";
                        break;
                    default:
                        break;
                }


                Main.npcChatText = text;
                currentHelp++;
                if (currentHelp > 8)
                    currentHelp = 0;
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ItemType<CleanseRune>());
            shop.item[nextSlot].shopCustomPrice = 3;
            shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<BarrierRune>());
            shop.item[nextSlot].shopCustomPrice = 3;
            shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<GhostRune>());
            shop.item[nextSlot].shopCustomPrice = 3;
            shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<ExhaustRune>());
            shop.item[nextSlot].shopCustomPrice = 5;
            shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemType<LiftRune>());
            shop.item[nextSlot].shopCustomPrice = 5;
            shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
            nextSlot += 6;
            if (NPC.downedBoss1)
            {
                shop.item[nextSlot].SetDefaults(ItemType<ClarityRune>());
                shop.item[nextSlot].shopCustomPrice = 8;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemType<IgniteRune>());
                shop.item[nextSlot].shopCustomPrice = 8;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemType<VanishRune>());
                shop.item[nextSlot].shopCustomPrice = 8;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemType<HealRune>());
                shop.item[nextSlot].shopCustomPrice = 10;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemType<GarrisonRune>());
                shop.item[nextSlot].shopCustomPrice = 10;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot += 6;
            }
            if (NPC.downedBoss2)
            {
                shop.item[nextSlot].SetDefaults(ItemType<ClairvoyanceRune>());
                shop.item[nextSlot].shopCustomPrice = 8;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemType<SmiteRune>());
                shop.item[nextSlot].shopCustomPrice = 10;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemType<SurgeRune>());
                shop.item[nextSlot].shopCustomPrice = 10;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemType<FlashRune>());
                shop.item[nextSlot].shopCustomPrice = 15;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot += 7;
            }
            if (NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ItemType<SyphonRune>());
                shop.item[nextSlot].shopCustomPrice = 8;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemType<ReviveRune>());
                shop.item[nextSlot].shopCustomPrice = 10;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemType<TeleportRune>());
                shop.item[nextSlot].shopCustomPrice = 15;
                shop.item[nextSlot].shopSpecialCurrency = TerraLeague.instance.SumCurrencyID;
                nextSlot++;
            }
            if (Main.hardMode)
            {
                nextSlot = 38;
                shop.item[nextSlot].SetDefaults(ItemType<HextechChest>());
                nextSlot++;
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.SapphireBolt;
            attackDelay = 1;
        }
        
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }
    }
}
