using System.Collections.Generic;
using RimWorld;
using Verse;

namespace DynamicWalkSpeeds.Modifiers
{
    public static class TerritoryModifier
    {
        public static float GetTerritoryMultiplier(Pawn pawn, DynamicWalkSpeedsSettings settings)
        {
            if (pawn == null || pawn.Map == null || !settings.enableTerritoryModifiers)
                return 1.0f;

            Map map = pawn.Map;
            bool isHostileTile = IsHostileMapTile(map);
            bool hasHostiles = HasActiveHostilePawns(map, pawn);

            bool isHostileTerritory;
            if (settings.linkTerritoryTriggers)
            {
                isHostileTerritory = isHostileTile || hasHostiles;
            }
            else
            {
                isHostileTerritory = (settings.hostileMapTileTrigger && isHostileTile) ||
                                     (settings.activeHostilePawnsTrigger && hasHostiles);
            }

            return isHostileTerritory ? settings.hostileTerritoryMultiplier : 1.0f;
        }

        private static bool IsHostileMapTile(Map map)
        {
            Faction mapFaction = map.ParentFaction;
            return mapFaction != null && mapFaction != Faction.OfPlayer && mapFaction.HostileTo(Faction.OfPlayer);
        }

        private static bool HasActiveHostilePawns(Map map, Pawn pawn)
        {
            IReadOnlyList<Pawn> allPawns = map.mapPawns?.AllPawnsSpawned;
            if (allPawns == null) return false;

            for (int i = 0; i < allPawns.Count; i++)
            {
                Pawn p = allPawns[i];
                if (p != null && !p.Dead && !p.Downed && p.HostileTo(pawn))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
