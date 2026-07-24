using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace DynamicWalkSpeeds.Settings
{
    public static class SettingsUIWeather
    {
        private static Vector2 scrollPosition = Vector2.zero;

        public static void DrawWeatherTab(Listing_Standard listing, DynamicWalkSpeedsSettings settings, Rect inRect)
        {
            listing.CheckboxLabeled("Enable Weather Modifiers", ref settings.enableWeatherModifiers, "Applies speed adjustments based on active weather.");
            if (!settings.enableWeatherModifiers) return;

            listing.Gap(10f);
            listing.Label("Weather Speed Multipliers (0.50x to 1.50x):");

            List<WeatherDef> allWeathers = DefDatabase<WeatherDef>.AllDefsListForReading;
            if (allWeathers == null || allWeathers.Count == 0) return;

            Rect outRect = listing.GetRect(250f);
            Rect viewRect = new Rect(0f, 0f, outRect.width - 18f, allWeathers.Count * 32f);

            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            Listing_Standard scrollListing = new Listing_Standard();
            scrollListing.Begin(viewRect);

            for (int i = 0; i < allWeathers.Count; i++)
            {
                WeatherDef w = allWeathers[i];
                if (!settings.weatherMultipliers.TryGetValue(w.defName, out float curVal))
                {
                    curVal = Modifiers.WeatherModifier.GetDefaultWeatherMultiplier(w);
                    settings.weatherMultipliers[w.defName] = curVal;
                }

                float newVal = scrollListing.SliderLabeled($"{w.LabelCap} ({curVal:F2}x)", curVal, 0.50f, 1.50f);
                settings.weatherMultipliers[w.defName] = newVal;
            }

            scrollListing.End();
            Widgets.EndScrollView();
        }
    }
}
