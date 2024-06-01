using System.Collections.Generic;
using UnityEngine;

public class SectionFactory : Factory
{
    public GameObject SectionPrefab;
       
    private float _sectionOffset;
    private int _sectionIndex;
    private GameObject _plane;

    public void InitSection(GameObject plane, int sectionsCount, int sectionIndex)
    {
        _plane = plane;

        Vector3 planeSize = GetBoundsOf(_plane.transform).size;
        float zSize = planeSize.z * _plane.transform.localScale.z;
        _sectionOffset = zSize / sectionsCount;
        _sectionIndex = sectionIndex;
    }    

    public override GameObject CreateObject()
    {
        float currentHeight = SpawnPoint.transform.position.y - (_sectionOffset * _sectionIndex);
        var placedSectionPosition = new Vector3(_plane.transform.position.x, currentHeight, _plane.transform.position.z - 0.5f);
        GameObject placedSection = Instantiate(SectionPrefab, placedSectionPosition, Quaternion.identity);
        placedSection.name = $"Section {_sectionIndex}";
        placedSection.transform.parent = _plane.transform;
        return placedSection;
    }
}
