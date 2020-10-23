using Microsoft.Xna.Framework;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CompleteItems;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Energized : Passive
    {
        int baseDamage = 35;
        int rangedScaling;
        bool superCharge = false;

        public Energized(int BaseDamage, int RangedScaling, bool SuperCharge = false)
        {
            baseDamage = BaseDamage;
            rangedScaling = RangedScaling;
            superCharge = SuperCharge;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            string charge = superCharge ? "\nRanged attacks and melee swings will generate charge and moving will generate even more charge" : "\nMoving, ranged attacks and melee swings will generate charge";
            return TooltipName("ENERGIZED") + TerraLeague.CreateColorString(PassiveSecondaryColor, "At max charge your next melee or ranged attack will deal ") + baseDamage + " + " + TerraLeague.CreateScalingTooltip(DamageType.RNG, modPlayer.RNG, rangedScaling) + TerraLeague.CreateColorString(PassiveSecondaryColor, " damage" + charge);
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            //int num;


            //num = TerraLeague.FindAccessorySlotOnPlayer(player, GetModItem(ItemType<StaticShiv>()));
            //if (num != -1)
            //    if (passiveStat >= 100)
            //        modPlayer.EnergizedDischarge = true;

            //num = TerraLeague.FindAccessorySlotOnPlayer(player, GetModItem(ItemType<RapidFire>()));
            //if (num != -1)
            //    if (passiveStat >= 100)
            //        modPlayer.EnergizedDetonate = true;

            //num = TerraLeague.FindAccessorySlotOnPlayer(player, GetModItem(ItemType<Stormrazer>()));
            //if (num != -1)
            //    if (passiveStat >= 100)
            //        modPlayer.EnergizedStorm = true;

            //num = TerraLeague.FindAccessorySlotOnPlayer(player, GetModItem(ItemType<KircheisShard>()));
            //if (num != -1)
            //    if (passiveStat >= 100)
            //        modPlayer.EnergizedShard = true;

            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            float stat;
            float chargeRate = superCharge ? 0.1f : 0.05f;
            if (player.velocity.X < 0)
                stat = -player.velocity.X * chargeRate;
            else
                stat = player.velocity.X * chargeRate;

            AddStat(player, 100, stat);

            if (passiveStat >= 100)
            {
                if (Main.rand.Next(0, 6) == 0)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 261, 0, 0, 0, new Color(255, 255, 0, 150));
                    dust.noGravity = true;
                }
                player.AddBuff(BuffType<Buffs.EnergizedStrike>(), 1);
            }

            base.PostPlayerUpdate(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            AddStat(player, 100, 2);

            if (modPlayer.energized)
            {
                int bonusDamage = baseDamage + (int)(modPlayer.RNG * rangedScaling / 100d);

                int projID = -1;

                if (modPlayer.EnergizedDischarge)
                {
                    projID = ProjectileType<Item_EnergizedBolt>();
                }
                else if (modPlayer.EnergizedStorm)
                {
                    damage += bonusDamage;
                    Efx(player, target);
                    SendEfx(player, target, modItem);
                    target.AddBuff(BuffType<Buffs.Slowed>(), 180);
                }

                if (projID != -1)
                {
                    if (projID == ProjectileType<Item_EnergizedBolt>())
                        Projectile.NewProjectile(target.Center, Vector2.Zero, projID, bonusDamage, 0, player.whoAmI, -1, modPlayer.EnergizedStorm ? 1 : 0);
                    else if (projID == ProjectileType<Item_RapidfireExplosion>())
                        Projectile.NewProjectile(target.Center, Vector2.Zero, projID, bonusDamage, 0, player.whoAmI, modPlayer.EnergizedStorm ? 1 : 0);
                }
                //else if (modPlayer.EnergizedStorm)
                //{
                //    damage += bonusDamage;

                //    Efx(player, target);
                //    if (Main.netMode == NetmodeID.MultiplayerClient)
                //        PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, FindIfPassiveIsSecondary(modItem));
                //}

                modPlayer.energized = false;
                passiveStat = 0;
            }

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (proj.ranged)
            {
                AddStat(player, 100, 2);
            }

            if (modPlayer.energized && (proj.melee || proj.ranged))
            {
                int bonusDamage = baseDamage + (int)(modPlayer.RNG * rangedScaling / 100d);

                int projID = -1;

                if (modPlayer.EnergizedDischarge)
                {
                    projID = ProjectileType<Item_EnergizedBolt>();
                }
                else if (modPlayer.EnergizedDetonate)
                {
                    projID = ProjectileType<Item_RapidfireExplosion>();
                }
                else if (modPlayer.EnergizedStorm)
                {
                    damage += bonusDamage;
                    Efx(player, target);
                    SendEfx(player, target, modItem);
                    target.AddBuff(BuffType<Buffs.Slowed>(), 180);
                }

                if (projID != -1)
                {
                    if (projID == ProjectileType<Item_EnergizedBolt>())
                        Projectile.NewProjectile(target.Center, Vector2.Zero, projID, bonusDamage, 0, player.whoAmI, -1, modPlayer.EnergizedStorm ? 1 : 0);
                    else if (projID == ProjectileType<Item_RapidfireExplosion>())
                        Projectile.NewProjectile(target.Center, Vector2.Zero, projID, bonusDamage, 0, player.whoAmI, modPlayer.EnergizedStorm ? 1 : 0);
                }
                //else if (modPlayer.EnergizedStorm)
                //{
                //    Efx(player, target);
                //    if (Main.netMode == NetmodeID.MultiplayerClient)
                //        PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, FindIfPassiveIsSecondary(modItem));
                //    //damage += bonusDamage;
                //}

                passiveStat = 0;
            }

            // Energized Stuff
            if (proj.type == ProjectileType<Item_EnergizedBolt>())
            {
                int bonusDamage = baseDamage + (int)(modPlayer.RNG * rangedScaling / 100d);

                if (modPlayer.EnergizedDetonate)
                {
                    Projectile.NewProjectile(target.Center, Vector2.Zero, ProjectileType<Item_RapidfireExplosion>(), bonusDamage, 0, Main.player[proj.owner].whoAmI, modPlayer.EnergizedStorm ? 0 : 1);
                }
                if (modPlayer.EnergizedStorm)
                {
                    Efx(player, target);
                    SendEfx(player, target, modItem);
                    target.AddBuff(BuffType<Buffs.Slowed>(), 180);
                }
            }
            if (proj.type == ProjectileType<Item_RapidfireExplosion>())
            {
                if (modPlayer.EnergizedStorm)
                {
                    Efx(player, target);
                    SendEfx(player, target, modItem);
                    target.AddBuff(BuffType<Buffs.Slowed>(), 180);
                }
            }

            modPlayer.energized = false;
            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }

        override public void Efx(Player player, NPC HitNPC)
        {
            Main.PlaySound(new LegacySoundStyle(3, 53), HitNPC.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(HitNPC.position, HitNPC.width, HitNPC.height, 261, 0, 0, 0, new Color(255, 255, 0, 150), 1.5f);
                dust.velocity *= 3;
                dust.noGravity = true;
            }
        }
    }
}
