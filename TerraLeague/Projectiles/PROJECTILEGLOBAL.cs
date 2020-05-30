using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace TerraLeague.Projectiles
{
    public class PROJECTILEGLOBAL : GlobalProjectile
    {
        internal ProjectilePacketHandler PacketHandler = new ProjectilePacketHandler(3);
        public bool summonAbility = false;
        public bool abilitySpell = false;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            PLAYERGLOBAL modPlayer = Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.pirateSet  && !target.immortal && !target.SpawnedFromStatue && target.lifeMax > 5 && projectile.ranged)
            {
                if(Main.rand.Next(0, 5) == 0)
                    Item.NewItem(target.getRect(), ItemID.CopperCoin);
                if (Main.rand.Next(0, 50) == 0)
                    Item.NewItem(target.getRect(), ItemID.SilverCoin);
                if (Main.rand.Next(0, 500) == 0)
                    Item.NewItem(target.getRect(), ItemID.GoldCoin);
                if (Main.rand.Next(0, 500000) == 0)
                    Item.NewItem(target.getRect(), ItemID.PlatinumCoin);
            }

            base.OnHitNPC(projectile, target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (abilitySpell)
            {
                crit = (Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().arcanePrecision && Main.rand.Next(0, 100) < ArcanePrecision.critChance);
            }
            else
            {
                base.ModifyHitNPC(projectile, target, ref damage, ref knockback, ref crit, ref hitDirection);
            }
        }

        public bool DoNotCountRangedDamage(Projectile proj)
        {
            if (proj.Name == "Crystal Shard"
                || proj.Name == "Hallow Star"
                )
                return true;
            else
                return false;
        }

        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            PLAYERGLOBAL modPlayer = target.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.projectileDodge)
                return false;
            else
                return base.CanHitPlayer(projectile, target);
        }
    }
}
