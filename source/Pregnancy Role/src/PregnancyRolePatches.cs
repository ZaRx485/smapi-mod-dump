using Harmony;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Events;
using System;
using System.Collections.Generic;

namespace PregnancyRole
{
	internal class PregnancyRolePatches
	{
		protected static IModHelper Helper => ModEntry.Instance.Helper;
		protected static IMonitor Monitor => ModEntry.Instance.Monitor;
		protected static HarmonyInstance Harmony => ModEntry.Instance.harmony;

		private static Dictionary<Farmer, bool> GenderOverrides =
			new Dictionary<Farmer, bool> ();

		public static void Apply ()
		{
			Harmony.Patch (
				original: AccessTools.Method (typeof (FarmerTeam),
					"handleIncomingProposal"),
				prefix: new HarmonyMethod (typeof (PregnancyRolePatches),
					nameof (PregnancyRolePatches.Prefix)),
				postfix: new HarmonyMethod (typeof (PregnancyRolePatches),
					nameof (PregnancyRolePatches.Postfix))
			);

			Harmony.Patch (
				original: AccessTools.Method (typeof (FarmerTeam),
					"genderedKey"),
				prefix: new HarmonyMethod (typeof (PregnancyRolePatches),
					nameof (PregnancyRolePatches.genderedKey_Prefix))
			);

			Harmony.Patch (
				original: AccessTools.Method (typeof (PlayerCoupleBirthingEvent),
					nameof (PlayerCoupleBirthingEvent.setUp)),
				prefix: new HarmonyMethod (typeof (PregnancyRolePatches),
					nameof (PregnancyRolePatches.Prefix)),
				postfix: new HarmonyMethod (typeof (PregnancyRolePatches),
					nameof (PregnancyRolePatches.Postfix))
			);

			Harmony.Patch (
				original: AccessTools.Method (typeof (QuestionEvent),
					nameof (QuestionEvent.setUp)),
				prefix: new HarmonyMethod (typeof (PregnancyRolePatches),
					nameof (PregnancyRolePatches.Prefix)),
				postfix: new HarmonyMethod (typeof (PregnancyRolePatches),
					nameof (PregnancyRolePatches.Postfix))
			);
		}

		public static bool Prefix ()
		{
			try
			{
				long? spouseID = Game1.player.team.GetSpouse
					(Game1.player.UniqueMultiplayerID);
				if (spouseID.HasValue)
				{
					Game1.otherFarmers.TryGetValue (spouseID.Value,
						out Farmer spouse);
					if (spouse != null)
						OverrideFarmers (Game1.player, spouse);
				}
			}
			catch (Exception e)
			{
				Monitor.Log ($"Failed in {nameof (Prefix)}:\n{e}",
					LogLevel.Error);
			}
			return true;
		}

		public static void Postfix ()
		{
			try
			{
				ClearOverrides ();
			}
			catch (Exception e)
			{
				Monitor.Log ($"Failed in {nameof (Postfix)}:\n{e}",
					LogLevel.Error);
			}
		}

		public static bool genderedKey_Prefix (ref string __result,
			string baseKey, Farmer farmer)
		{
			try
			{
				if (!GenderOverrides.TryGetValue (farmer, out bool isMale))
					isMale = farmer.IsMale;
				__result = baseKey + (isMale ? "_Male" : "_Female");
				return false;
			}
			catch (Exception e)
			{
				Monitor.Log ($"Failed in {nameof (genderedKey_Prefix)}:\n{e}",
					LogLevel.Error);
				return true;
			}
		}

		protected static void OverrideFarmers (Farmer farmer1, Farmer farmer2)
		{
			if (GenderOverrides.ContainsKey (farmer1) ||
					GenderOverrides.ContainsKey (farmer2))
				return;

			Role role1 = Model.GetPregnancyRole (farmer1);
			Role role2 = Model.GetPregnancyRole (farmer2);

			if (role1 == Role.Adopt || role2 == Role.Adopt)
			{
				OverrideFarmer (farmer1, true);
				OverrideFarmer (farmer2, true);
			}
			else
			{
				OverrideFarmer (farmer1, role1 == Role.Make);
				OverrideFarmer (farmer2, role2 == Role.Make);
			}
		}

		private static void OverrideFarmer (Farmer farmer, bool isMale)
		{
			GenderOverrides[farmer] = farmer.IsMale;
			farmer.IsMale = isMale;
		}

		protected static void ClearOverrides ()
		{
			foreach (Farmer farmer in GenderOverrides.Keys)
				farmer.IsMale = GenderOverrides[farmer];
			GenderOverrides.Clear ();
		}
	}
}
