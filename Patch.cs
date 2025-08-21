using BepInEx;
using BepInEx.Configuration;
using BepInEx.NET.Common;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;
using Renderite.Shared;

namespace DefaultLaserState;

public abstract class BaseResonitePlugin : BasePlugin
{
    public abstract string Author { get; }
    public abstract string Link { get; }
}

[BepInPlugin(GUID, Name, Version)]
public class Patch : BaseResonitePlugin
{
    public const string GUID = "dev.lecloutpanda.defaultlaserstate";
    public const string Name = "Default Laser State";
    public const string Version = "1.0.0";
    public override string Author => "LeCloutPanda";
    public override string Link => "https://github.com/LeCloutPanda/DefaultLaserState";

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