using HarmonyLib;
using Verse;
using Verse.AI;
using DynamicWalkSpeeds.Modifiers;

namespace DynamicWalkSpeeds.Patches
{
    [HarmonyPatch(typeof(Pawn_PathFollower), "CostToMoveIntoCell", new System.Type[] { typeof(Pawn), typeof(IntVec3) })]
    public static class PathFollowerPatch
    {
        public static void Postfix(Pawn pawn, IntVec3 c, ref float __result)
        {
            if (pawn == null || pawn.Map == null || __result <= 0f)
                return;

            DynamicWalkSpeedsSettings settings = DynamicWalkSpeedsMod.settings;
            if (settings == null)
                return;

            float weatherMult = WeatherModifier.GetWeatherMultiplier(pawn.Map, settings);
            float floorMult = FloorModifier.GetFloorMultiplier(pawn.Map, c, settings);
            float surfaceMult = SurfaceModifier.GetSurfaceMultiplier(pawn.Map, c, settings);
            float territoryMult = TerritoryModifier.GetTerritoryMultiplier(pawn, settings);

            float totalSpeedMultiplier = weatherMult * floorMult * surfaceMult * territoryMult;
            if (totalSpeedMultiplier <= 0.01f)
                totalSpeedMultiplier = 0.01f;

            __result /= totalSpeedMultiplier;
        }
    }
}
