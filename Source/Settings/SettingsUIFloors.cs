using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace DynamicWalkSpeeds.Settings
{
    public static class SettingsUIFloors
    {
        private static Vector2 scrollPosition = Vector2.zero;

        public static void DrawFloorsTab(Listing_Standard listing, DynamicWalkSpeedsSettings settings, Rect inRect)
        {
            listing.CheckboxLabeled("Enable Floor Speed Modifiers", ref settings.enableFloorModifiers);
            if (!settings.enableFloorModifiers) return;

            listing.CheckboxLabeled("Link all floor types to Master Floor Scale", ref settings.linkFloors);
            settings.masterFloorScale = listing.SliderLabeled($"Master Floor Scale ({settings.masterFloorScale:F2}x)", settings.masterFloorScale, 0.50f, 2.00f);

            listing.Gap(10f);
            listing.Label("Individual Floor Multipliers:");

            List<TerrainDef> allTerrains = DefDatabase<TerrainDef>.AllDefsListForReading;
            if (allTerrains == null || allTerrains.Count == 0) return;

            Rect outRect = listing.GetRect(220f);
            Rect viewRect = new Rect(0f, 0f, outRect.width - 18f, allTerrains.Count * 32f);

            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            Listing_Standard scrollListing = new Listing_Standard();
            scrollListing.Begin(viewRect);

            for (int i = 0; i < allTerrains.Count; i++)
            {
                TerrainDef t = allTerrains[i];
                if (!settings.floorMultipliers.TryGetValue(t.defName, out float curVal))
                {
                    curVal = Modifiers.FloorModifier.GetDefaultTerrainMultiplier(t);
                    settings.floorMultipliers[t.defName] = curVal;
                }

                float newVal = scrollListing.SliderLabeled($"{t.LabelCap} ({curVal:F2}x)", curVal, 0.50f, 2.00f);
                settings.floorMultipliers[t.defName] = newVal;
            }

            scrollListing.End();
            Widgets.EndScrollView();
        }
    }
}
