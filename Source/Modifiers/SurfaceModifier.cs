using System.Collections.Generic;
using RimWorld;
using Verse;

namespace DynamicWalkSpeeds.Modifiers
{
    public static class SurfaceModifier
    {
        public static float GetSurfaceMultiplier(Map map, IntVec3 cell, DynamicWalkSpeedsSettings settings)
        {
            if (map == null || !settings.enableSurfacePenalties || !cell.InBounds(map))
                return 1.0f;

            float penalty = 0f;

            if (settings.snowPenaltyScale > 0)
            {
                float snowDepth = map.snowGrid?.GetDepth(cell) ?? 0f;
                penalty += snowDepth * 0.20f * settings.snowPenaltyScale;
            }

            if (settings.filthPenaltyScale > 0)
            {
                List<Thing> thingList = cell.GetThingList(map);
                if (thingList != null)
                {
                    for (int i = 0; i < thingList.Count; i++)
                    {
                        if (thingList[i] is Filth)
                        {
                            penalty += 0.05f * settings.filthPenaltyScale;
                        }
                    }
                }
            }

            return UnityEngine.Mathf.Clamp(1.0f - penalty, 0.3f, 1.0f);
        }
    }
}
