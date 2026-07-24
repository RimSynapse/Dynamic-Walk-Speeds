using Verse;

namespace DynamicWalkSpeeds.Modifiers
{
    public static class FloorModifier
    {
        public static float GetFloorMultiplier(Map map, IntVec3 cell, DynamicWalkSpeedsSettings settings)
        {
            if (map == null || !settings.enableFloorModifiers || !cell.InBounds(map))
                return 1.0f;

            TerrainDef terrain = cell.GetTerrain(map);
            if (terrain == null)
                return 1.0f;

            if (settings.floorMultipliers.TryGetValue(terrain.defName, out float mult))
            {
                return settings.linkFloors ? mult * settings.masterFloorScale : mult;
            }

            float defaultMult = GetDefaultTerrainMultiplier(terrain);
            return settings.linkFloors ? defaultMult * settings.masterFloorScale : defaultMult;
        }

        public static float GetDefaultTerrainMultiplier(TerrainDef terrain)
        {
            if (terrain == null) return 1.0f;

            bool isManufactured = terrain.generated || 
                                  terrain.designationCategory != null || 
                                  (terrain.researchPrerequisites != null && terrain.researchPrerequisites.Count > 0) ||
                                  (terrain.defName != null && (terrain.defName.Contains("Tile") || terrain.defName.Contains("Concrete") || terrain.defName.Contains("Carpet") || terrain.defName.Contains("Smooth")));

            return isManufactured ? 1.15f : 1.0f;
        }
    }
}
