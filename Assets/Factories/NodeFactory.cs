using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeFactory : Factory
{
    public GameObject Hole;
    public List<GameObject> Walls;

    private Node _node;
    private float _nodeOffset;
    private int _nodeIndex;
    private GameObject _placedSection;

    public void InitNode(Node node, GameObject placedSection, GameObject plane, int nodeIndex, int rowCapacity)
    {
        _node = node;
        _placedSection = placedSection;
        _nodeIndex = nodeIndex;

        Vector3 planeSize = GetBoundsOf(plane.transform).size;
        float xSize = planeSize.x * plane.transform.localScale.x;
        _nodeOffset = xSize / rowCapacity;
    }
    public override GameObject CreateObject()
    {
        GameObject nodePrefab = _node.IsBlocked ? Walls[Random.Range(0, Walls.Count)] : Hole;

        float currentNodeOffset = _nodeOffset * _nodeIndex;
        var newNodePosition = new Vector3(SpawnPoint.transform.position.x + currentNodeOffset,
            _placedSection.transform.position.y + PlaceOn(nodePrefab.transform, _placedSection.transform),
            _placedSection.transform.position.z);

        GameObject createdNode = Instantiate(nodePrefab, newNodePosition, nodePrefab.transform.rotation);

        createdNode.transform.parent = _placedSection.transform;
        createdNode.name = _node.ToString();

        return createdNode;
    }
}