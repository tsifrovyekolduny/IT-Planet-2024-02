using System.Collections.Generic;
using UnityEngine;

public class SectionFactory : Factory
{
    public GameObject SectionPrefab;
    public int SectionIndex;
    public override GameObject CreateObject()
    {
        float currentHeight = SpawnPoint.transform.position.y - (sectionOffset * sectionIndex);
        var placedSectionPosition = new Vector3(Plane.transform.position.x,
currentHeight,
        Plane.transform.position.z - 0.5f);
        return gameObject;
    }
}
