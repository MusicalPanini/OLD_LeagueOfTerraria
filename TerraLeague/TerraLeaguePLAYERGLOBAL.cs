using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Audio;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;
using TerraLeague.NPCs;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using TerraLeague.Items.Weapons;
using TerraLeague.Items.StartingItems;
using TerraLeague.Items.SummonerSpells;
using TerraLeague.Items.CustomItems;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Items.CustomItems.Passives;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items;
using TerraLeague.Items.Weapons.Abilities;

namespace TerraLeague
{
    public class PLAYERGLOBAL : ModPlayer
    {
        internal PlayerPacketHandler PacketHandler = ModNetHandler.playerHandler;
        internal NPCSpawnInfo nPCSpawnInfo = new NPCSpawnInfo();

        public static int lifestealMax = 25;

        // Ability Animation
        public int abilityAnimationType = 0;
        public int abilityAnimation = 0;
        public int abilityAnimationMax = 0;
        public Item abilityItem = null;
        public Vector2 abilityItemPosition = Vector2.Zero;
        public float abilityRotation = 0;
        public int oldUsedInventorySlot = -1;

        /// <summary>
        /// Is set to 0 everytime you take or deal damage. Counts up by 1 every frame up to 240
        /// </summary>
        public int CombatTimer = 240;
        /// <summary>
        /// Frame counter for shields
        /// </summary>
        internal int shieldFrame = 0;
        /// <summary>
        /// Tracks the use time of melee swings
        /// </summary>
        internal int usetime = 0;
        /// <summary>
        /// Timer for the mana regen ticks
        /// </summary>
        internal int manaRegenTimer = 0;
        /// <summary>
        /// Is the player in a surface marble biome
        /// </summary>
        internal bool zoneSurfaceMarble = false;
        /// <summary>
        /// Is the player in the Black Mist
        /// </summary>
        internal bool zoneBlackMist = false;
        internal bool zoneTargonPeak = false;
        internal bool zoneTargon = false;
        internal bool zoneTargonMonolith = false;
        /// <summary>
        /// Has the player hit an enemy with current melee swing
        /// </summary>
        internal bool hasHitMelee = false;


        #region Custom Stats
        public int BonusMEL = 0;
        public int BonusRNG = 0;
        public int BonusMAG = 0;
        public int BonusSUM = 0;
        /// <summary>
        /// Melee stat for abilities, passives, and actives (MEL)
        /// </summary>
        public int MEL
        {
            get
            {
                int x = (int)((meleeDamageLastStep*100) - 100) ;
                int baseDamage;
                if (NPC.downedGolemBoss)
                    baseDamage = 70;
                else if (NPC.downedPlantBoss)
                    baseDamage = 50;
                else if (NPC.downedMechBossAny)
                    baseDamage = 35;
                else if (Main.hardMode)
                    baseDamage = 20;
                else if (NPC.downedBoss2)
                    baseDamage = 15;
                else
                    baseDamage = 10;


                int stat = (int)(x * 1.5) + baseDamage + BonusMEL;

                if (stat < 0)
                    return 0;
                else
                    return stat;
            }
        }
        /// <summary>
        /// Ranged stat for abilities, passives, and actives (RNG)
        /// </summary>
        public int RNG
        {
            get
            {
                int x = (int)((rangedDamageLastStep * 100) - 100);

                //int stat = (int)Math.Pow(x * ((Math.Sqrt(3/2f)/10f)), 2);
                int stat = (int)(x * 2) + BonusRNG;

                if (stat < 0)
                    return 0;
                else
                    return stat;
            }
        }
        /// <summary>
        /// Magic stat for abilities, passives, and actives
        /// </summary>
        public int MAG
        {
            get
            {
                int x = (int)((magicDamageLastStep * 100) - 100);

                //int stat = (int)Math.Pow(x * (1f/(5f*Math.Sqrt(2))), 2);
                int stat = (int)(x * 2.5) + BonusMAG;

                if (stat < 0)
                    return 0;
                else
                    return stat;
            }
        }
        /// <summary>
        /// Summoner stat for abilities, passives, and actives
        /// </summary>
        public int SUM
        {
            get
            {
                int x = (int)((minionDamageLastStep * 100) - 100);

                //int stat = (int)Math.Pow(x * ((Math.Sqrt(3/2f)/10f)), 2);
                int stat = (int)(x * 1.75) + BonusSUM;

                if (stat < 0)
                    return 0;
                else
                    return stat;
            }
        }

        /// <summary>
        /// consumeAmmoStat
        /// </summary>
        private double consumeAmmo = 0;
        /// <summary>
        /// Chance to not consume ammo
        /// </summary>
        public double ConsumeAmmoChance
        {
            get
            {
                if (consumeAmmo > 1)
                {
                    return 1;
                }
                else
                {
                    return consumeAmmo;
                }
            }
            set { consumeAmmo = value; }
        }

        public double rangedAttackSpeed = 1;

        public int abilityHaste = 0;
        /// <summary>
        /// Cooldown multiplier. Can't be less than 0.6
        /// </summary>
        public float Cdr
        {
            get { return 100/(abilityHaste + 100f); }
        }
        /// <summary>
        /// <para>The Ability Haste last frame.</para>
        /// Used for tooltips or situations where you may not have calculated everything on the current frame
        /// </summary>
        public int abilityHasteLastStep = 0;
        public float CdrLastStep
        {
            get { return 100 / (abilityHasteLastStep + 100f); }
        }

        public int itemHaste = 0;
        public float ItemCdr
        {
            get { return 100 / (itemHaste + 100f); }
        }
        public int itemHasteLastStep = 0;
        public float ItemCdrLastStep
        {
            get { return 100 / (itemHasteLastStep + 100f); }
        }

        public int summonerHaste = 0;
        public float SummonerCdr
        {
            get { return 100 / (summonerHaste + 100f); }
        }
        public int summonerHasteLastStep = 0;
        public float SummonerCdrLastStep
        {
            get { return 100 / (itemHasteLastStep + 100f); }
        }

        public int ultHaste = 0;
        public float UltCdr
        {
            get { return 100 / (ultHaste + 100f); }
        }
        public int ultHasteLastStep = 0;
        public float UltCdrLastStep
        {
            get { return 100 / (ultHasteLastStep + 100f); }
        }

        // Healpower Stuff
        /// <summary>
        /// Healing and shielding multiplier
        /// </summary>
        public double healPower = 1;
        /// <summary>
        /// <para>The healPower last frame.</para>
        /// Used for tooltips or situations where you may not have calculated everything on the current frame
        /// </summary>
        public double healPowerLastStep = 1;
        /// <summary>
        /// <para>Did the Player have Spiritual Restoration (increase all healing by 30%) last frame.</para>
        /// Used for tooltips or situations where you may not have calculated everything on the current frame
        /// </summary>
        public bool hasSpiritualRestorationLastStep = false;

        /// <summary>
        /// <para>Total amount of armor.</para>
        /// Armor is defence against contact damage
        /// </summary>
        public int armor = 0;
        public float ArmorDamageReduction
        {
            get
            {
                int trueArmor = armor;

                if (TerraLeague.UseCustomDefenceStat)
                    trueArmor += player.statDefense;

                if (trueArmor >= 0)
                {
                    return 100 / (100f + trueArmor);
                }
                else
                {
                    return 2 - (100 / (100f - trueArmor));
                }
            }
        }

        /// <summary>
        /// <para>Total amount of resist.</para>
        /// Resist is defence against projectile
        /// </summary>
        public int resist = 0;
        public float ResistDamageReduction
        {
            get
            {
                int trueResist = resist;

                if (TerraLeague.UseCustomDefenceStat)
                    trueResist += player.statDefense;

                if (trueResist >= 0)
                {
                    return 100 / (100f + trueResist);
                }
                else
                {
                    return 2 - (100 / (100f - trueResist));
                }
            }
        }

        /// <summary>
        /// Players mana regen per second
        /// </summary>
        public int manaRegen = 1;
        /// <summary>
        /// Mana regen mulitplier
        /// </summary>
        public double manaRegenModifer = 1;

        // Flat Damage
        /// <summary>
        /// Flat damage added to melee attacks
        /// </summary>
        public int meleeFlatDamage = 0;
        /// <summary>
        /// Flat damage added to ranged attacks
        /// </summary>
        public int rangedFlatDamage = 0;
        /// <summary>
        /// Flat damage added to magic attacks
        /// </summary>
        public int magicFlatDamage = 0;
        /// <summary>
        /// Flat damage added to minion attacks
        /// </summary>
        public int minionFlatDamage = 0;

        // Damage Modifiers
        /// <summary>
        /// Melee damage multiplier
        /// </summary>
        public double meleeModifer = 1;
        /// <summary>
        /// Ranged damage multiplier
        /// </summary>
        public double rangedModifer = 1;
        /// <summary>
        /// Magic damage multiplier
        /// </summary>
        public double magicModifer = 1;
        /// <summary>
        /// Minion damage multiplier
        /// </summary>
        public double minionModifer = 1;

        // Armor Pen
        /// <summary>
        /// Amount of defence ignored by melee attacks
        /// </summary>
        public int meleeArmorPen = 0;
        /// <summary>
        /// Amount of defence ignored by ranged attacks
        /// </summary>
        public int rangedArmorPen = 0;
        /// <summary>
        /// Amount of defence ignored by magic attacks
        /// </summary>
        public int magicArmorPen = 0;
        /// <summary>
        /// Amount of defence ignored by minion attacks
        /// </summary>
        public int minionArmorPen = 0;

        // On Hit Damage
        /// <summary>
        /// On hit damage applied by melee attacks
        /// </summary>
        public int meleeOnHit = 0;
        /// <summary>
        /// On hit damage applied by ranged attacks
        /// </summary>
        public int rangedOnHit = 0;
        /// <summary>
        /// On hit damage applied by magic attacks
        /// </summary>
        public int magicOnHit = 0;
        /// <summary>
        /// On hit damage applied by minion attacks
        /// </summary>
        public int minionOnHit = 0;

        // Stat Scaling
        /// <summary>
        /// Melee stat multiplier (Increases the stats instead of just the damage)
        /// </summary>
        public float meleeStatScaling = 1;
        /// <summary>
        /// Ranged stat multiplier (Increases the stats instead of just the damage)
        /// </summary>
        public float rangedStatScaling = 1;
        /// <summary>
        /// Magic stat multiplier (Increases the stats instead of just the damage)
        /// </summary>
        public float magicStatScaling = 1;
        /// <summary>
        /// Minion stat multiplier (Increases the stats instead of just the damage)
        /// </summary>
        public float minionStatScaling = 1;

        // Shield Stuff
        /// <summary>
        /// List of players current shields
        /// </summary>
        public List<Shield> Shields = new List<Shield>();
        
        public int MagicShield = 0;
        public int PhysicalShield = 0;
        public int NormalShield = 0;
        public int PureHealthLastStep = 0;
        public string wasHitByProjOrNPCLastStep = "None";
        public Color currentShieldColor = new Color(255,255,255,0);

        /// <summary>
        /// Returns the players current health without the shields
        /// </summary>
        /// <param name="maxHealth">If true, returns the players max health instead</param>
        /// <returns></returns>
        public int GetRealHeathWithoutShield(bool maxHealth = false)
        {
            if (maxHealth)
                return player.statLifeMax2 - GetTotalShield();
            else
                return player.statLife - GetTotalShield();
        }
        /// <summary>
        /// Returns the total amount of shielding on the player
        /// </summary>
        /// <returns></returns>
        public int GetTotalShield()
        {
            return MagicShield + PhysicalShield + NormalShield;
        }
        #endregion

        #region Lifesteal and Healing
        /// <summary>
        /// Melee lifesteel percent
        /// </summary>
        public double lifeStealMelee = 0;
        /// <summary>
        /// Ranged lifesteel percent
        /// </summary>
        public double lifeStealRange = 0;
        /// <summary>
        /// Magic lifesteel percent
        /// </summary>
        public double lifeStealMagic = 0;
        /// <summary>
        /// Minion lifesteel percent
        /// </summary>
        public double lifeStealMinion = 0;
        /// <summary>
        /// Total amount of life stolen on the current step
        /// </summary>
        public double lifeStealCharge = 0;
        /// <summary>
        /// Players max life multiplier
        /// </summary>
        public double healthModifier = 1;
        /// <summary>
        /// Players damage taken multiplier
        /// </summary>
        public double damageTakenModifier = 1;

        /// <summary>
        /// Total amount of life to heal next step
        /// </summary>
        public int lifeToHeal = 0;
        #endregion

        // Other
        public bool meleeProjCooldown = false;

        // Costume Stuff
        public bool darkinCostume = false;
        public bool darkinCostumeHideVanity = false;
        public bool darkinCostumeForceVanity = false;

        // Stat Calculation Stuff
        /// <summary>
        /// Used for all modded minion damage instead of the vanilla stat (player.minionDamage) to let summoned minions scale their damage while active
        /// </summary>
        public double TrueMinionDamage = 0;
        public double meleeDamageLastStep = 0;
        public double rangedDamageLastStep = 0;
        public double magicDamageLastStep = 0;
        public double minionDamageLastStep = 0;
        public double rocketDamageLastStep = 0;
        public double arrowDamageLastStep = 0;
        public double bulletDamageLastStep = 0;
        public int defenceLastStep = 0;
        public int armorLastStep = 0;
        public int resistLastStep = 0;
        public int maxLifeLastStep = 0;
        public int manaLastStep = 0;
        public double extraSumCDRLastStep = 0;
        public double maxMinionsLastStep = 0;

        // Summoner Spells
        public SummonerSpell[] sumSpells = new SummonerSpell[2] { GetInstance<GhostRune>(), GetInstance<BarrierRune>() };
        public int[] sumCooldowns = new int[2];
        SummonerSpell initSum1 = null;
        SummonerSpell initSum2 = null;
        public double extraSumCDR = 0;

        public bool reviving = false;

        // Abilities
        public Ability[] Abilities = new Ability[4];
        public int[] AbilityCooldowns = new int[4];

        // Actives and Passives
        public bool[] PassivesAreActive = new bool[12];
        public bool[] ActivesAreActive = new bool[6];

        // Custom Armor Set Bonuses
        public bool pirateSet = false;
        public bool cannonSet = false;
        public int cannonTimer = 0;
        public bool petriciteSet = false;
        public bool prophetSet = false;
        public int prophetTimer = 0;
        public bool solariSet = false;
        public int solariCharge = 0;
        public static int solariMaxCharge = 7200; // 4 minutes
        public bool solarStorm = false;
        public bool voidbornSet = false;

        // Targon Peak Cooldown
        public int blessingCooldown = 0;

        // Buffs
        public bool bioBarrage = false;
        public bool crushingBlows = false;
        public bool contactDodge = false;
        public bool deadlyPlumage = false;
        public bool deathLotus = false;
        public bool decisiveStrike = false;
        public bool echo = false;
        public bool finalsparkChannel = false;
        public bool flameHarbinger = false;
        public bool frostHarbinger = false;
        public bool garrison = false;
        public bool gathFire = false;
        public bool ghosted = false;
        public bool grievousWounds = false;
        public bool highlander = false;
        public bool lightningRush = false;
        public bool minions = false;
        public bool onslaught = false;
        public bool projectileDodge = false;
        public bool rally = false;
        public bool rejuvenation = false;
        public bool rightoftheArcaneChannel = false;
        public bool spinningAxe = false;
        public bool stopWatchActive = true;
        public bool slowed = false;
        public bool stonePlating = false;
        public bool stunned = false;
        public bool surge = false;
        public bool tidecallersBlessing = false;
        public bool toxicShot = false;
        public bool trueInvis = false;
        public bool invincible = false;
        public bool forDemacia = false;
        public bool deathFromBelowRefresh = false;
        public bool greymark = false;
        public bool greymarkBuff = false;
        public bool sunAmulet = false;
        public int sunAmuletDamage = 0;
        public bool flashofBrilliance = false;
        public int flashofBrillianceCooldown = 0;
        public bool hextechEvolutionSet = false;
        public int hextechEvoltionCooldown = 0;
        public Vector2 hextechEvolutionAngle = Vector2.Zero;
        public bool immolate = false;
        public bool excessiveForce = false;
        public bool celestialFrostbite = false;
        public bool chargerBlessing = false;
        public bool scourgeBlessing = false;
        public bool bottleOfStardust = false;
        public bool bottleOfStarDustBuffer = false;
        public bool targonArena = false;

        // Lifeline Garbage
        public bool LifeLineHex = false;
        public bool LifeLineMaw = false;
        public bool LifeLineSteraks = false;

        // May Need Deleting
        public bool excited = false;
        public bool gathering1 = false;
        public bool gathering2 = false;
        public bool gathering3 = false;

        // Feast Stacks
        public bool feast1 = false;
        public bool feast2 = false;
        public bool feast3 = false;
        public int feastStacks = 0;

        // Umbral Trespass Stuff
        public bool umbralTrespassing = false;
        public NPC umbralTaggedNPC;
        public Player umbralTaggedPlayer;
        public bool taggedIsNPC = true;

        // Starfire Spellblade Ascension
        public int AscensionTimer = 0;
        public int AscensionStacks = 0;

        // Whisper Shots
        public int WhisperShotsLeft = 4;
        public int DestinyShotsLeft = 2;
        public int ReloadTimer = 0;

        // Echoing Flames cooldowns
        public int echoingFlames_LT = 0;
        public int echoingFlames_LM = 0;
        public int echoingFlames_LB = 0;
        public int echoingFlames_RB = 0;
        public int echoingFlames_RM = 0;
        public int echoingFlames_RT = 0;

        // Lunari Gun Stuff
        public int currentGun = 0;
        public float calibrumAmmo = 100;
        public float severumAmmo = 100;
        public float gravitumAmmo = 100;
        public float infernumAmmo = 100;
        public float crescendumAmmo = 100;

        // Requiem Stuff
        public bool requiem = false;
        public bool requiemChannel = false;
        public List<NPC> TaggedNPC;
        public List<Player> TaggedPlayer;
        public int requiemDamage = 0;
        public int requiemChannelTime = 0;
        public int requiemTime = 0;

        // Accessories
        public int[] accessoryCooldown = new int[6];
        public double[] accessoryStat = new double[6];

        #region Starting Items
        public bool dblade = false;
        public bool dring = false;
        public bool dsheild = false;
        public bool darkSeal = false;
        public int darkSealStacks = 0;
        #endregion

        #region Boots
        // Boots of Speed
        public bool T1Boots = false;
        // Hermes Boots
        public bool T2Boots = false;
        // Specter Boots
        public bool T3Boots = false;
        // Lightning Boots
        public bool T4Boots = false;
        // Frostspark
        public bool T5Boots = false;

        public bool swifties = false;
        #endregion

        #region Basic Items
        #endregion

        #region Advanced Items
        public bool spellblade = false;
        public bool spellBladeBuff = false;
        public bool EnergizedShard = false;
        # endregion

        #region Complete Items
        public bool triForce = false;
        public bool icyZone = false;
        public bool windsFury = false;
        public bool windFuryReplicator = false;
        public int windsFuryCooldown = 0;
        public bool windPower = false;
        public bool Disruption = false;
        public bool angelsProtection = false;
        public bool cleaveBasic = false;
        public bool cleaveMaxLife = false;
        public bool cleaveLifesteal = false;
        public bool nightStalker = false;
        public bool guinsoosRage = false;
        public int absorbtionTimer = 0;
        public int cleaveCooldown = 0;
        public int cauterizedDamage = 0;
        public int lifeLineCooldown;
        public bool manaCharge = false;
        public bool truemanaCharge = false;
        public int manaChargeStacks = 0;
        public bool awe = false;
        public bool arcanePrecision = false;
        public bool haunted = false;


        // Energized Items
        public bool energized = false;
        public bool EnergizedDischarge = false;
        public bool EnergizedDetonate = false;
        public bool EnergizedStorm = false;

        public int madnessTimer = 0;
        public bool rawPower = false;
        public bool veil = false;
        public bool warmogsHeart = false;
        public int rageTimer = 0;
        public bool summonedBlade = false;
        public bool critEdge = false;
        public bool spiritualRestur = false;
        public bool ardentsFrenzy = false;
        public bool rapids = false;
        public bool bloodShield = false;
        public bool bloodPool = false;
        #endregion

        public override void ResetEffects()
        {
            ResetShieldStuff();
            ResetCustomStats();


            #region Buffs
            bioBarrage = false;
            crushingBlows = false;
            contactDodge = false;
            deadlyPlumage = false;
            deathLotus = false;
            decisiveStrike = false;
            echo = false;
            finalsparkChannel = false;
            flameHarbinger = false;
            frostHarbinger = false;
            garrison = false;
            gathFire = false;
            ghosted = false;
            grievousWounds = false;
            highlander = false;
            lightningRush = false;
            minions = false;
            projectileDodge = false;
            onslaught = false;
            rally = false;
            rejuvenation = false;
            requiem = false;
            requiemChannel = false;
            rightoftheArcaneChannel = false;
            slowed = false;
            spinningAxe = false;
            stonePlating = false;
            stunned = false;
            surge = false;
            tidecallersBlessing = false;
            toxicShot = false;
            trueInvis = false;
            umbralTrespassing = false;
            invincible = false;
            forDemacia = false;
            deathFromBelowRefresh = false;
            greymark = false;
            greymarkBuff = false;
            sunAmulet = false;
            immolate = false;
            excessiveForce = false;
            celestialFrostbite = false;
            chargerBlessing = false;
            scourgeBlessing = false;
            bottleOfStardust = bottleOfStarDustBuffer;
            bottleOfStarDustBuffer = false;
            rapids = false;
            targonArena = false;

            pirateSet = false;
            cannonSet = false;
            petriciteSet = false;
            prophetSet = false;
            hextechEvolutionSet = false;
            voidbornSet = false;

            if (!solariSet)
                solariCharge = 0;
            solariSet = false;
            solarStorm = false;

            excited = false;
            gathering1 = false;
            gathering2 = false;
            gathering3 = false;
            feast1 = false;
            feast2 = false;
            feast3 = false;
            #endregion

            // Costumes
            darkinCostume = false;
            darkinCostumeHideVanity = false;
            darkinCostumeForceVanity = false;

            #region Accessories
            // Starting
            dblade = false;
            dring = false;
            dsheild = false;
            if (!darkSeal)
                darkSealStacks = 0;
            darkSeal = false;

            //Boots
            T1Boots = false;
            T2Boots = false;
            T3Boots = false;
            T4Boots = false;
            T5Boots = false;
            swifties = false;

            // Basic


            // Advanced
            spellblade = false;
            spellBladeBuff = false;
            truemanaCharge = manaCharge;
            manaCharge = false;
            awe = false;

            // Complete
            summonedBlade = false;
            cleaveBasic = false;
            cleaveMaxLife = false;
            cleaveLifesteal = false;

            EnergizedShard = false;
            EnergizedDischarge = false;
            EnergizedDetonate = false;
            EnergizedStorm = false;
            energized = false;

            LifeLineHex = false;
            LifeLineMaw = false;
            LifeLineSteraks = false;

            windsFury = false;
            windFuryReplicator = false;
            windPower = false;
            triForce = false;
            icyZone = false;
            Disruption = false;
            rawPower = false;
            warmogsHeart = false;
            critEdge = false;
            ardentsFrenzy = false;
            guinsoosRage = false;
            arcanePrecision = false;
            haunted = false;

            if (spiritualRestur)
                hasSpiritualRestorationLastStep = true;
            else
                hasSpiritualRestorationLastStep = false;
            spiritualRestur = false;

            angelsProtection = false;

            nightStalker = false;

            veil = false;
            bloodShield = false;
            bloodPool = false;
            #endregion

            for (int i = 1; i < 6; i++)
            {
                switch (i)
                {
                    case 1:
                        currentGun = player.HasItem(ItemType<Calibrum>()) ? 1 : 0;
                        break;
                    case 2:
                        currentGun = player.HasItem(ItemType<Severum>()) ? 2 : 0;
                        break;
                    case 3:
                        currentGun = player.HasItem(ItemType<Gravitum>()) ? 3 : 0;
                        break;
                    case 4:
                        currentGun = player.HasItem(ItemType<Infernum>()) ? 4 : 0;
                        break;
                    case 5:
                        currentGun = player.HasItem(ItemType<Crescendum>()) ? 5 : 0;
                        break;
                    default:
                        currentGun = 0;
                        break;
                }

                if (currentGun != 0)
                    break;
            }
        }

        public void ResetCustomStats()
        {
            TrueMinionDamage = 0;
            consumeAmmo = 0;
            rangedAttackSpeed = 1;
            abilityHaste = 0;
            itemHaste = 0;
            summonerHaste = 0;
            ultHaste = 0;
            healPower = 1;
            armor = 0;
            resist = 0;
            manaRegenModifer = 1;
            manaRegen = 1;
            extraSumCDR = 0;

            if (lifeStealMagic == 0 && lifeStealMelee == 0 && lifeStealRange == 0 && lifeStealMinion == 0)
                lifeStealCharge = 0;

            lifeStealMelee = 0;
            lifeStealRange = 0;
            lifeStealMagic = 0;
            lifeStealMinion = 0;
            damageTakenModifier = 1;
            healthModifier = 1;

            BonusMEL = 0;
            BonusRNG = 0;
            BonusMAG = 0;
            BonusSUM = 0;

            // Flat Bonus Damage
            meleeFlatDamage = 0;
            rangedFlatDamage = 0;
            magicFlatDamage = 0;
            minionFlatDamage = 0;

            // Damage Modifiers
            meleeModifer = 1;
            rangedModifer = 1;
            magicModifer = 1;
            minionModifer = 1;

            // Armor Pen
            meleeArmorPen = 0;
            rangedArmorPen = 0;
            magicArmorPen = 0;
            minionArmorPen = 0;

            // On Hit Damage
            meleeOnHit = 0;
            rangedOnHit = 0;
            magicOnHit = 0;
            minionOnHit = 0;

            // Stat Scaling
            meleeStatScaling = 1;
            rangedStatScaling = 1;
            magicStatScaling = 1;
            minionStatScaling = 1;
        }

        public override void UpdateDead()
        {
            ResetCustomStats();

            lifeStealCharge = 0;
            requiem = false;
            requiemChannel = false;
            slowed = false;
            umbralTrespassing = false;
            spellBladeBuff = false;
            excited = false;
            gathering1 = false;
            gathering2 = false;
            gathering3 = false;
            ghosted = false;
            finalsparkChannel = false;
            feast1 = false;
            feast2 = false;
            feast3 = false;
            feastStacks = 0;
            meleeProjCooldown = false;

            angelsProtection = false;
            nightStalker = false;
            absorbtionTimer = 0;
            madnessTimer = 0;
            AscensionTimer = 0;
            AscensionStacks = 0;
            CombatTimer = 240;
            manaLastStep = 0;
            rageTimer = 0;
            cauterizedDamage = 0;
            veil = false;

            ClearShields();

            SummonerCooldowns();
            AbilityCooldownsAndStuff();

            if (player.whoAmI == Main.myPlayer)
            {
                if (sumSpells[0].Name == "ReviveRune" && TerraLeague.Sum1.JustPressed && sumCooldowns[0] == 0)
                    UseSummonerSpell(1);

                if (sumSpells[1].Name == "ReviveRune" && TerraLeague.Sum2.JustPressed && sumCooldowns[1] == 0)
                    UseSummonerSpell(2);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnEnterWorld(Player player)
        {
            mod.Logger.Debug("Player: " + player.name);
            for (int i = 0; i < sumSpells.Length; i++)
            {
                if (i == 0)
                {
                    if (initSum1 != null)
                        sumSpells[i] = initSum1;
                    else
                        sumSpells[i] = (SummonerSpell)GetInstance<BarrierRune>();

                    mod.Logger.Debug("OnEnterWorld: set Sum 1 to " + sumSpells[i].Name);
                    initSum1 = null;
                }
                else if (i == 1)
                {
                    if (initSum2 != null)
                        sumSpells[i] = initSum2;
                    else
                        sumSpells[i] = (SummonerSpell)GetInstance<GhostRune>();

                    mod.Logger.Debug("OnEnterWorld: set Sum 2 to " + sumSpells[i].Name);
                    initSum2 = null;
                }
            }
        }

        public override TagCompound Save()
        {
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                mod.Logger.Debug("Player: " + player.name);
                mod.Logger.Debug("Save: Completed Save with Sums " + sumSpells[0] + " and " + sumSpells[1]);
                mod.Logger.Debug("Player: Active = " + player.active);

                if (player.active)
                {
                    return new TagCompound
                    {
                        {"manaChargeStacks", manaChargeStacks},
                        {"sumSpellOne", sumSpells[0].GetType().Name},
                        {"sumSpellTwo", sumSpells[1].GetType().Name},
                        {"blessingCooldown", blessingCooldown},
                    };
                }
                else
                {
                    return new TagCompound
                    {
                        {"manaChargeStacks", manaChargeStacks},
                        {"sumSpellOne", "BarrierRune"},
                        {"sumSpellTwo", "GhostRune"},
                        {"blessingCooldown", blessingCooldown},
                    };
                }

            }
            return null;
        }

        public override void Load(TagCompound tag)
        {
            if (player.name == "TestDude")
            {

            }
            if (Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                manaChargeStacks = tag.GetInt("manaChargeStacks");
                initSum1 = (SummonerSpell)mod.GetItem(tag.GetString("sumSpellOne"));
                initSum2 = (SummonerSpell)mod.GetItem(tag.GetString("sumSpellTwo"));
                blessingCooldown = tag.GetInt("blessingCooldown");

                mod.Logger.Debug("Player: " + player.name);
                mod.Logger.Debug("Load: Completed Load"/* with Sums " + initSum1.Name + " and " + initSum2.Name*/);
            }
        }
        
        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            Item bag = new Item();
            bag.SetDefaults(ItemType<DoransBag>());
            items.Add(bag);

            Item weapon = new Item();
            weapon.SetDefaults(ItemType<WeaponKit>());
            items.Add(weapon);
        }

        public override void UpdateBiomes()
        {
            zoneTargonPeak = false;
            zoneTargon = false;

            if (NPC.CountNPCS(NPCType<TargonSigil>()) != 0)
            {
                NPC sigil = Main.npc.FirstOrDefault(x => x.type == NPCType<TargonSigil>());
                if (sigil != null)
                {
                    zoneTargonPeak = sigil.Distance(player.MountedCenter) <= Main.worldSurface * 0.3 * 16;
                    if (sigil.Distance(player.MountedCenter) <= Main.worldSurface * 0.4 * 16 && !Main.hardMode && NPC.CountNPCS(NPCType<TargonBoss>()) <= 0)
                    {
                        player.AddBuff(BuffType<CelestialFrostbite>(), 2);
                    }

                    if (Math.Abs(sigil.Center.X - player.MountedCenter.X) < Main.maxTilesX / 21 * 16 && Math.Abs(sigil.Center.Y - player.MountedCenter.Y) < Main.worldSurface * 16)
                    {
                        zoneTargon = true;
                    }
                }
            }

            zoneSurfaceMarble = (TerraLeagueWORLDGLOBAL.marbleBlocks > 300);
            if (zoneSurfaceMarble)
            {
                nPCSpawnInfo.marble = true;
            }

            zoneBlackMist = ((player.ZoneBeach && !Main.dayTime && Main.moonPhase == 4) || (TerraLeagueWORLDGLOBAL.BlackMistEvent && player.ZoneOverworldHeight));

            if (zoneBlackMist)
            {
                player.blind = true;
            }


            if (Main.tile[(int)player.MountedCenter.X / 16, (int)player.MountedCenter.Y / 16].wall == (ushort)WallType<Walls.TargonStoneWall_Arena>())
            {
                player.AddBuff(BuffType<InTargonArena>(), 5);
            }
            else if (player.HasBuff(BuffType<InTargonArena>()) && NPC.CountNPCS(NPCType<TargonBoss>()) > 0)
            {
                var ded = new PlayerDeathReason();
                ded.SourceCustomReason = player.name + " tried to run from Targon's Challenge";
                player.KillMe(ded, 999999, 0);
            }
        }

        #region Multiplayer Stuff
        public override void clientClone(ModPlayer clientClone)
        {
            PLAYERGLOBAL clone = clientClone as PLAYERGLOBAL;

            if (clone != null)
            {
                clone.accessoryStat = accessoryStat;
                clone.accessoryStat = accessoryStat;
                clone.MagicShield = MagicShield;
                clone.PhysicalShield = PhysicalShield;
                clone.NormalShield = NormalShield;
                clone.zoneBlackMist = zoneBlackMist;
                clone.zoneSurfaceMarble = zoneSurfaceMarble;
            }
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {

        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            PLAYERGLOBAL oldClone = clientPlayer as PLAYERGLOBAL;

            for (int i = 0; i < accessoryStat.Length; i++)
            {
                if (oldClone.accessoryStat[i] != accessoryStat[i])
                {
                    PacketHandler.SendEquipData(-1, player.whoAmI, accessoryStat[i], i);
                }
            }
            

            if (oldClone.NormalShield != NormalShield)
            {
                PacketHandler.SendShieldTotal(-1, player.whoAmI, player.whoAmI, NormalShield, 0);
            }
            if (oldClone.MagicShield != MagicShield)
            {
                PacketHandler.SendShieldTotal(-1, player.whoAmI, player.whoAmI, MagicShield, 1);
            }
            if (oldClone.PhysicalShield != PhysicalShield)
            {
                PacketHandler.SendShieldTotal(-1, player.whoAmI, player.whoAmI, PhysicalShield, 2);
            }

            if (oldClone.zoneSurfaceMarble != zoneSurfaceMarble)
            {
                PacketHandler.SendBiome(-1, player.whoAmI, player.whoAmI, 0, zoneSurfaceMarble);
            }

            if (oldClone.zoneBlackMist != zoneBlackMist)
            {
                PacketHandler.SendBiome(-1, player.whoAmI, player.whoAmI, 1, zoneBlackMist);
            }
        }
        #endregion

        public override void PreUpdateBuffs()
        {
            base.PreUpdateBuffs();
        }

        public override void PostUpdateRunSpeeds()
        {
            if (T1Boots && player.accRunSpeed < 5)
                player.accRunSpeed = 5;
            if (T2Boots && player.accRunSpeed < 6.5f)
                player.accRunSpeed = 6.5f;
            if (T3Boots && player.accRunSpeed < 6.5f)
                player.accRunSpeed = 6.5f;
            if (T4Boots && player.accRunSpeed < 7)
                player.accRunSpeed = 7;
            if (T5Boots && player.accRunSpeed < 7.5f)
                player.accRunSpeed = 7.5f;

            if (highlander)
            {
                player.maxRunSpeed *= 2.4f;
                player.moveSpeed *= 4f;
            }

            if (ghosted)
            {
                player.accRunSpeed += 5;
                player.maxRunSpeed *= 2;
                player.moveSpeed *= 3;
            }

            if (swifties)
            {
                if (T5Boots)
                    player.accRunSpeed += 3;
                else if (T4Boots)
                    player.accRunSpeed += 2f;
                else if (T3Boots)
                    player.accRunSpeed += 1.5f;
                else if (T2Boots)
                    player.accRunSpeed += 1;
                else
                    player.accRunSpeed += 0.5f;
            }
            base.PostUpdateRunSpeeds();
        }

        public override void UpdateLifeRegen()
        {
            UpdateStats();
            if (player.lifeRegen > 0)
            {
                if (warmogsHeart)
                {
                    if (player.statLifeMax2 >= 600 && player.velocity == Vector2.Zero)
                    {
                        player.lifeRegen += 8;
                        player.lifeRegenTime *= 2;
                    }
                    else
                        player.lifeRegen += 3;
                }
                if (spiritualRestur)
                    player.lifeRegen = (int)(player.lifeRegen * 1.3);
            }

            base.UpdateLifeRegen();
        }

        public override void UpdateBadLifeRegen()
        {
            base.UpdateBadLifeRegen();
            if (celestialFrostbite && !NPC.downedBoss1)
            {
                player.lifeRegenTime = 0;
                if (player.lifeRegen < 0)
                    player.lifeRegen -= 20;
                else
                    player.lifeRegen = -20;
            }
            if (targonArena && !TerraLeagueWORLDGLOBAL.TargonArenaDefeated && NPC.CountNPCS(NPCType<TargonBoss>()) <= 0)
            {
                player.lifeRegenTime = 0;
                if (player.lifeRegen < 0)
                    player.lifeRegen -= 100;
                else
                    player.lifeRegen = -100;
            }

            if (invincible && player.lifeRegen < 0)
            {
                player.lifeRegen = 0;
            }
        }

        /// <summary>
        /// Updates important stats after parsing all equips and buffs
        /// </summary>
        public void UpdateStats()
        {
            player.statLifeMax2 = (int)(GetRealHeathWithoutShield(true) * healthModifier) + GetTotalShield();

            player.meleeDamage *= meleeStatScaling;
            player.rangedDamage *= rangedStatScaling;
            player.magicDamage *= magicStatScaling;
            TrueMinionDamage *= minionStatScaling;
            TrueMinionDamage += (player.minionDamage * minionStatScaling) - player.minionDamage;

            if (player.ammoCost75)
            {
                ConsumeAmmoChance += 0.25;
                player.ammoCost75 = false;
            }

            if (player.ammoCost80)
            {
                ConsumeAmmoChance += 0.2;
                player.ammoCost80 = false;
            }

            if (player.ammoBox)
            {
                ConsumeAmmoChance += 0.2;
                player.ammoBox = false;
            }

            if (player.ammoPotion)
            {
                ConsumeAmmoChance += 0.2;
                player.ammoPotion = false;
            }
        }

        public override void PreUpdate()
        {
            CheckActivesandPassivesAreActive();
            if (stunned)
            {
                player.velocity = Vector2.Zero;
                player.gravity = 0f;
                player.moveSpeed = 0f;
                player.dash = 0;
                player.noKnockback = true;
                player.grappling[0] = -1;
                player.grapCount = 0;
                player.controlJump = false;
                player.controlDown = false;
                player.controlLeft = false;
                player.controlRight = false;
                player.controlUp = false;
                player.controlUseItem = false;
                player.controlUseTile = false;
                player.controlThrow = false;
                player.gravDir = 1f;
            }

            if (lifeStealCharge >= 1)
            {
                if (lifeStealCharge > lifestealMax)
                    lifeStealCharge = lifestealMax;

                int heal = ScaleValueWithHealPower((float)lifeStealCharge);
                
                if (bloodShield && GetRealHeathWithoutShield() >= GetRealHeathWithoutShield(true))
                {
                    player.AddBuff(BuffType<Buffs.BloodShield>(), 180);
                    AddShieldAttachedToBuff((int)(heal), BuffType<Buffs.BloodShield>(), Color.Red, ShieldType.Basic);
                }
                else
                {
                    player.statLife += heal;
                }
                    player.HealEffect(heal);

                lifeStealCharge = 0;
            }
            if (feastStacks >= 500 && feastStacks < 2500)
            {
                player.AddBuff(BuffType<FeastStack1>(), 2);
            }
            else if (feastStacks >= 2500 && feastStacks < 12500)
            {
                player.AddBuff(BuffType<FeastStack2>(), 2);
            }
            else if (feastStacks >= 12500)
            {
                player.AddBuff(BuffType<FeastStack3>(), 2);
                if (feastStacks > 12500)
                    feastStacks = 12500;
            }
            if (umbralTrespassing)
            {
                player.immuneAlpha = 255;
                player.velocity = Vector2.Zero;
                player.gravity = 0;
                if (taggedIsNPC)
                {
                    player.position = umbralTaggedNPC.position;
                    player.position.X = umbralTaggedNPC.position.X + (umbralTaggedNPC.width * 0.5f) - (player.width * 0.5f);
                    player.position.Y = umbralTaggedNPC.position.Y - umbralTaggedNPC.height * 0.5f;

                }

                if (umbralTaggedNPC.life <= 0 && taggedIsNPC)
                {
                    player.ClearBuff(BuffType<UmbralTrespassing>());
                }
            }

            if (requiemChannel)
            {
                Lighting.AddLight(player.Center, 0f, 0.75f, 0.3f);
                Color color = Main.rand.NextBool() ? new Color(0, 255, 140) : new Color(0, 255, 0);
                Dust dust = Dust.NewDustDirect(new Vector2(player.position.X, player.Center.Y - 320), player.width, 400, 186, 0f, -5f, 197, color, 2.5f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity.X *= 0.3f;
                dust.fadeIn = 2.6f;

                player.position = player.oldPosition;
                player.velocity = Vector2.Zero;
                if (requiemChannelTime == 1 && player == Main.LocalPlayer)
                {
                    var npcs = TerraLeague.GetAllNPCsInRange(player.Center, 999999, true, true);

                    for (int i = 0; i < npcs.Count; i++)
                    {
                        NPC npc = Main.npc[npcs[i]];
                        Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y), Vector2.Zero, ProjectileType<DeathsingerTome_Requiem>(), requiemDamage, 0, player.whoAmI, npc.whoAmI);
                    }
                }
            }

            if (finalsparkChannel || deathLotus || rightoftheArcaneChannel)
            {
                player.position = player.oldPosition;
                player.velocity = Vector2.Zero;
            }

            if (player.ownedProjectileCounts[ProjectileType<DarkinScythe_ReapingSlash>()] > 0)
            {
                player.noKnockback = true;
            }

            base.PreUpdate();
        }
        
        public override void PostUpdate()
        {
            if (TerraLeague.instance.debugMode)
            {
                for (int i = 0; i < AbilityCooldowns.Length; i++)
                {
                    if (AbilityCooldowns[i] > 60)
                        AbilityCooldowns[i] = 60;
                }

                for (int i = 0; i < sumCooldowns.Length; i++)
                {
                    sumCooldowns[i] = 0;
                }

                calibrumAmmo = 100;
                severumAmmo = 100;
                gravitumAmmo = 100;
                infernumAmmo = 100;
                crescendumAmmo = 100;

                blessingCooldown = 0;
            }

            if (player.itemTime <= 1 && oldUsedInventorySlot != -1)
            {
                player.selectedItem = oldUsedInventorySlot;
                player.itemLocation = Vector2.Zero;
                oldUsedInventorySlot = -1;
            }

            // Handles the modded regen
            LinearManaRegen();

            // Handles Melee Projectile Cooldowns to look like Vanilla
            MeleeProjectileCooldown();

            // Handles the Revive Summoner Spells effects
            if (reviving)
            {
                //player.Teleport(new Vector2(player.lastDeathPostion.X, player.lastDeathPostion.Y - 32), 1);
                player.HealEffect(player.statLifeMax2);
                player.ManaEffect(player.statManaMax2);
                player.statLife = player.statLifeMax2;
                player.AddBuff(BuffType<Revived>(), ReviveRune.buffDuration * 60);

                ReviveRune.Efx(player);
                SummonerSpell.PacketHandler.SendRevive(-1, player.whoAmI, player.whoAmI);

                //player.ChangeSpawn((int)originalSpawn.X, (int)originalSpawn.Y);
                reviving = false;
            }

            if (shieldFrame >= 24)
                shieldFrame = 0;
            else
                shieldFrame++;

            if (CombatTimer < 240)
            {
                CombatTimer++;
            }

            if (usetime > 0)
            {
                usetime--;
            }
            if (usetime == 0)
            {
                hasHitMelee = false;
            }

            if (blessingCooldown > 0)
                blessingCooldown--;
            if (blessingCooldown == 1)
            {
                Main.NewText("Celestial voices call out to you. Another blessing is ready", 0, 200, 255);
            }

            if (sunAmulet)
            {
                if (Main.time % 60 == 0)
                {
                    float light = Lighting.BrightnessAverage((int)player.position.ToTileCoordinates16().X, (int)player.position.ToTileCoordinates16().Y, 2, 3);
                    sunAmuletDamage = (int)(light * 7);
                }

                player.meleeDamage += sunAmuletDamage * 0.01f;
                player.rangedDamage += sunAmuletDamage * 0.01f;
                player.magicDamage += sunAmuletDamage * 0.01f;
                TrueMinionDamage += sunAmuletDamage * 0.01f;
            }

            if (flashofBrilliance && flashofBrillianceCooldown <= 0)
                player.AddBuff(BuffType<FlashofBrilliance>(), 2);
            if (flashofBrillianceCooldown > 0)
                flashofBrillianceCooldown--;

            // Stopwatch enabler
            if (Main.time == 0 && !stopWatchActive && Main.dayTime)
            {
                stopWatchActive = true;
            }

            // Lunari Ammo Handler
            if (currentGun != 1 && calibrumAmmo < 100)
            {
                calibrumAmmo += 5 / 60f;
                if (calibrumAmmo > 100)
                    calibrumAmmo = 100;
            }
            if (currentGun != 2 && severumAmmo < 100)
            {
                severumAmmo += 5 / 60f;
                if (severumAmmo > 100)
                    severumAmmo = 100;
            }
            if (currentGun != 3 && gravitumAmmo < 100)
            {
                gravitumAmmo += 5 / 60f;
                if (gravitumAmmo > 100)
                    gravitumAmmo = 100;
            }
            if (currentGun != 4 && infernumAmmo < 100)
            {
                infernumAmmo += 5 / 60f;
                if (infernumAmmo > 100)
                    infernumAmmo = 100;
            }
            if (currentGun != 5 && crescendumAmmo < 100)
            {
                crescendumAmmo += 5 / 60f;
                if (crescendumAmmo > 100)
                    crescendumAmmo = 100;
            }

            // Cannon armor set bonus cooldown
            if (cannonTimer > 0)
            {
                cannonTimer--;
            }

            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                // Prophet set bonus cooldown
                if (prophetTimer > 0)
                {
                    prophetTimer--;
                }
                else if (prophetSet)
                {
                    TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 103, -0.25f);

                    for (int i = 0; i < player.maxMinions; i++)
                    {
                        Projectile.NewProjectile(player.MountedCenter, new Vector2(Main.rand.NextFloat(-4, 4), -6), ProjectileType<VoidProphetsStaff_Zzrot>(), (int)(20 * minionDamageLastStep), 1, player.whoAmI);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 27, 0, -3);
                    }

                    prophetTimer = 60 * 6;
                }

                // Hextech Evolution set bonus
                if (hextechEvoltionCooldown > 0)
                    hextechEvoltionCooldown--;


                if (hextechEvolutionSet && hextechEvoltionCooldown <= 0)
                {
                    float distance = 800;
                    int target = -1;
                    Vector2 handPos = player.MountedCenter + new Vector2(player.direction * -10, -16);

                    for (int k = 0; k < 200; k++)
                    {
                        NPC npc = Main.npc[k];
                        if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.immortal)
                        {
                            Vector2 newMove = Main.npc[k].Center - handPos;
                            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                            if (distanceTo < distance && Collision.CanHit(handPos, 4, 4, npc.position, npc.width, npc.height))
                            {
                                distance = distanceTo;
                                target = k;
                            }
                        }
                    }
                    if (target != -1)
                    {
                        hextechEvolutionAngle = TerraLeague.CalcVelocityToPoint(handPos, Main.npc[target].Center, 8).RotatedBy(-0.01f * 20);
                        Projectile.NewProjectile(handPos, hextechEvolutionAngle, ProjectileType<EvolutionSet_Lazer>(), (int)(30 * player.magicDamage), 0, player.whoAmI);
                    }
                    else
                    {
                        hextechEvolutionAngle = Vector2.Zero;
                    }

                    hextechEvoltionCooldown = 90;
                }
                else if (hextechEvolutionSet && hextechEvoltionCooldown > 50 && hextechEvolutionAngle != Vector2.Zero)
                {
                    Vector2 handPos = player.MountedCenter + new Vector2(player.direction * -12, -16).RotatedBy(player.fullRotation);
                    hextechEvolutionAngle = hextechEvolutionAngle.RotatedBy(0.01f);
                    Projectile.NewProjectile(handPos, hextechEvolutionAngle, ProjectileType<EvolutionSet_Lazer>(), (int)(30 * player.magicDamage), 0, player.whoAmI, hextechEvoltionCooldown % 20);
                }
            }
            
            // Solari set bonus
            if (solariSet)
            {
                Lighting.AddLight((int)(player.position.X + (float)(player.width / 2)) / 16, (int)(player.position.Y + (float)(player.height / 2)) / 16, 0.8f, 0.95f, 1f);
                if (Main.dayTime)
                {
                    player.lifeRegen += 2;
                    player.statDefense += 4;
                    player.meleeSpeed += 0.1f;
                    player.meleeDamage += 0.1f;
                    player.meleeCrit += 2;
                    player.rangedDamage += 0.1f;
                    player.rangedCrit += 2;
                    player.magicDamage += 0.1f;
                    player.magicCrit += 2;
                    player.pickSpeed -= 0.15f;
                    TrueMinionDamage += 0.1f;
                    player.minionKB += 0.5f;

                    if (solariCharge < solariMaxCharge)
                        solariCharge++;
                    else
                        player.AddBuff(BuffType<SolarFlareCharged>(), 2);

                    if(solariCharge == solariMaxCharge - 1)
                    {
                        Main.PlaySound(SoundID.MaxMana, -1, -1, 1, 1f, 0f);
                        for (int num225 = 0; num225 < 12; num225++)
                        {
                            int num226 = Dust.NewDust(player.position, player.width, player.height, DustID.AmberBolt, 0f, 0f, 0, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                            Main.dust[num226].noGravity = true;
                            Dust obj2 = Main.dust[num226];
                            obj2.velocity *= 0.5f;
                        }
                    }
                }
            }

            // Starfire Spellblade stack handler
            if (CombatTimer >= 240 /*|| player.HeldItem.type != ItemType<StarfireSpellblades>()*/)
            {
                AscensionTimer = 0;
                AscensionStacks = 0;
            }
            if (CombatTimer < 240 && AscensionStacks < 6 && player.HeldItem.type == ItemType<StarfireSpellblades>())
            {
                AscensionTimer++;
                if (AscensionTimer >= 30)
                {
                    AscensionTimer = 0;
                    AscensionStacks++;
                }
            }

            // Whisper reload handler
            if (ReloadTimer > 0)
            {
                ReloadTimer--;
                if (ReloadTimer == 0)
                {
                    WhisperShotsLeft = 4;
                    DestinyShotsLeft = 2;
                }
            }

            #region Echoing Flames Cooldown
            if (echoingFlames_LB > 0)
                echoingFlames_LB--;
            if (echoingFlames_LM > 0)
                echoingFlames_LM--;
            if (echoingFlames_LT > 0)
                echoingFlames_LT--;
            if (echoingFlames_RT > 0)
                echoingFlames_RT--;
            if (echoingFlames_RM > 0)
                echoingFlames_RM--;
            if (echoingFlames_RB > 0)
                echoingFlames_RB--;
            #endregion

            if (highlander)
            {
                player.armorEffectDrawShadow = true;
            }

            // Dusts
            if (tidecallersBlessing)
            {
                if ((Main.time % 2) == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 pos = new Vector2(32, 0).RotatedBy((MathHelper.TwoPi * i / 3) + (MathHelper.TwoPi * (Main.time % 120) / 120));

                        Dust dust = Dust.NewDustPerfect(player.MountedCenter + pos, DustType<Dusts.BubbledBubble>(), null, 0, default, 1.5f);
                        dust.noLight = true;
                        dust.noGravity = true;
                        dust.velocity *= 0.0f;
                    }
                }
            }
            if (ghosted)
            {
                for (int i = 0; i < player.velocity.Length()/7f; i++)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 111, 0, 0, 0, default, player.velocity.Length() / 5f);
                    dust.noLight = true;
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                }
                
            }
            if (gathFire)
            {
                player.armorEffectDrawOutlines = true;
                Lighting.AddLight(player.Center, new Vector3(0.1f, 0.6f, 0.8f));
                if (Main.rand.Next(0,6) == 0)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 261, 0, 0, 0, new Color(15, 170, 200));
                    dust.velocity.X = 0;
                    dust.velocity.Y -= 2;
                    dust.noGravity = true;
                }
            }
            if (angelsProtection)
            {
                player.armorEffectDrawShadow = true;
                if (Main.rand.Next(0, 5) == 0)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 16, 0, 0, 0, new Color(255, 255, 255, 150));
                    dust.noGravity = true;
                }
            }
            if (nightStalker)
            {
                if (Main.rand.Next(0, 5) == 0)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 117, 0, 0, 0, new Color(50, 0, 0, 200), 1.3f);
                    dust.velocity *= 0.5f;
                    dust.alpha = 40;
                    dust.noGravity = true;
                }
            }
            if (surge)
            {
                if (Main.rand.Next(0, 3) == 0)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 261, 0, 0, 0, new Color(255, 0, 0, 255));
                    dust.velocity.X = 0;
                    dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
            if (garrison)
            {
                if (Main.rand.Next(0, 3) == 0)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 261, 0, 0, 0, new Color(131, 234, 46, 255));
                    dust.velocity.X = 0;
                    dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
            if (rejuvenation)
            {
                if (Main.rand.Next(0, 3) == 0)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 263, 0, 0, 0, new Color(248, 137, 89), 1f);
                    dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                    dust.noGravity = true;
                    dust.noLight = true;

                    dust = Dust.NewDustDirect(player.position, player.width, player.height, 263, 0, 0, 0, new Color(237, 137, 164), 1);
                    dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                    dust.noGravity = true;
                    dust.noLight = true;
                }
            }
            if (spinningAxe)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 211, 0, 0, 0, new Color(255, 0, 0));
                dust.noGravity = true;
                dust.scale = 1.4f;
            }
            if (flameHarbinger)
            {
                int displacement = Main.rand.Next(30);

                for (int i = 0; i < 12; i++)
                {
                    Vector2 pos = new Vector2(30, 0).RotatedBy(MathHelper.ToRadians((30 * i) + displacement)) + player.Center;

                    Dust dustR = Dust.NewDustPerfect(pos, DustID.Fire, Vector2.Zero, 0, default(Color), 2f);
                    dustR.noGravity = true;
                }
            }
            if (bioBarrage)
            {
                Dust dustIndex = Dust.NewDustDirect(player.position, player.width, player.height, 167, 0, -4, 50);
                dustIndex.velocity *= 0.3f;
            }
            if (toxicShot)
            {
                Dust dustIndex = Dust.NewDustDirect(player.position, player.width, player.height, 186, 0, -4, 50);
                dustIndex.velocity *= 0.3f;
            }
            if (immolate)
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 pos = new Vector2(player.position.X, player.position.Y + (player.height * 0.9f));
                    Dust dustIndex = Dust.NewDustDirect(pos, player.width, player.height / 10, 6, 12f, -1f, 100, default(Color), 1.25f);
                    dustIndex.noGravity = true;
                    dustIndex.velocity.Y *= 0.4f;
                    dustIndex.velocity.X *= 0.6f;
                    dustIndex.velocity.X += player.velocity.X;
                    dustIndex.noLight = true;
                    Dust dustIndex2 = Dust.NewDustDirect(pos, player.width, player.height / 10, 6, -12f, -1f, 100, default(Color), 1.5f);
                    dustIndex2.noGravity = true;
                    dustIndex2.noLight = true;
                    dustIndex2.velocity.Y *= 0.4f;
                    dustIndex2.velocity.X *= 0.6f;
                    dustIndex2.velocity.X += player.velocity.X;
                }
            }

            // Lifeline cooldown handler
            if (lifeLineCooldown > 0)
                lifeLineCooldown--;

            // Cauterized Wounds damage Handler
            if (Main.time % 60 == 1 && cauterizedDamage > 0)
            {
                PlayerDeathReason ded = new PlayerDeathReason();
                ded.SourceCustomReason = player.name + " died to their wounds";

                if (cauterizedDamage < 6)
                {
                    player.statLife -= (cauterizedDamage * 2) / 3;
                    CombatText.NewText(player.Hitbox, Color.DarkRed, (cauterizedDamage * 2) / 3, false, true);
                    //player.Hurt(ded, cauterizedDamage + (int)(player.statDefense * (Main.expertMode ? 0.75 : 0.5)), 0, false, true, false, 0);
                    cauterizedDamage = 0;
                }
                else
                {
                    player.statLife -= (cauterizedDamage * 2) / 3;
                    CombatText.NewText(player.Hitbox, Color.DarkRed, (cauterizedDamage * 2) / 3, false, true);
                    cauterizedDamage = (cauterizedDamage * 2) / 3;
                    //player.Hurt(ded, (cauterizedDamage / 3) + (int)(player.statDefense * (Main.expertMode ? 0.75 : 0.5)), 0, false, true, false, 0);
                }

                if (GetRealHeathWithoutShield() <= 0)
                    player.KillMe(ded, 0, 1, false);
            }

            // Healing handler
            if (lifeToHeal > 0)
            {
                HealLife();
            }

            // Double Tap Actions
            if (player.doubleTapCardinalTimer[0] > 0 && player.doubleTapCardinalTimer[0] < 15)
            {
                if (player.releaseDown)
                {
                    if (solariSet && solariCharge >= solariMaxCharge)
                    {
                        solariCharge = 0;
                        player.AddBuff(BuffType<SolarFlareStorm>(), 360);
                    }
                }
            }

            // Ability and Summoner Spells Handler
            if (player.whoAmI == Main.myPlayer)
            {
                if (!player.silence && !player.noItems)
                {
                    if (TerraLeague.Item1.JustPressed)
                    {
                        LeagueItem item = player.armor[3].modItem as LeagueItem;
                        if (item != null)
                            if (item.Active != null)
                                item.Active.DoActive(player, item);
                    }
                    if (TerraLeague.Item2.JustPressed)
                    {
                        LeagueItem item = player.armor[4].modItem as LeagueItem;
                        if (item != null)
                            if (item.Active != null)
                                item.Active.DoActive(player, item);
                    }
                    if (TerraLeague.Item3.JustPressed)
                    {
                        LeagueItem item = player.armor[5].modItem as LeagueItem;
                        if (item != null)
                            if (item.Active != null)
                                item.Active.DoActive(player, item);
                    }
                    if (TerraLeague.Item4.JustPressed)
                    {
                        LeagueItem item = player.armor[6].modItem as LeagueItem;
                        if (item != null)
                            if (item.Active != null)
                                item.Active.DoActive(player, item);
                    }
                    if (TerraLeague.Item5.JustPressed)
                    {
                        LeagueItem item = player.armor[7].modItem as LeagueItem;
                        if (item != null)
                            if (item.Active != null)
                                item.Active.DoActive(player, item);
                    }
                    if (TerraLeague.Item6.JustPressed)
                    {
                        LeagueItem item = player.armor[8].modItem as LeagueItem;
                        if (item != null)
                            if (item.Active != null)
                                item.Active.DoActive(player, item);
                    }
                }

                if (TerraLeague.QAbility.Current && Abilities[0] != null)
                    if (Abilities[0].CanCurrentlyBeCast(player))
                        Abilities[0].DoEffect(player, AbilityType.Q);
                        //UseAbility(AbilityType.Q);

                if (TerraLeague.WAbility.Current && Abilities[1] != null)
                    if (Abilities[1].CanCurrentlyBeCast(player))
                        Abilities[1].DoEffect(player, AbilityType.W);
                //UseAbility(AbilityType.W);

                if (TerraLeague.EAbility.Current && Abilities[2] != null)
                    if (Abilities[2].CanCurrentlyBeCast(player))
                        Abilities[2].DoEffect(player, AbilityType.E);
                //UseAbility(AbilityType.E);

                if (TerraLeague.RAbility.Current && Abilities[3] != null)
                    if (Abilities[3].CanCurrentlyBeCast(player))
                        Abilities[3].DoEffect(player, AbilityType.R);
                //UseAbility(AbilityType.R);

                if (!player.silence)
                {
                    if (TerraLeague.Sum1.JustPressed && sumSpells[0] != null && sumCooldowns[0] == 0 /*&& canUseSummoner*/)
                    {
                        UseSummonerSpell(1);
                    }
                    if (TerraLeague.Sum2.JustPressed && sumSpells[1] != null && sumCooldowns[1] == 0/*&& canUseSummoner*/)
                    {
                        UseSummonerSpell(2);
                    }
                }
            }

            #region Ability Animation Junk
            //if (abilityAnimation > 0)
            //    abilityAnimation--;
            //if (abilityAnimation <= 0)
            //{
            //    abilityAnimation = 0;
            //    abilityAnimationMax = 0;
            //    abilityItem = null;
            //    abilityAnimationType = 0;
            //    abilityItemPosition = Vector2.Zero;
            //}
            #endregion

            // Runs PostPlayerUpdate() for all equiped LeagueItems
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                LeagueItem.RunEnabled_PostPlayerUpdate(player);
            }

            // Handles Summoner Spell cooldowns
            SummonerCooldowns();
            // Handles Ability Cooldowns
            AbilityCooldownsAndStuff();

            healPowerLastStep = healPower;
            meleeDamageLastStep = Math.Round(player.meleeDamage + player.allDamage - 1, 2);
            rangedDamageLastStep = Math.Round(player.rangedDamage + player.allDamage - 1, 2);
            magicDamageLastStep = Math.Round(player.magicDamage + player.allDamage - 1 - player.manaSickReduction, 2);
            minionDamageLastStep = Math.Round(player.minionDamage + player.allDamage - 1, 2) + TrueMinionDamage;
            rocketDamageLastStep = Math.Round(player.rocketDamage, 2);
            arrowDamageLastStep = Math.Round(player.arrowDamage, 2);
            bulletDamageLastStep = Math.Round(player.bulletDamage, 2);
            defenceLastStep = player.statDefense;
            armorLastStep = armor;
            resistLastStep = resist;
            abilityHasteLastStep = abilityHaste;
            itemHasteLastStep = itemHaste;
            summonerHasteLastStep = summonerHaste;
            ultHasteLastStep = ultHaste;
            extraSumCDRLastStep = Math.Round(extraSumCDR, 2);
            maxMinionsLastStep = player.maxMinions;
            maxLifeLastStep = GetRealHeathWithoutShield(true);

            SetShieldColor();
            base.PostUpdate();
        }

        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
        {
            if (darkinCostume)
                player.AddBuff(BuffType<DarkinBuff>(), 2, true);

            base.UpdateEquips(ref wallSpeedBuff, ref tileSpeedBuff, ref tileRangeBuff);
        }

        public override void UpdateVanityAccessories()
        {
            for (int n = 13; n < 18 + player.extraAccessorySlots; n++)
            {
                Item item = player.armor[n];
                if (item.type == ItemType<Items.Accessories.DarkinArtifact>())
                {
                    darkinCostumeHideVanity = false;
                    darkinCostumeForceVanity = true;
                }
            }
        }
        
        public override void UpdateBiomeVisuals()
        {
            //bool useVoidMonolith = voidMonolith && !usePurity && !NPC.AnyNPCs(NPCID.MoonLordCore);
            //if (TerraLeague.DrawBlackMistFog)
            //player.ManageSpecialBiomeVisuals("TerraLeague:Targon", zoneTargonMonolith, player.Center);

            bool doTargonEFX = (zoneTargonPeak || zoneTargonMonolith);
            player.ManageSpecialBiomeVisuals("TerraLeague:Targon", doTargonEFX, player.Center);
            player.ManageSpecialBiomeVisuals("TerraLeague:TheBlackMist", zoneBlackMist, player.Center);

            base.UpdateBiomeVisuals();
        }

        /// <summary>
        /// <para>Runs just before the player dies</para>
        /// Return false to prevent the player from dying
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="hitDirection"></param>
        /// <param name="pvp"></param>
        /// <param name="playSound"></param>
        /// <param name="genGore"></param>
        /// <param name="damageSource"></param>
        /// <returns></returns>
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            bool? itemKill = LeagueItem.RunEnabled_PreKill(player, damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
            if (itemKill != null)
            {
                return (bool)itemKill;
            }
            
            if (GetRealHeathWithoutShield() <= 0)
            {
                ClearShields();
                return true;
            }
            ClearShields();
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        /// <summary>
        /// Runs on hitting an NPC with a sword swing.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <param name="crit"></param>
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (item.melee)
                ModifyHitNPCTrue(item, target, ref damage, ref knockback, ref crit);

            base.ModifyHitNPC(item, target, ref damage, ref knockback, ref crit);
        }

        /// <summary>
        /// Runs on hitting an NPC with a projectile
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <param name="crit"></param>
        /// <param name="hitDirection"></param>
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            CombatTimer = 0;
            base.ModifyHitNPCWithProj(proj, target, ref damage, ref knockback, ref crit, ref hitDirection);

            // Some projectiles are considered a melee swing by this mod, and will go on to be treated as such
            if (TerraLeague.IsProjActuallyMeleeAttack(proj) && proj.melee)
            {
                ModifyHitNPCTrue(null, target, ref damage, ref knockback, ref crit);
            }
            else
            {
                TerraLeagueNPCsGLOBAL modNPC = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>();

                int onhitdamage = 0;

                // Adds the mods custom minion damage stat to the modifier to apply the correct damage
                if (TerraLeague.IsMinionDamage(proj) && (!proj.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell && !proj.GetGlobalProjectile<PROJECTILEGLOBAL>().summonAbility))
                {
                    minionModifer *= (float)TrueMinionDamage + 1;
                }
                if (modNPC.abyssalCurse && proj.magic)
                {
                    magicModifer *= 1.08;
                }
                if (modNPC.OrgDest && proj.magic)
                {
                    magicModifer *= 1.1;
                }
                if (modNPC.doomed && proj.magic)
                {
                    magicModifer *= 1.2;
                }

                if (bioBarrage && proj.ranged)
                {
                    int bioonhit = (int)(target.lifeMax * (0.04 + (MAG * 0.0005)));
                    if (bioonhit > Items.Weapons.Abilities.BioArcaneBarrage.GetMaxOnHit(this))
                    {
                        bioonhit = Items.Weapons.Abilities.BioArcaneBarrage.GetMaxOnHit(this);
                    }

                    onhitdamage += bioonhit;
                }

                // Runs NPCHitWithProjectile() for all equiped LeagueItems
                LeagueItem.RunEnabled_NPCHitWithProjectile(player, proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref onhitdamage);
                
                if (tidecallersBlessing)
                {
                    target.AddBuff(BuffType<Slowed>(), 60 * 2);
                }
                if (voidbornSet && proj.minion)
                {
                    player.ManaEffect(2);
                    player.statMana += 2;
                }

                // +-+-+-+-+FINALIZED DAMAGE MODIFIERS+-+-+-+-+

                // Add All Modifiers to the damage type
                if (proj.melee)
                {
                    player.armorPenetration += meleeArmorPen;
                    onhitdamage += meleeOnHit;

                    damage = (int)(damage * meleeModifer);
                    damage += meleeFlatDamage;
                }
                if (proj.ranged)
                {
                    player.armorPenetration += rangedArmorPen;
                    onhitdamage += rangedOnHit;

                    damage = (int)(damage * rangedModifer);
                    damage += rangedFlatDamage;
                }
                if (proj.magic)
                {
                    player.armorPenetration += magicArmorPen;
                    onhitdamage += magicOnHit;
                    damage = (int)(damage * magicModifer);
                    damage += magicFlatDamage;
                }
                if (proj.minion)
                {
                    player.armorPenetration += minionArmorPen;
                    onhitdamage += minionOnHit;

                    damage = (int)(damage * minionModifer);
                    damage += minionFlatDamage;
                }

                if (toxicShot && proj.ranged)
                {
                    onhitdamage += Items.Weapons.Abilities.ToxicShot.GetMaxOnHit(this);
                    target.AddBuff(BuffID.Venom, 240);
                }
                if (proj.melee)
                {
                    if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().frozen)
                    {
                        ShatterEnemy(target, ref damage);
                    }
                }


                // +-+-+-+-+FINALIZED DAMAGE+-+-+-+-+


                if (flameHarbinger)
                    target.AddBuff(BuffType<HarbingersInferno>(), 180);
                if (scourgeBlessing)
                {
                    target.AddBuff(BuffID.CursedInferno, 120);
                    if (bottleOfStardust)
                        target.AddBuff(BuffType<GrievousWounds>(), 120);
                }
                if (!proj.GetGlobalProjectile<PROJECTILEGLOBAL>().noOnHitEffects)
                    FlashOfBrillianceEffect(player, damage, target);

                // Performs Winds Fury (Runanns Hurricane)
                if (proj.ranged && (windsFury || windFuryReplicator) && !TerraLeague.DoNotCountRangedDamage(proj) && windsFuryCooldown == 0)
                {
                    int shotsfired = 0;

                    int target1 = TerraLeague.GetClosestNPC(player.Center, 520, target.whoAmI);
                    int target2 = -1;
                    if (target1 != -1)
                    {
                        target2 = TerraLeague.GetClosestNPC(player.Center, 520, new int[] { target.whoAmI, target1 });
                    }

                    if (windsFury)
                    {
                        if (target1 != -1)
                        {
                            Projectile.NewProjectileDirect(player.MountedCenter, TerraLeague.CalcVelocityToPoint(player.MountedCenter, Main.npc[target1].Center, 4), ProjectileType<Item_RunaansShot>(), (int)(damage * 0.4f), 0, player.whoAmI, target1);
                            if (target2 != -1)
                            {
                                Projectile.NewProjectileDirect(player.MountedCenter, TerraLeague.CalcVelocityToPoint(player.MountedCenter, Main.npc[target2].Center, 4), ProjectileType<Item_RunaansShot>(), (int)(damage * 0.4f), 0, player.whoAmI, target2);
                            }
                        }
                    }
                    else
                    {
                        if (target1 != -1)
                        {
                            Projectile newProj = Projectile.NewProjectileDirect(player.MountedCenter, TerraLeague.CalcVelocityToPoint(player.MountedCenter, Main.npc[target1].Center, 10), proj.type, (int)(damage * 0.4f), 0, player.whoAmI, target.whoAmI);
                            newProj.ranged = false;
                            if (target2 != -1)
                            {
                                newProj = Projectile.NewProjectileDirect(player.MountedCenter, TerraLeague.CalcVelocityToPoint(player.MountedCenter, Main.npc[target2].Center, 10), proj.type, (int)(damage * 0.4f), 0, player.whoAmI, target.whoAmI);
                                newProj.ranged = false;
                            }
                        }
                    }

                    Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 24);
                    shotsfired++;

                    if (target1 != -1)
                        windsFuryCooldown = (int)(15 / rangedAttackSpeed);
                }

                // Lifesteal calculation
                double LifeCharge = 0;
                if (lifeStealMelee > 0 && proj.melee)
                {
                    LifeCharge += lifeStealMelee; //* (damage - (target.defense * 0.5));
                }
                if (lifeStealRange > 0 && proj.ranged)
                {
                    LifeCharge += lifeStealRange;// * (damage - (target.defense * 0.5));
                }
                if (lifeStealMagic > 0 && proj.magic)
                {
                    LifeCharge += lifeStealMagic;// * (damage - (target.defense * 0.5));
                }
                if (lifeStealMinion > 0 && TerraLeague.IsMinionDamage(proj))
                {
                    LifeCharge += lifeStealMinion;// * (damage - (target.defense * 0.5));
                }

                // Modify lifesteal
                if (LifeCharge > 0 && !target.immortal)
                {
                    //if (ProjectileID.Sets.Homing[proj.type])
                    //    LifeCharge /= 3;
                    //if (proj.penetrate != 1)
                    //    LifeCharge /= 3;
                    if (grievousWounds)
                        LifeCharge = 0;
                    lifeStealCharge += LifeCharge;
                }

                // On Hit Damage Calculation
                if (onhitdamage > 0 && Main.rand.NextBool(4))
                {
                    if (proj.melee)
                        onhitdamage = (int)(onhitdamage * 0.75);
                    target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().OnHitDamage(target, player, onhitdamage, 0, 0, (guinsoosRage && (proj.ranged || proj.melee)));
                    player.addDPS(onhitdamage);
                }

                OnKilledEnemy(target, damage, crit);
            }
        }

        /// <summary>
        /// The real method for melee swing damage, modified to handle projectiles if the mod considers them a melee swing
        /// </summary>
        /// <param name="item"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <param name="crit"></param>
        public void ModifyHitNPCTrue(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            CombatTimer = 0;
            int onhitdamage = 0;

            if (decisiveStrike)
            {
                target.AddBuff(BuffType<Slowed>(), 180);
                player.ClearBuff(BuffType<Buffs.DecisiveStrike>());
            }

            // Runs NPCHit() for all equiped LeagueItems
            LeagueItem.RunEnabled_NPCHit(player, item, target, ref damage, ref knockback, ref crit, ref onhitdamage);

            // Damage Buffs
            if (scourgeBlessing)
            {
                target.AddBuff(BuffID.ShadowFlame, 120);
                if (bottleOfStardust)
                    target.AddBuff(BuffType<GrievousWounds>(), 120);
            }
            if (excessiveForce)
                meleeModifer += 3;

            // +-+-+-+-+FINALIZED DAMAGE MODIFIERS+-+-+-+-+

            player.armorPenetration += meleeArmorPen;
            onhitdamage += meleeOnHit;

            damage = (int)(damage * meleeModifer);
            damage += meleeFlatDamage;

            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().frozen)
            {
                ShatterEnemy(target, ref damage);
            }

            // +-+-+-+-+FINALIZED DAMAGE+-+-+-+-+

            if ((cleaveLifesteal || cleaveMaxLife || cleaveBasic) && cleaveCooldown == 0)
            {
                int cleaveDamage = 0;
                var npcs = TerraLeague.GetAllNPCsInRange(player.MountedCenter, 200, true, true);

                if (cleaveLifesteal)
                {
                    Cleave.Efx(player.whoAmI, CleaveType.Lifesteal);
                    Passive.PacketHandler.SendCleave(-1, player.whoAmI, 2, player.whoAmI);
                    cleaveDamage = (int)(MEL * 50 / 100f);
                    cleaveCooldown = 45;
                }
                else if (cleaveMaxLife)
                {
                    Cleave.Efx(player.whoAmI, CleaveType.MaxLife);
                    Passive.PacketHandler.SendCleave(-1, player.whoAmI, 1, player.whoAmI);
                    cleaveDamage = (int)((MEL * 40 / 100f) + (player.statLifeMax2 * 0.05));
                    cleaveCooldown = 45;
                }
                else if (cleaveBasic)
                {
                    Cleave.Efx(player.whoAmI, CleaveType.Basic);
                    Passive.PacketHandler.SendCleave(-1, player.whoAmI, 0, player.whoAmI);
                    cleaveDamage = (int)(MEL * 40 / 100f);
                    cleaveCooldown = 60;
                }

                for (int i = 0; i < npcs.Count; i++)
                {
                    NPC npc = Main.npc[npcs[i]];
                    if (player.CanHit(npc) && npc.whoAmI != target.whoAmI)
                    {
                        player.ApplyDamageToNPC(npc, cleaveDamage, 0, 0, crit);
                        if (cleaveLifesteal && npc.type != NPCID.TargetDummy)
                            lifeToHeal += (int)((cleaveDamage - (npc.defense * 0.5)) * 0.05);
                    }
                }
                damage += cleaveDamage;
            }

            if (tidecallersBlessing)
            {
                target.AddBuff(BuffType<Slowed>(), 60 * 2);
            }
            if (flameHarbinger)
            {
                target.AddBuff(BuffType<HarbingersInferno>(), 180);
            }
            if (excessiveForce)
            {
                player.ClearBuff(BuffType<Buffs.ExcessiveForce>());
                excessiveForce = false;
                float angle = player.AngleTo(Main.MouseWorld);
                for (int i = 0; i < 12; i++)
                {
                    Projectile.NewProjectileDirect(target.Center, new Vector2(12, 0).RotatedBy(angle + MathHelper.ToRadians((-30 + (5 * i)))), ProjectileType<AtlasGauntlets_ExcessiveForce>(), damage, knockback/2, player.whoAmI, target.whoAmI);
                }
            }
            FlashOfBrillianceEffect(player, damage, target);

            // Lifesteal calculation
            if (lifeStealMelee > 0 && !target.immortal)
            {
                double LifeCharge = lifeStealMelee;// * (damage - (target.defense * 0.5));

                if (grievousWounds)
                    LifeCharge = 0;
                if (LifeCharge > 0)
                    lifeStealCharge += LifeCharge;
            }

            // On Hit Damage Calculation
            if (onhitdamage > 0 && Main.rand.NextBool(4))
            {
                target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().OnHitDamage(target, player, onhitdamage, 0, 0, guinsoosRage);
                player.addDPS(onhitdamage);
            }

            OnKilledEnemy(target, damage, crit);
            hasHitMelee = true;
        }

        /// <summary>
        /// Runs when the player is hit by npc contact damage
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="damage"></param>
        /// <param name="crit"></param>
        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            // Multiplies the damage
            damage = (int)(damage * damageTakenModifier);

            // Some projectiles in vanilla Terraria are actually NPCs so they can be hit and killed
            if (TerraLeague.IsEnemyActuallyProj(npc))
            {
                wasHitByProjOrNPCLastStep = "Proj";

                // Runs OnHitByProjectile(npc) for all equiped LeagueItems
                LeagueItem.RunEnabled_OnHitByProjectile(player, npc, ref damage, ref crit);

                ArmorResistScaledDamage(ref damage, false);
                //// Reduces the projectile damage based on Players resist stat
                //if (Main.expertMode)
                //    damage -= (int)(resist * 0.75);
                //else
                //    damage -= (int)(resist * 0.5);
            }
            else
            {
                wasHitByProjOrNPCLastStep = "NPC";

                // Runs OnHitByNPC() for all equiped LeagueItems
                LeagueItem.RunEnabled_OnHitByNPC(player, npc, ref damage, ref crit);

                ArmorResistScaledDamage(ref damage, true);
                //// Reduces the contact damage based on Players armor stat
                //if (Main.expertMode)
                //    damage -= (int)(armor * 0.75);
                //else
                //    damage -= (int)(armor * 0.5);
            }

            OnHitByEnemy(npc, ref damage, crit);
            base.ModifyHitByNPC(npc, ref damage, ref crit);
        }

        /// <summary>
        /// Runs when the player is hit by a projectile
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="damage"></param>
        /// <param name="crit"></param>
        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            // Multiplies the damage
            damage = (int)(damage * damageTakenModifier);

            wasHitByProjOrNPCLastStep = "Proj";

            // Runs OnHitByProjectile(Projectile) for all equiped LeagueItems
            LeagueItem.RunEnabled_OnHitByProjectile(player, proj, ref damage, ref crit);

            // Greymark
            if (greymark)
                player.AddBuff(BuffType<GreymarkBuff>(), 4 * 60);

            ArmorResistScaledDamage(ref damage, false);
            // Reduces the projectile damage based on Players resist stat
            //if (Main.expertMode)
            //    damage -= (int)(resist * 0.75) / 4;
            //else
            //    damage -= (int)(resist * 0.5) / 2;


            OnHitByEnemy(Main.npc[0], ref damage, crit);
            base.ModifyHitByProjectile(proj, ref damage, ref crit);
        }

        void ArmorResistScaledDamage(ref int damage, bool armor = true)
        {
            TerraLeague.Log(armor ? "--CONTACT--" : "--PROJECTILE--", Color.HotPink);
            
            if (armor)
            {
                TerraLeague.Log("Inital: " + damage, Color.MediumSlateBlue);
            }
            else
            {
                TerraLeague.Log("Inital: " + damage * (Main.expertMode ? 4 : 2), Color.MediumSlateBlue);
            }


            if (armor)
            {
                damage = (int)Math.Round(damage * ArmorDamageReduction, 0);
                TerraLeague.Log("Reduction: " + damage + " ~ Percent: " + ArmorDamageReduction, Color.MediumSlateBlue);
            }
            else
            {
                damage = (int)Math.Round(damage * ResistDamageReduction, 0);
                TerraLeague.Log("Reduction: " + damage * (Main.expertMode ? 4 : 2) + " ~ Percent: " + ResistDamageReduction, Color.MediumSlateBlue);
            }

            if (TerraLeague.UseCustomDefenceStat)
            {
                if (armor)
                {
                    if (Main.expertMode)
                        damage += (int)Math.Round(player.statDefense * 0.75);
                    else
                        damage += (int)Math.Round(player.statDefense * 0.5);

                    TerraLeague.Log("PostDef: " + damage + " ~ Added Damage: " + (int)Math.Round(player.statDefense * (Main.expertMode ? 0.75 : 0.5)), Color.MediumSlateBlue);
                    TerraLeague.Log("Expected Result: " + (damage - (int)Math.Round(player.statDefense * (Main.expertMode ? 0.75 : 0.5))), Color.DarkGreen);

                }
                else
                {
                    if (Main.expertMode)
                        damage += (int)Math.Round(player.statDefense * 0.75) / 4;
                    else
                        damage += (int)Math.Round(player.statDefense * 0.5) / 2;

                    double addedDamage = player.statDefense * (Main.expertMode ? 0.75 : 0.5);

                    TerraLeague.Log("PostDef: " + (damage * (Main.expertMode ? 4 : 2)) + " ~ Added Damage: " + (int)Math.Round(addedDamage), Color.MediumSlateBlue);
                    TerraLeague.Log("Expected Result: " + ((damage * (Main.expertMode ? 4 : 2)) - (int)Math.Round(addedDamage)), Color.Green);
                }

            }

        }

        /// <summary>
        /// <para>Checks if the player can be hit by the requested NPC</para>
        /// Return false to deny contact damage
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="cooldownSlot"></param>
        /// <returns></returns>
        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().bubbled || invincible || contactDodge || player.ownedProjectileCounts[ProjectileType<XanCrestBlades_BladeSurge>()] > 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projCheck = Main.projectile[i];
                    if (projCheck.owner == player.whoAmI)
                    {
                        if (projCheck.active && projCheck.GetGlobalProjectile<PROJECTILEGLOBAL>().playerInvincible)
                        {
                            return false;
                        }
                    }
                }
                return base.CanBeHitByNPC(npc, ref cooldownSlot);
            }
        }

        /// <summary>
        /// <para>Checks if the player can be hit by the requested Projectile</para>
        /// Return false to deny projectile damage
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (invincible)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projCheck = Main.projectile[i];
                    if (projCheck.owner == player.whoAmI)
                    {
                        if (projCheck.active && projCheck.GetGlobalProjectile<PROJECTILEGLOBAL>().playerInvincible)
                        {
                            return false;
                        }
                    }
                }
                return base.CanBeHitByProjectile(proj);
            }
        }

        /// <summary>
        /// Checks if you are about to deal enough damage to kill the hit enemy and runs code accordingly
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="damage"></param>
        /// <param name="crit"></param>
        public void OnKilledEnemy(NPC npc, int damage, bool crit)
        {
            if (npc.life - (damage - (npc.defense / 2)) <= 0 || npc.life - (damage - (npc.defense / 2)) * 2 <= 0 && crit)
            {
                // Runs OnKilledNPC() for all equiped LeagueItems
                LeagueItem.RunEnabled_OnKilledNPC(player, npc, ref damage, ref crit);

                // Gives mana if Dorans Ring is equiped
                if (dring && player.statMana < player.statManaMax2)
                {
                    player.ManaEffect(5);
                    player.statMana += 5;
                }

                // Gives the Excited buff if holding the Pow Pow or Fish Bones weapons
                if (player.HeldItem.type == ItemType<PowPow>() || player.HeldItem.type == ItemType<FishBones>())
                {
                    player.AddBuff(BuffType<PowPowExcited>(), 300);
                }
            }
        }

        /// <summary>
        /// Runs when the player is hit by anything
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="crit"></param>
        public void OnHitByEnemy(NPC npc, ref int damage, bool crit)
        {
            //if (GetTotalShield() <= 0)
                player.AddBuff(BuffType<GrievousWounds>(), 180); // 3 seconds
            CombatTimer = 0;
        }

        /// <summary>
        /// Sends a Buff to another player through the server
        /// </summary>
        /// <param name="buff">The buffs ID</param>
        /// <param name="duration">The buffs duration in frames (60 = 1 second)</param>
        /// <param name="target">ID of the player who is recieving the buff</param>
        /// <param name="toWho">Who this information is being sent to (-1 is all players on the server)</param>
        /// <param name="fromWho">Who is sending this information (player.whoAmI)</param>
        internal void SendBuffPacket(int buff, int duration, int target, int toWho, int fromWho)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                PacketHandler.SendBuff(toWho, fromWho, buff, duration, target);
            }
        }

        /// <summary>
        /// Sends healing to another player through the server
        /// </summary>
        /// <param name="healAmount">Amount of healing</param>
        /// <param name="healTarget">ID of the player you are healing</param>
        /// <param name="toWho">Who this information is being sent to (-1 is all playres on the server)</param>
        /// <param name="fromWho">Who is sending this information (player.whoAmI)</param>
        internal void SendHealPacket(int healAmount, int healTarget, int toWho, int fromWho)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (bloodPool)
                {
                    healAmount += (int)GetPassiveStat(new BloodPool(0));
                    FindAndSetPassiveStat(new BloodPool(0), 0);
                }

                if (ardentsFrenzy)
                {
                    SendBuffPacket(BuffType<Frenzy>(), 240, healTarget, toWho, fromWho);
                    player.AddBuff(BuffType<Frenzy>(), 240);
                }

                if (rapids)
                {
                    SendBuffPacket(BuffType<Buffs.Rapids>(), 240, healTarget, toWho, fromWho);
                    player.AddBuff(BuffType<Buffs.Rapids>(), 240);
                }

                PacketHandler.SendHealing(toWho, fromWho, healAmount, healTarget);
            }
        }

        /// <summary>
        /// Sends a Shield to another player through the server
        /// </summary>
        /// <param name="shieldAmount">Size of the shield</param>
        /// <param name="shieldTarget">ID of the player you are shielding</param>
        /// <param name="shieldType">The type of shield it will be (Basic, Physical, or Magic)</param>
        /// <param name="shieldDuration">How long the shield will last in frames (60 = 1)</param>
        /// <param name="toWho">Who this information is being sent to (-1 is all players on the server)</param>
        /// <param name="fromWho">Who is sending this information (player.whoAmI)</param>
        /// <param name="shieldColor">The color of the shield</param>
        internal void SendShieldPacket(int shieldAmount, int shieldTarget, ShieldType shieldType, int shieldDuration, int toWho, int fromWho, Color shieldColor)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (bloodPool)
                {
                    SendHealPacket((int)GetPassiveStat(new BloodPool(0)), shieldTarget, toWho, fromWho);
                    FindAndSetPassiveStat(new BloodPool(0), 0);
                }

                if (ardentsFrenzy)
                {
                    SendBuffPacket(BuffType<Frenzy>(), 240, shieldTarget, toWho, fromWho);
                    player.AddBuff(BuffType<Frenzy>(), 240);
                }

                if (rapids)
                {
                    SendBuffPacket(BuffType<Buffs.Rapids>(), 240, shieldTarget, toWho, fromWho);
                    player.AddBuff(BuffType<Buffs.Rapids>(), 240);
                }

                PacketHandler.SendShield(toWho, fromWho, shieldAmount, (int)shieldType, shieldDuration, shieldTarget, shieldColor);
            }
        }

        /// <summary>
        /// Sends mana to another player through the server
        /// </summary>
        /// <param name="manaAmount">Amount of mana</param>
        /// <param name="manaTarget">ID of the player you are giving mana</param>
        /// <param name="toWho">Who this information is being sent to (-1 is all players on the server)</param>
        /// <param name="fromWho">Who is sending this information (player.whoAmI)</param>
        internal void SendManaPacket(int manaAmount, int manaTarget, int toWho, int fromWho)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = mod.GetPacket();

                PacketHandler.SendMana(toWho, fromWho, manaAmount, manaTarget);
            }
        }

        /// <summary>
        /// Uses whats in lifeToHeal to heal the player
        /// </summary>
        public void HealLife()
        {
            if (spiritualRestur)
                lifeToHeal = (int)(lifeToHeal * 1.3);

            //if (player.HasBuff(BuffID.PotionSickness))
            //    lifeToHeal /= 2;

            int trueLifeHeal = lifeToHeal;

            if (GetRealHeathWithoutShield(true) - GetRealHeathWithoutShield(false) < trueLifeHeal)
            {
                trueLifeHeal = GetRealHeathWithoutShield(true) - GetRealHeathWithoutShield(false);
            }

            if (lifeToHeal > 0)
            {
                player.statLife += trueLifeHeal;
                player.HealEffect(lifeToHeal);
                lifeToHeal = 0;
            }
        }

        /// <summary>
        /// Activates the desired Summoner Spell
        /// </summary>
        /// <param name="num">1 = Left Slot | 2 = Right Slot</param>
        public void UseSummonerSpell(int num)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (num > 2)
                    num = 2;
                if (num < 1)
                    num = 1;
                sumSpells[num - 1].DoEffect(player, num);
            }
        }
        
        /// <summary>
        /// Reduces the cooldown of summoner spells by 1 tick if applicable
        /// </summary>
        public void SummonerCooldowns()
        {
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                for (int i = 0; i < sumSpells.Length; i++)
                {
                    if (sumCooldowns[i] > 0)
                        sumCooldowns[i]--;
                }
            }
        }

        /// <summary>
        /// Activates the desired ability if it exists
        /// </summary>
        /// <param name="abilityType"></param>
        [Obsolete]
        public void UseAbility(AbilityType abilityType)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (Abilities[(int)abilityType] != null)
                    Abilities[(int)abilityType].DoEffect(player, abilityType);
            }
        }

        /// <summary>
        /// Finds abilities and handles their cooldowns
        /// </summary>
        public void AbilityCooldownsAndStuff()
        {
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                bool[] Found = new bool[Abilities.Length];

                //AbilityItem heldItem = player.HeldItem.modItem as AbilityItem;
                Item currentItem = player.HeldItem;

                if (currentItem.type != ItemID.None)
                {
                    AbilityItemGLOBAL heldItem = currentItem.GetGlobalItem<AbilityItemGLOBAL>();
                    //heldItem.SetDefaults();
                    if (heldItem.IsAbilityItem)
                    {

                        for (int i = 0; i < Abilities.Length; i++)
                        {
                            if (heldItem.GetIfAbilityExists((AbilityType)i))
                            {
                                Abilities[i] = heldItem.GetAbility((AbilityType)i);
                                Found[i] = true;
                            }
                        }
                    }
                }

                foreach (var item in player.inventory)
                {
                    //AbilityItem curItem = item.modItem as AbilityItem;

                    if (item.type != ItemID.None)
                    {
                        AbilityItemGLOBAL curItem = item.GetGlobalItem<AbilityItemGLOBAL>();
                        //curItem.SetDefaults();
                        if (curItem.IsAbilityItem)
                        {

                            for (int i = 0; i < Abilities.Length; i++)
                            {
                                if (curItem.GetIfAbilityExists((AbilityType)i) && !Found[i])
                                {
                                    Abilities[i] = curItem.GetAbility((AbilityType)i);
                                    Found[i] = true;
                                }
                            }

                            if (Found.Where(x => x).Count() == Found.Length)
                                break;
                        }
                    }
                }

                for (int i = 0; i < Abilities.Length; i++)
                {
                    if (!Found[i])
                        Abilities[i] = null;
                }

                for (int i = 0; i < AbilityCooldowns.Length; i++)
                {
                    if (AbilityCooldowns[i] > 0)
                        AbilityCooldowns[i]--;
                }
            }
        }

        public override void AnglerQuestReward(float rareMultiplier, List<Item> rewardItems)
        {
            if (Main.rand.Next(3) == 0)
            {
                Item item = new Item();
                item.SetDefaults(ItemType<BrassBar>());
                item.stack = (int)(Main.rand.Next(6, 20));

                rewardItems.Add(item);
            }
            base.AnglerQuestReward(rareMultiplier, rewardItems);
        }

        public override void FrameEffects()
        {
            if ((darkinCostume || darkinCostumeForceVanity) && !darkinCostumeHideVanity)
            {
                player.legs = mod.GetEquipSlot("DarkinLegs", EquipType.Legs);
                player.body = mod.GetEquipSlot("DarkinBody", EquipType.Body);
                player.head = mod.GetEquipSlot("DarkinHead", EquipType.Head);
            }
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            DrawOnPlayer();

            //layers.Where(x => x.Name == "HeldItem").First().layer.Method.

            TerraLeague.ShieldEffect.visible = true;

            int itemIndex = layers.FindIndex(x => x.Name == "HeldItem");

            layers.Add(TerraLeague.ShieldEffect);

            if (trueInvis)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    layers[i].visible = false;
                }
            }

            AnimateSpellEffects();

            if (requiemChannel || finalsparkChannel || rightoftheArcaneChannel)
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 5;
            }

            if (player.armor.FirstOrDefault(x => x.type == ItemType<Items.Armor.HextechEvolutionMask>()) != null)
            {
                var hairLayer = layers.FirstOrDefault(x => x.Name == "Hair");
                var faceLayer = layers.FirstOrDefault(x => x.Name == "Face");
                if (hairLayer != null || faceLayer != null)
                {
                    layers.Insert(16, hairLayer);
                }
            }

            if (player.ownedProjectileCounts[ProjectileType<AtlasGauntlets_Right>()] > 0)
            {
                var armsLayer = layers.FirstOrDefault(x => x.Name == "Arms");
                if (armsLayer != null)
                {
                    armsLayer.visible = false;
                }
            }

            base.ModifyDrawLayers(layers);
        }

        /// <summary>
        /// Draw additional stuff on the player or world that is not appart of the player drawLayer list 
        /// </summary>
        public void DrawOnPlayer()
        {
            if (player.HasBuff(BuffType<Buffs.SpinningAxe>()) && !trueInvis)
            {
                Texture2D texture = mod.GetTexture("Projectiles/DarksteelThrowingAxe_SpinningAxe");
                Color color = Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16);
                float rotation = MathHelper.ToRadians(((float)Main.time % 15)*24) * player.direction;
                Main.spriteBatch.Draw
                (
                    texture,
                    new Vector2
                    (
                        player.position.X - Main.screenPosition.X + player.width * 0.5f,
                        player.position.Y - Main.screenPosition.Y + player.height * 0.5f
                    ),
                    new Rectangle(0, 0, texture.Width, texture.Height),
                    //Color.White,
                    color,
                    rotation,
                    texture.Size() * 0.5f,
                    1,
                    player.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0f
                );
            }

            
        }

        /// <summary>
        /// Ability Animation Handling
        /// </summary>
        public void AnimateSpellEffects()
        {
            if (abilityAnimation > 0 && player.itemAnimation <= 0)
            {
                if (abilityAnimationType == 1)
                {
                    if ((double)abilityAnimation < (double)abilityAnimationMax * 0.333)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 3;
                    }
                    else if ((double)abilityAnimation < (double)abilityAnimationMax * 0.666)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 2;
                    }
                    else
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height;
                    }
                }
                else if (abilityAnimationType == 2)
                {
                    if ((double)abilityAnimation > (double)abilityAnimationMax * 0.5)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 3;
                    }
                    else
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 2;
                    }
                }
                else if (abilityAnimationType == 3)
                {
                    if ((double)abilityAnimation > (double)abilityAnimationMax * 0.666)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 3;
                    }
                    else
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 3;
                    }
                }
                else if (abilityAnimationType == 4)
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 2;
                }
                else if (abilityAnimationType == 5)
                {
                    if (abilityItem.type == ItemID.Blowpipe || abilityItem.type == ItemID.Blowgun)
                    {
                        player.bodyFrame.Y = player.bodyFrame.Height * 2;
                    }
                    else
                    {
                        float num2 = abilityRotation * (float)player.direction;
                        player.bodyFrame.Y = player.bodyFrame.Height * 3;
                        
                        if ((double)num2 < -0.75)
                        {
                            player.bodyFrame.Y = player.bodyFrame.Height * 2;
                            if (player.gravDir == -1f)
                            {
                                player.bodyFrame.Y = player.bodyFrame.Height * 4;
                            }
                        }
                        if ((double)num2 > 0.6)
                        {
                            player.bodyFrame.Y = player.bodyFrame.Height * 4;
                            if (player.gravDir == -1f)
                            {
                                player.bodyFrame.Y = player.bodyFrame.Height * 2;
                            }
                        }
                    }
                }
                else if (abilityAnimationType == 6)
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 5;
                }
            }
        }

        /// <summary>
        /// Resets shield information for the beginning of a new frame
        /// </summary>
        private void ResetShieldStuff()
        {
            if (Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                player.statLife -= GetTotalShield();

                MagicShield = 0;
                PhysicalShield = 0;
                NormalShield = 0;
                

                for (int i = 0; i < Shields.Count; i++)
                {
                    Shield shield = Shields[i];

                    if (shield.AssociatedBuff != -1)
                    {
                        if (!player.HasBuff(shield.AssociatedBuff))
                        {
                            Shields.RemoveAt(i);
                        }
                    }
                    else
                    {
                        Shields[i] = new Shield(shield.ShieldAmount, shield.ShieldTimeLeft - 1, shield.ShieldColor, shield.ShieldType);
                    }
                }

                for (int i = 0; i < Shields.Count; i++)
                {
                    if (Shields[i].ShieldTimeLeft != 0)
                    {
                        if (Shields[i].ShieldType == ShieldType.Magic)
                            MagicShield += Shields[i].ShieldAmount;
                        else if (Shields[i].ShieldType == ShieldType.Physical)
                            PhysicalShield += Shields[i].ShieldAmount;
                        else
                            NormalShield += Shields[i].ShieldAmount;
                    }
                }

                Shields.RemoveAll(x => x.ShieldTimeLeft == 0);
                player.statLifeMax2 += GetTotalShield();
                player.statLife += GetTotalShield();
                PureHealthLastStep = GetRealHeathWithoutShield();
            }
            else
            {
                player.statLifeMax2 += GetTotalShield();
                PureHealthLastStep = GetRealHeathWithoutShield();
            }



            if (GetRealHeathWithoutShield() <= 0 && !player.dead && player.active && Main.LocalPlayer.whoAmI == player.whoAmI && GetTotalShield() > 0)
            {
                var ded = new PlayerDeathReason();
                ded.SourceCustomReason = "The shield around " + player.name + " couldn't save them";

                ClearShields();
                player.KillMe(ded, 0, 0);
                Kill(0, 0, false, ded);
            }

        }

        /// <summary>
        /// <para>Creates a shield on the player that is attached to a buff</para>
        /// If the buff is removed, so is the shield
        /// </summary>
        /// <param name="amount">Shield amount</param>
        /// <param name="buff">The buff ID the shield is attached to</param>
        /// <param name="color">The color of the shield</param>
        /// <param name="type">Shield type</param>
        public void AddShieldAttachedToBuff(int amount, int buff, Color color, ShieldType type)
        {
            int index = Shields.FindIndex(item => item.AssociatedBuff == buff);
            if (index >= 0)
            {
                if (buff == BuffType<Buffs.BloodShield>())
                {
                    int num = Shields[index].ShieldAmount + amount;

                    if (num > 200)
                        num = 200;

                    Shields[index] = new Shield(num, color, buff, type);
                }
                else if (buff == BuffType<PetricitePlating>())
                {
                    int num = Shields[index].ShieldAmount + amount;

                    if (num > 50)
                        num = 50;

                    Shields[index] = new Shield(num, color, buff, type);
                }
                else
                {
                    Shields[index] = new Shield(amount, color, buff, type);
                }
            }
            else
            {
                Shields.Add(new Shield(amount, color, buff, type));
            }
        }

        /// <summary>
        /// Creates a shield that is on a timer
        /// </summary>
        /// <param name="amount">Shield amount</param>
        /// <param name="duration">Duration of the shield in frames (60 = 1 second)</param>
        /// <param name="color">The color of the shield</param>
        /// <param name="type">Shield type</param>
        public void AddShield(int amount, int duration, Color color, ShieldType type)
        {
            Shields.Add(new Shield(amount, duration, color, type));
        }

        public void SetShieldColor()
        {
            if (player.whoAmI == Main.myPlayer)
            {
                Color oldCol = currentShieldColor;


                if (player.HasBuff(BuffType<DivineJudgementBuff>()))
                {
                    currentShieldColor = Color.Gold;
                }
                else if (GetTotalShield() > 0)
                {
                    currentShieldColor = Shields.Last().ShieldColor;
                }
                else if (veil)
                {
                    currentShieldColor = Color.Purple;
                }
                else
                {
                    currentShieldColor = new Color(255, 255, 255, 0);
                }

                if (Main.netMode == NetmodeID.MultiplayerClient && (oldCol.R != currentShieldColor.R || oldCol.G != currentShieldColor.G || oldCol.B != currentShieldColor.B || oldCol.A != currentShieldColor.A))
                {
                    PacketHandler.SendNewShield(-1, player.whoAmI, player.whoAmI, currentShieldColor);
                }
            }
        }

        /// <summary>
        /// Removes all shields from the player
        /// </summary>
        public void ClearShields()
        {
            player.statLifeMax2 -= MagicShield + PhysicalShield + NormalShield;

            Shields.RemoveAll(x => true);
            MagicShield = 0;
            PhysicalShield = 0;
            NormalShield = 0;
        }

        /// <summary>
        /// Sets the stats of the requested Passive
        /// </summary>
        /// <param name="SearchTarget">The Passive to search for</param>
        /// <param name="setTo">The number to set the stat to</param>
        /// <param name="secondaryPassive">Search the secondary slot of the LeagueItems</param>
        public void FindAndSetPassiveStat(Passive SearchTarget, float setTo)
        {
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                for (int i = 3; i < 9; i++)
                {
                    LeagueItem item = player.armor[i].modItem as LeagueItem;

                    if (item != null)
                    {
                        if (item.Passives != null)
                        {
                            for (int j = 0; j < item.Passives.Length; j++)
                            {
                                if (item.Passives[j].GetType() == SearchTarget.GetType() && item.Passives[j].currentlyActive)
                                {
                                    item.Passives[j].passiveStat = setTo;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the stats of the requested Active
        /// </summary>
        /// <param name="SearchTarget">The Active to search for</param>
        /// <param name="setTo">The number to set the stat to</param>
        public void FindAndSetActiveStat(Active SearchTarget, int setTo)
        {
            if (player.whoAmI == Main.LocalPlayer.whoAmI)
            {
                for (int i = 3; i < 9; i++)
                {
                    LeagueItem item = player.armor[i].modItem as LeagueItem;

                    if (item != null)
                    {
                        if (item.Active != null)
                        {
                            if (item.Active.GetType() == SearchTarget.GetType() && item.Active.currentlyActive)
                            {
                                item.Active.activeStat = setTo;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current stat of the requested Passive
        /// </summary>
        /// <param name="SearchTarget">The Passive to search fo</param>
        /// <param name="secondaryPassive">Search the secondary slot of the LeagueItems</param>
        /// <returns></returns>
        public float GetPassiveStat(Passive SearchTarget)
        {
            for (int i = 3; i < 9; i++)
            {
                LeagueItem item = player.armor[i].modItem as LeagueItem;

                if (item != null)
                {
                    if (item.Passives != null)
                    {
                        for (int j = 0; j < item.Passives.Length; j++)
                        {
                            if (item.Passives[j].GetType() == SearchTarget.GetType() && item.Passives[j].currentlyActive)
                            {
                                return item.Passives[j].passiveStat;
                            }
                        }
                    }
                }

            }
            return -1;
        }

        /// <summary>
        /// Alters the vanilla mana regen system to be a flat increase in current mana per second 1:1 with player.manaRegen * modPlayer.manaRegenModifer + additional modifiers
        /// </summary>
        public void LinearManaRegen()
        {
            if (TerraLeague.UseCustomManaRegen)
            {
                // Nebular Armor Thingys
                if (player.nebulaLevelMana > 0)
                    player.nebulaManaCounter = 0;


                if (player.manaRegenBuff)
                    manaRegen += 5;
                if (player.HasBuff(BuffID.StarInBottle))
                    player.manaRegenBonus += 25;

                player.manaRegenDelay = 90000;
                manaRegen += player.statManaMax2 / 75;
                //player.manaRegen = (int)(player.manaRegen * manaRegenModifer * (1 + (player.manaRegenBonus / 25.0)) * (1 + player.nebulaLevelMana));

                double trueModifier = manaRegenModifer + (player.manaRegenBonus / 50.0) + player.nebulaLevelMana;
                manaRegen = (int)(manaRegen * trueModifier);
                //if (player.manaRegen < 2)
                //    player.manaRegen = 2;

                // Out of combat regen
                if (CombatTimer >= 240)
                    manaRegen *= 2;

                if (manaRegenTimer == 60 && player.statMana < player.statManaMax2)
                    player.statMana += manaRegen;

                if (manaRegenTimer == 60)
                    manaRegenTimer = 0;
                else
                    manaRegenTimer++;
            }
        }

        /// <summary>
        /// Checks what Actives and Passives are currently useable because 2 of the same Active or Passive can't be active at once
        /// </summary>
        public void CheckActivesandPassivesAreActive()
        {
            List<string> names = new List<string>();
            //PassivesAreActive = new bool[12];
            //ActivesAreActive = new bool[6];

            for (int i = 0; i < 6; i++)
            {
                LeagueItem legItem = player.armor[i + 3].modItem as LeagueItem;

                if (legItem != null)
                {
                    if (legItem.Passives != null)
                    {
                        for (int j = 0; j < legItem.Passives.Length; j++)
                        {
                            bool contains = names.Contains(legItem.Passives[j].GetType().Name);
                            bool notUnique = legItem.Passives[j].deactivateIfNotUnique;
                            if (!contains || (contains && !notUnique))
                            {
                                names.Add(legItem.Passives[j].GetType().Name);
                                legItem.Passives[j].currentlyActive = true;
                            }
                            else
                            {
                                legItem.Passives[j].currentlyActive = false;
                            }
                        }
                    }
                    if (legItem.Active != null)
                    {
                        if (!names.Contains(legItem.Active.GetType().Name))
                        {
                            names.Add(legItem.Active.GetType().Name);
                            legItem.Active.currentlyActive = true;
                        }
                        else
                        {
                            legItem.Active.currentlyActive = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies the damage of frozen enemies
        /// </summary>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        public void ShatterEnemy(NPC target, ref int damage)
        {
            int lifeDam = target.lifeMax / 10;

            damage += lifeDam > 200 ? 200 : lifeDam;

            ShatterEffect(target);
            
            TerraLeague.RemoveBuffFromNPC(BuffType<Frozen>(), target.whoAmI);
            target.AddBuff(BuffType<FrozenCooldown>(), 300);

            target.netUpdate = true;
        }
        /// <summary>
        /// Does the partical effect for shatter
        /// </summary>
        /// <param name="target"></param>
        public void ShatterEffect(NPC target)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 27, -0.5f);
            for (int i = 0; i < 20; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(target.position, target.width, target.height, 80, 0, -2, 0, default(Color), 1.5f);
                dustIndex.velocity *= 2;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.LocalPlayer.whoAmI)
                PacketHandler.SendShatterEFX(-1, player.whoAmI, target.whoAmI);
        }

        public void CausticWoundsEffect(NPC target)
        {
            Main.PlaySound(new LegacySoundStyle(2, 122), target.Center);
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, 8, 8, 112, 0, 0, 0, new Color(59, 0, 255), 1f);
            }
        }

        public void SetTempUseItem(int itemToUse)
        {
            oldUsedInventorySlot = player.selectedItem;
            player.selectedItem = player.FindItem(itemToUse);

            if (oldUsedInventorySlot == 58 && player.selectedItem == -1)
                player.selectedItem = 58;
        }

        public void MeleeProjectileCooldown()
        {
            if (player.itemTime == 1 && meleeProjCooldown && player.whoAmI == Main.myPlayer)
            {
                Main.PlaySound(SoundID.MaxMana, -1, -1, 1, 1f, 0f);
                for (int num225 = 0; num225 < 5; num225++)
                {
                    int num226 = Dust.NewDust(player.position, player.width, player.height, 45, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                    Main.dust[num226].noLight = true;
                    Main.dust[num226].noGravity = true;
                    Dust obj2 = Main.dust[num226];
                    obj2.velocity *= 0.5f;
                }
                meleeProjCooldown = false;
            }
        }

        public void FlashOfBrillianceEffect(Player player, int Damage, NPC target)
        {
            if (flashofBrilliance && flashofBrillianceCooldown <= 0)
            {
                flashofBrillianceCooldown = 60;
                flashofBrilliance = false;

                List<int> magicItems = new List<int>() { ItemID.FlowerofFire, ItemID.FrostStaff, ItemID.ShadowFlameHexDoll, ItemID.StaffofEarth, ItemID.CrystalSerpent, ItemID.SpectreStaff, ItemID.WaterBolt};

                for (int i = 0; i < Main.rand.Next(1, 4); i++)
                {
                    Item chosenItem = new Item();
                    chosenItem.SetDefaults(magicItems[Main.rand.Next(magicItems.Count)]);
                    //int dam = chosenItem.damage / 3;
                    //if (dam < 30)
                    //    dam = 30;

                    int dam = 50;
                    float speed = Main.rand.NextFloat(chosenItem.shootSpeed * 0.9f, chosenItem.shootSpeed * 1.1f);
                    Vector2 velocity = TerraLeague.CalcVelocityToPoint(player.MountedCenter, target.Center, speed).RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));

                    Projectile proj = Projectile.NewProjectileDirect(player.MountedCenter, velocity, chosenItem.shoot, dam, chosenItem.knockBack, player.whoAmI);
                    proj.GetGlobalProjectile<PROJECTILEGLOBAL>().noOnHitEffects = true;
                    Main.PlaySound(chosenItem.UseSound, player.MountedCenter);
                }
            }
        }

        public int ScaleValueWithHealPower(float value, bool useLastFrameValue = false)
        {
            if (useLastFrameValue)
                return (int)Math.Round(value * healPowerLastStep, 0);
            else
                return (int)Math.Round(value * healPower, 0);
        }
    }
}

public struct Shield
{
    public int ShieldAmount;
    public int ShieldTimeLeft;
    public Color ShieldColor;
    public int AssociatedBuff;
    public ShieldType ShieldType;

    public Shield(int shieldAmount, int shieldTimeLeft, Color color, ShieldType type)
    {
        ShieldAmount = shieldAmount;
        ShieldTimeLeft = shieldTimeLeft;
        ShieldColor = color;
        AssociatedBuff = -1;
        ShieldType = type;
    }

    public Shield(int shieldAmount, Color color, int AttachedBuffType, ShieldType type)
    {
        ShieldAmount = shieldAmount;
        ShieldColor = color;
        AssociatedBuff = AttachedBuffType;
        ShieldTimeLeft = -1;
        ShieldType = type;
    }

    public Shield(int shieldAmount, Color color, int AttachedBuffType, ShieldType type, int shieldTimeLeft)
    {
        ShieldAmount = shieldAmount;
        ShieldColor = color;
        AssociatedBuff = AttachedBuffType;
        ShieldTimeLeft = shieldTimeLeft;
        ShieldType = type;
    }
}

public enum AbilityType : int
{
    Q,
    W,
    E,
    R
}

public enum DamageType
{
    MEL,
    RNG,
    MAG,
    SUM,
    NONE
}

public enum ShieldType
{
    Basic,
    Magic,
    Physical
}
