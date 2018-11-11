using Terraria;
using Terraria.ModLoader;

namespace PressF11
{
	class PressF11 : Mod
	{
        public static ModHotKey SwitchSlots;
        public static ModHotKey SwapVertical;
        public static ModHotKey SwapHorizontal;
        public static ModHotKey LootAll;
        public static ModHotKey CraftRandom;

		public PressF11()
		{
		}

        public override void Load()
        {
            SwitchSlots = RegisterHotKey("Switch Slots", "Y");
            SwapVertical = RegisterHotKey("Swap Vertical", "U");
            SwapHorizontal = RegisterHotKey("Swap Horizontal", "I");
            LootAll = RegisterHotKey("Loot All", "L");
            CraftRandom = RegisterHotKey("Craft Random Item(s)", "K");
        }

        public override void Unload()
        {
            SwitchSlots = null;
            SwapVertical = null;
            SwapHorizontal = null;
            LootAll = null;
            CraftRandom = null;
        }

    }
}
