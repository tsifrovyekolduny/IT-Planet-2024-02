using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class Node
{
    public int Index = -1;
    public Node Destination;
    public bool IsBlocked
    {
        get
        {
            return Index == -1;
        }
    }

    public override string ToString()
    {
        string outputLine = "";
        if (IsBlocked)
        {
            outputLine += "[]";
        }
        else
        {
            if (Destination != null)
            {
                outputLine += $"{Index}({Destination.Index})";
            }
            else
            {
                outputLine += $"{Index}(!!!)";
            }
        }

        return outputLine;
    }
}

public class NodesCollection
{
    private int _rowCapacity;
    private List<Node> _nodes;

    public List<int> GetAllWays()
    {
        return _nodes.Where(node => !node.IsBlocked).Select(node => node.Index).ToList<int>();
    }

    public NodesCollection(int rowCapacity)
    {
        _nodes = new List<Node>();
        _rowCapacity = rowCapacity;
    }

    public Node GetNode(int index)
    {
        return _nodes.First(node => node.Index == index);
    }

    public int Count
    {
        get
        {
            return _nodes.Count;
        }
    }

    public Node this[int index]
    {
        get
        {
            return _nodes[index];
        }
    }

    public void AddObstacleInFrontOf(int behindNodeIndex)
    {
        int indexOfNode = _nodes.IndexOf(GetNode(behindNodeIndex + 1));
        _nodes.Insert(indexOfNode, new Node());
    }
    public void AddNode(int index)
    {
        Node newNode = new Node();
        newNode.Index = index;
        _nodes.Add(newNode);
    }

    public override string ToString()
    {
        string outputLine = "";

        foreach (var section in Sections)
        {
            foreach (Node node in section)
            {
                outputLine += node.ToString();
                outputLine += ", ";
            }
            outputLine += $"\\n";
        }

        return outputLine;
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

    public List<List<Node>> Sections
    {
        get
        {
            List<List<Node>> sections = new List<List<Node>>();
            List<Node> currentSection = null;

            for (int nodeIndex = 0; nodeIndex < Count; ++nodeIndex)
            {
                if (nodeIndex % _rowCapacity == 0)
                {
                    currentSection = null;
                }

                if (currentSection == null)
                {
                    currentSection = new List<Node>();
                    sections.Add(currentSection);
                }
                currentSection.Add(_nodes[nodeIndex]);
            }

            return sections;
        }
    }

    public List<Node> GetSubSection(Node currentNode)
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
    public int NodesCount = 50;
    public int RowCapacity = 5;
    public NodesCollection Nodes;
    public int PeriodicityOfCorrectNode = 10;

    public GameObject Wall;
    public GameObject Hole;
    public GameObject Section;
    public GameObject SectionSpawnPoint;
    public GameObject Plane;
    public GameObject Player;
    private GameObject _player;

    [SerializeField]
    private Node _currentNode;
    [SerializeField]
    private List<Node> _correctWay;

    Node GetRandomNode(int reverseNodeIndex, ref List<int> allWays)
    {
        int startIndex = 0;

        int randomIndex = Random.Range(startIndex, allWays.Count - 1);
        Node randomNode = Nodes.GetNode(allWays[randomIndex]);
        allWays.RemoveAt(randomIndex);

        return randomNode;
    }

    Node FindDestinationNode(int reverseNodeIndex)
    {
        return Nodes.GetNode(reverseNodeIndex);
    }

    void InitDestionations()
    {
        _correctWay = new List<Node>();
        List<int> allWays = Nodes.GetAllWays();
        allWays.RemoveAt(allWays.Count - 1);
        allWays.RemoveAt(0);

        int reverseNodeIndex = NodesCount - 1;

        while (reverseNodeIndex >= 0)
        {
            Node startNode;

            if (reverseNodeIndex == 1)
            {
                Node rootNode = Nodes.GetNode(0);
                rootNode.Destination = Nodes.GetNode(1);
                break;
            }
            else
            {
                startNode = GetRandomNode(reverseNodeIndex, ref allWays);
                startNode.Destination = FindDestinationNode(reverseNodeIndex);
            }

            --reverseNodeIndex;
        }
    }

    void InitObstacles()
    {
        int countOfObstacles = NodesCount / 3;
        List<int> usedIndexes = new List<int>();
        while (countOfObstacles > 0)
        {
            int randomIndex = Random.Range(1, NodesCount - 1);
            if (usedIndexes.Contains(randomIndex))
            {
                continue;
            }

            usedIndexes.Add(randomIndex);
            Nodes.AddObstacleInFrontOf(randomIndex);
            --countOfObstacles;
        }
        Nodes.AddObstacleInFrontOf(0);
        Nodes.AddObstacleInFrontOf(NodesCount - 2);
    }

    void DrawAll()
    {
        var sections = Nodes.Sections;

        Vector3 planeSize = Plane.GetComponent<MeshRenderer>().localBounds.size;

        float sectionOffset = planeSize.x / sections.Count;
        float nodeOffset = planeSize.z / RowCapacity;

        for (int sectionIndex = 0; sectionIndex < sections.Count; ++sectionIndex)
        {
            var section = sections[sectionIndex];

            float currentHeight = SectionSpawnPoint.transform.position.y - (sectionOffset * sectionIndex);
            var newSectionPosition = new Vector3(Plane.transform.position.x, currentHeight, Plane.transform.position.z);
            GameObject newSection = Instantiate(Section, newSectionPosition, Quaternion.identity, Plane.transform);

            newSection.name += sectionIndex;

            Vector3 sectionSize = newSection.GetComponent<MeshRenderer>().localBounds.size;
            for (int nodeIndex = 0; nodeIndex < section.Count; ++nodeIndex)
            {
                float currentNodeOffset = nodeOffset * nodeIndex;
                var newNodePosition = new Vector3(SectionSpawnPoint.transform.position.x + currentNodeOffset, currentHeight + (sectionSize.y / 2), Plane.transform.position.z);
                Node node = section[nodeIndex];

                GameObject nodePrefab = node.IsBlocked ? Wall : Hole;
                GameObject createdNode = Instantiate(nodePrefab, newNodePosition, Quaternion.identity);
                createdNode.transform.parent = newSection.transform;
                createdNode.name = node.ToString();
            }
        }
    }
    
    void Start()
    {
        Nodes = new NodesCollection(RowCapacity);
        for (int nodeIndex = 0; nodeIndex < NodesCount; ++nodeIndex)
        {
            Nodes.AddNode(nodeIndex);
        }
        _currentNode = Nodes.GetNode(0);
        
        InitDestionations();
        Debug.Log("Nodes initialized:\\n" + Nodes.ToString());

        InitObstacles();
        Debug.Log("Destinations initialized:\\n" + Nodes.ToString());

        DrawPlayer(SectionSpawnPoint.transform.position);
        DrawAll();
    }

    void DrawPlayer(Vector3 spawnPoint)
    {
        float halfOfHeight = Player.GetComponent<BoxCollider>().bounds.size.y;
        spawnPoint = new Vector3(spawnPoint.x, spawnPoint.y + halfOfHeight, spawnPoint.z);
        _player = Instantiate(Player, spawnPoint, Quaternion.identity);
        
        _player.GetComponent<LabirynthPlayerScript>().HoleEnteredEvent.AddListener(MovePlayerToNode);
    }    
    
    public void MovePlayerToNode(string nodeName)
    {
        Regex regex = new Regex(@"\d*");
        var matches = regex.Matches(nodeName);
        Node startNode = Nodes.GetNode(int.Parse(matches[0].Value));
        Node endNode = Nodes.GetNode(int.Parse(matches[2].Value));

        Destroy(_player);

        GameObject endNodeGameObject = GameObject.Find(endNode.ToString());
        Vector3 newPosition = new Vector3(endNodeGameObject.transform.position.x,
            endNodeGameObject.transform.position.y,
            SectionSpawnPoint.transform.position.z);

        DrawPlayer(newPosition);
        _currentNode = endNode;
    }
    void Update()
    {

    }
}
