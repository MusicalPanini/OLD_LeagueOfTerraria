using Terraria.ModLoader;

namespace SburbTest
{
	public class SburbTest : Mod
	{
        public static SburbTest instance;

		public SburbTest()
		{
            instance = this;
        }

        public override void Unload()
        {
            instance = null;
        }
    }
}