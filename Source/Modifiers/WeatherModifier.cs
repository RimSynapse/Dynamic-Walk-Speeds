using RimWorld;
using Verse;

namespace DynamicWalkSpeeds.Modifiers
{
    public static class WeatherModifier
    {
        public static float GetWeatherMultiplier(Map map, DynamicWalkSpeedsSettings settings)
        {
            if (map == null || !settings.enableWeatherModifiers)
                return 1.0f;

            WeatherDef curWeather = map.weatherManager?.curWeather;
            if (curWeather == null)
                return 1.0f;

            if (settings.weatherMultipliers.TryGetValue(curWeather.defName, out float mult))
            {
                return mult;
            }

            return GetDefaultWeatherMultiplier(curWeather);
        }

        public static float GetDefaultWeatherMultiplier(WeatherDef weather)
        {
            if (weather == null) return 1.0f;
            
            float penalty = 0f;
            if (weather.rainRate > 0) penalty += weather.rainRate * 0.10f;
            if (weather.snowRate > 0) penalty += weather.snowRate * 0.15f;
            
            return UnityEngine.Mathf.Clamp(1.0f - penalty, 0.5f, 1.5f);
        }
    }
}
