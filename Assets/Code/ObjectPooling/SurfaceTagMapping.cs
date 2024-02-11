using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SurfaceTagMapping", menuName = "Mapping/SurfaceToTag")]
public class SurfaceTagMapping : ScriptableObject
{
    [System.Serializable]
    public class SurfaceTagPair
    {
        public SurfaceType surfaceType;
        public string tag;
    }

    public List<SurfaceTagPair> mappings;

    // Utility method to get tag by SurfaceType
    public string GetTagForSurfaceType(SurfaceType surfaceType)
    {
        foreach (var mapping in mappings)
        {
            if (mapping.surfaceType == surfaceType)
            {
                return mapping.tag;
            }
        }
        return null; // Return null or a default tag if not found
    }
}
