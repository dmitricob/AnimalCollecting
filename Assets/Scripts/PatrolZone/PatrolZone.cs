using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PatrolZone
{
    public class PatrolZone : MonoBehaviour
    {
        [SerializeField] internal List<Rect> _patrolZones;

        public Vector2 GetRandomPoint()
        {
            var rectId = Random.Range(0, _patrolZones.Count);
            var currentRect = _patrolZones[rectId];
            return new Vector2(Random.Range(currentRect.xMin, currentRect.xMax),Random.Range(currentRect.yMin, currentRect.yMax));
        }

        public bool IsInZone(Vector2 position) => _patrolZones.Any(rect => rect.Contains(position));
        
        public void OnDrawGizmosSelected()
        {
            var color = Color.cyan;
            color.a = 0.7f;
            Gizmos.color = color;
            foreach (var zone in _patrolZones)
            {
                Gizmos.DrawCube(zone.center, zone.size);
            }
        }
    }
}