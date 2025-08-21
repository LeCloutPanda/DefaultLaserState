using BepInEx;
using BepInEx.Configuration;
using BepInEx.NET.Common;
using BepInExResoniteShim;
using FrooxEngine;
using HarmonyLib;

namespace DefaultLaserState;

[ResonitePlugin("dev.lecloutpanda.defaultlaserstate", "Default Laser State", "1.0.0", "LeCloutPanda", "https://github.com/LeCloutPanda/DefaultLaserState")]
public class Patch : BasePlugin 
{
    private static ConfigEntry<bool> LASERSTATE;

    public override void Load()
    {
        LASERSTATE = Config.Bind("General", "Default Laser State", true, "Enable/disable the laser on spawn");
    
        HarmonyInstance.PatchAll();
    }

	[HarmonyPatch(typeof(InteractionHandler), "OnAwake")]
	private class CommonToolPatch
	{
		[HarmonyPostfix]
		private static void Postfix(InteractionHandler __instance, Sync<bool> ____laserEnabled)
		{
			InteractionHandler __instance2 = __instance;
			Sync<bool> ____laserEnabled2 = ____laserEnabled;
			__instance2.RunInUpdates(3, delegate
			{
				if (__instance2.Slot.ActiveUser == __instance2.LocalUser)
				{
					____laserEnabled2.Value = LASERSTATE.Value;
				}
			});
		}
	}
}