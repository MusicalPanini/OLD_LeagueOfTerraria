using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class BootsOfMobility : LeagueBoot
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boots of Mobility");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.accessory = true;
            base.SetDefaults();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
        }

        public override void Tier1Update(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().CombatTimer >= 240)
            {
                player.moveSpeed += 0.1f;
                player.runAcceleration += 0.01f;

                Dust dust = Dust.NewDustDirect(new Microsoft.Xna.Framework.Vector2(player.position.X, player.position.Y + 30), player.width, player.height - 30, DustID.Smoke, 0, 0, 0, new Microsoft.Xna.Framework.Color(223, 135, 29));
                dust.noGravity = true;
                dust.velocity *= 0;
            }
            base.Tier1Update(player);
        }

        public override void Tier2Update(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().CombatTimer >= 240)
            {
                player.moveSpeed += 0.2f;
                player.runAcceleration += 0.02f;

                Dust dust = Dust.NewDustDirect(new Microsoft.Xna.Framework.Vector2(player.position.X, player.position.Y + 30), player.width, player.height - 30, DustID.Smoke, 0, 0, 0, new Microsoft.Xna.Framework.Color(223, 135, 29));
                dust.noGravity = true;
                dust.velocity *= 0;
            }
            base.Tier1Update(player);
        }

        public override void Tier3Update(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().CombatTimer >= 240)
            {
                player.moveSpeed += 0.4f;
                player.runAcceleration += 0.04f;

                Dust dust = Dust.NewDustDirect(new Microsoft.Xna.Framework.Vector2(player.position.X, player.position.Y + 30), player.width, player.height - 30, DustID.Smoke, 0, 0, 0, new Microsoft.Xna.Framework.Color(223, 135, 29));
                dust.noGravity = true;
                dust.velocity *= 0;
            }
            base.Tier1Update(player);
        }

        public override void Tier4Update(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().CombatTimer >= 240)
            {
                player.moveSpeed += 0.8f;
                player.runAcceleration += 0.08f;

                Dust dust = Dust.NewDustDirect(new Microsoft.Xna.Framework.Vector2(player.position.X, player.position.Y + 30), player.width, player.height - 30, DustID.Smoke, 0, 0, 0, new Microsoft.Xna.Framework.Color(223, 135, 29));
                dust.noGravity = true;
                dust.velocity *= 0;
            }
            base.Tier1Update(player);
        }

        public override void Tier5Update(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().CombatTimer >= 240)
            {
                player.moveSpeed += 0.1f;
                player.runAcceleration += 0.1f;

                Dust dust = Dust.NewDustDirect(new Microsoft.Xna.Framework.Vector2(player.position.X, player.position.Y + 30), player.width, player.height - 30, DustID.Smoke, 0, 0, 0, new Microsoft.Xna.Framework.Color(223, 135, 29));
                dust.noGravity = true;
                dust.velocity *= 0;
            }
            base.Tier1Update(player);
        }

        public override string Tier1Tip()
        {
            return "While out of combat, gain an unremarkable amount of mobility";
        }

        public override string Tier2Tip()
        {
            return "While out of combat, gain a slight amount of mobility";
        }

        public override string Tier3Tip()
        {
            return "While out of combat, gain a good amount of mobility";
        }

        public override string Tier4Tip()
        {
            return "While out of combat, gain a great amount of mobility";
        }

        public override string Tier5Tip()
        {
            return "While out of combat, gain an insane amount of mobility";
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BootsOfSpeed>());
            recipe.AddIngredient(ItemID.Cloud, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
