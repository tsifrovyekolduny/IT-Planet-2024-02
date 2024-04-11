using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Node
{
    public int Index;
    public Node Destination;
    public bool IsBlocked
    {
        get
        {
            return Index == null;
        }
    }
}

public class NodesCollection
{
    private int _rowCapacity;
    private List<Node> _nodes;

    public NodesCollection(int rowCapacity)
    {
        _nodes = new List<Node>();
        _rowCapacity = rowCapacity;
    }

    public Node GetNode(int index)
    {
        return _nodes.First(node => node.Index == index);
    }
    public void AddObstacle()
    {
        _nodes.Add(new Node());
    }
    public void AddNode(int index)
    {
        Node newNode = new Node();
        newNode.Index = index;
        _nodes.Add(newNode);
    }
    
    int GetExtremeIndexOfSection(int indexOfNode, bool forward)
    {
        int addValue = forward ? 1 : -1;
        int sectionIndex = indexOfNode / _rowCapacity;

        int sectionStartIndex = (indexOfNode / _rowCapacity) * _rowCapacity;
        int sectionEndIndex = sectionStartIndex + (_rowCapacity - 1);


        int currentIndexOfNode = indexOfNode + addValue;
        Node node = _nodes[currentIndexOfNode];
        while (!node.IsBlocked && 
            currentIndexOfNode > sectionStartIndex && 
            currentIndexOfNode < sectionEndIndex)
        {
            currentIndexOfNode += addValue;
            node = _nodes[currentIndexOfNode];
        }
        return currentIndexOfNode + (-1 * addValue);
    }

    public List<Node> GetSectionOfNodes(Node currentNode)
    {
        List<Node> section = new List<Node>();
        int startIndex = 0;
        int endIndex = _rowCapacity - 1;

        int indexOfNode = _nodes.IndexOf(currentNode);

        startIndex = GetExtremeIndexOfSection(indexOfNode, true);
        endIndex = GetExtremeIndexOfSection(indexOfNode, false);

        return _nodes.Skip(startIndex).Take(endIndex - startIndex).ToList();
    }
}

public class LabManager : MonoBehaviour
{
    public int NodesCount = 10;
    public int RowCapacity = 5;
    public NodesCollection Nodes;

    

    // Start is called before the first frame update
    void Start()
    {
        Nodes = new NodesCollection(RowCapacity);
        for (int nodeIndex = 0; nodeIndex < NodesCount; ++nodeIndex)
        {
            Nodes.AddNode(nodeIndex);
        }

        Nodes.GetNode(1).Destination = Nodes.GetNode(0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
