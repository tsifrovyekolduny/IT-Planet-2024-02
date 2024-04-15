using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class Node
{
    public int Index = -1;
    public Node _destination;
    public Node Destination
    {
        get
        {
            return _destination;
        }
        set
        {
            value.Parent = this;
            _destination = value;
        }
    }
    public Node Parent;

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

    public int IndexOfSubSection(List<Node> nodeSubSection)
    {
        var allSubSections = SubSections;
        for (int indexOfSubSection = 0; indexOfSubSection < allSubSections.Count; ++indexOfSubSection)
        {
            var subSection = allSubSections[indexOfSubSection];
            if (nodeSubSection.Count == subSection.Count)
            {
                for (int indexOfNode = 0; indexOfNode < nodeSubSection.Count; ++indexOfNode)
                {
                    if (nodeSubSection[indexOfNode] != subSection[indexOfNode])
                    {
                        break;
                    }

                    if (indexOfNode == nodeSubSection.Count - 1)
                    {
                        return indexOfSubSection;
                    }
                }
            }
        }

        return -1;
    }

    public int IndexOfSubSection(Node node)
    {
        var nodeSubSection = GetSubSection(node);
        return IndexOfSubSection(nodeSubSection);
    }

    public List<List<Node>> SubSections
    {
        get
        {
            List<List<Node>> subSections = new List<List<Node>>();

            foreach (var section in Sections)
            {
                List<Node> subSection = new List<Node>();

                foreach (Node node in section)
                {
                    if (!node.IsBlocked)
                    {
                        subSection.Add(node);
                    }
                    else
                    {
                        if (subSection.Count > 0)
                        {
                            subSections.Add(subSection);
                        }
                        subSection = new List<Node>();
                    }
                }
                if (subSection.Count > 0)
                {
                    subSections.Add(subSection);
                }
            }

            return subSections;
        }
    }

    public void InitDestionations(int nodesCount)
    {
        List<int> allWays = GetAllWays();
        allWays.RemoveAt(allWays.Count - 1);
        allWays.RemoveAt(0);

        int reverseNodeIndex = nodesCount - 1;

        while (reverseNodeIndex >= 0)
        {
            Node startNode;

            if (reverseNodeIndex == 1)
            {
                Node rootNode = GetNode(0);
                rootNode.Destination = GetNode(1);
                break;
            }
            else
            {
                startNode = GetRandomNode(reverseNodeIndex, ref allWays);
                startNode.Destination = GetNode(reverseNodeIndex);
            }

            --reverseNodeIndex;
        }
    }

    Node GetRandomNode(int reverseNodeIndex, ref List<int> allWays)
    {
        int startIndex = 0;
        int randomIndex = 0;

        Node randomNode;
        Node endNode = GetNode(reverseNodeIndex);
        randomNode = endNode;

        while (endNode == randomNode)
        {
            randomIndex = Random.Range(startIndex, allWays.Count - 1);
            randomNode = GetNode(allWays[randomIndex]);

            if (allWays.Count == 1 && randomNode == endNode)
            {
                randomNode = GetNode(Count / 2);
                break;
            }
        }

        allWays.RemoveAt(randomIndex);

        return randomNode;
    }

    public void InitObstacles(int nodesCount)
    {
        int countOfObstacles = nodesCount / 3;
        List<int> usedIndexes = new List<int>();
        while (countOfObstacles > 0)
        {
            int randomIndex = Random.Range(1, nodesCount - 1);
            if (usedIndexes.Contains(randomIndex))
            {
                continue;
            }

            usedIndexes.Add(randomIndex);
            AddObstacleInFrontOf(randomIndex);
            --countOfObstacles;
        }
        AddObstacleInFrontOf(0);
        AddObstacleInFrontOf(nodesCount - 2);
    }
    public List<Node> GetSubSection(Node currentNode)
    {
        return SubSections.First(nodes => nodes.Contains(currentNode));
    }
}

public class LabManager : MonoBehaviour
{
    public int NodesCount = 50;
    public int RowCapacity = 5;
    public NodesCollection Nodes;
    public int PeriodicityOfCorrectNode = 10;

    public LabTrackingPlayer Camera;
    public List<GameObject> Walls;
    public GameObject Hole;
    public GameObject Section;
    public GameObject SectionSpawnPoint;
    public GameObject Plane;
    public GameObject Player;
    private GameObject _player;

    private Dictionary<int, Node> _bestOptionDict;

    [SerializeField]
    private Node _currentNode;

    void DrawAll()
    {
        var sections = Nodes.Sections;

        Vector3 planeSize = GetBoundsOf(Plane.transform).size;

        float xSize = planeSize.x * Plane.transform.localScale.x;
        float zSize = planeSize.z * Plane.transform.localScale.z;

        float sectionOffset = zSize / sections.Count;
        float nodeOffset = xSize / RowCapacity;

        for (int sectionIndex = 0; sectionIndex < sections.Count; ++sectionIndex)
        {
            var section = sections[sectionIndex];

            float currentHeight = SectionSpawnPoint.transform.position.y - (sectionOffset * sectionIndex);
            var newSectionPosition = new Vector3(Plane.transform.position.x,
                currentHeight,
                Plane.transform.position.z - 0.5f);
            GameObject newSection = Instantiate(Section, newSectionPosition, Quaternion.identity, Plane.transform);
            newSection.transform.localScale = new Vector3(xSize / 2, newSection.transform.localScale.y, newSection.transform.localScale.z);

            Vector3 sectionSize = GetBoundsOf(newSection.transform).size;
            for (int nodeIndex = 0; nodeIndex < section.Count; ++nodeIndex)
            {
                Node node = section[nodeIndex];
                GameObject nodePrefab = node.IsBlocked ? Walls[Random.Range(0, Walls.Count)] : Hole;

                float currentNodeOffset = nodeOffset * nodeIndex;
                var newNodePosition = new Vector3(SectionSpawnPoint.transform.position.x + currentNodeOffset,
                    currentHeight + PlaceOn(nodePrefab.transform, newSection.transform),
                    newSection.transform.position.z);

                GameObject createdNode = Instantiate(nodePrefab, newNodePosition, nodePrefab.transform.rotation);

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


        Nodes.InitDestionations(NodesCount);
        Debug.Log("Nodes initialized:\\n" + Nodes.ToString());

        Nodes.InitObstacles(NodesCount);
        Debug.Log("Destinations initialized:\\n" + Nodes.ToString());

        _bestOptionDict = new Dictionary<int, Node>();
        InitBestOption(Nodes.GetNode(NodesCount - 1));

        DrawAll();
        GameObject firstNodeObject = GetNodeInScene(Nodes.GetNode(0));
        DrawPlayer();
    }

    void InitBestOption(Node destinationNode)
    {
        if (destinationNode == Nodes.GetNode(0))
        {
            return;
        }
        Node startNode = destinationNode.Parent;
        var subSection = Nodes.GetSubSection(startNode);
        int indexOfStartNode = Nodes.IndexOfSubSection(subSection);
        if (_bestOptionDict.ContainsKey(indexOfStartNode))
        {
            return;
        }
        _bestOptionDict.Add(indexOfStartNode, startNode);

        foreach (Node node in subSection)
        {
            InitBestOption(node);
        }

    }

    GameObject GetNodeInScene(Node node)
    {
        return GameObject.Find(node.ToString());
    }

    Bounds GetBoundsOf(Transform gmObj)
    {
        MeshRenderer meshR;
        meshR = gmObj.GetComponent<MeshRenderer>();
        if (meshR == null)
        {
            List<MeshRenderer> meshs = new List<MeshRenderer>();
            gmObj.GetComponentsInChildrenRecursively<MeshRenderer>(meshs);
            meshR = meshs[0];
            Debug.Log($"No meshR in this parent! First meshR of children: {meshR.ToString()}");
        }
        return meshR.localBounds;
    }

    float PlaceOn(Transform child, Transform platform)
    {
        float halfHeightOfPlatform = (GetBoundsOf(platform).size.y + platform.localScale.y) / 2 - 0.03f;

        return halfHeightOfPlatform;
    }

    void ChangeFocusableForNodesIn(List<Node> subSection, bool focusable)
    {
        foreach (Node node in subSection)
        {
            GetNodeInScene(node).GetComponent<Door>().CanBeFocusable = focusable;
        }
    }

    void DrawPlayer()
    {
        Transform thisNode = GetNodeInScene(_currentNode).transform;        
        float widthOfNode = thisNode.localScale.x;
        float depthOfNode = thisNode.localScale.z * 4;        
        

        Vector3 spawnPoint = new Vector3(thisNode.position.x, thisNode.position.y, thisNode.position.z - depthOfNode);
        Vector3 spawnPointWithOffset = new Vector3(spawnPoint.x + widthOfNode, spawnPoint.y, spawnPoint.z);
        _player = Instantiate(Player, spawnPoint, Quaternion.identity);
        Camera.Player = _player.transform;

        if (_currentNode != Nodes.GetNode(NodesCount - 1))
        {
            int indexOfSubSection = Nodes.IndexOfSubSection(_currentNode);
            Node rightNode = _bestOptionDict[indexOfSubSection];
            GameObject rightNodeGameObject = GetNodeInScene(rightNode);

            ChangeFocusableForNodesIn(Nodes.GetSubSection(_currentNode), true);

            var playerScipt = _player.GetComponent<LabirynthPlayerScript>();

            playerScipt.RightNode = rightNodeGameObject;
            playerScipt.HoleEnteredEvent.AddListener(MovePlayerToNode);
            StartCoroutine(playerScipt.MoveSelf(spawnPointWithOffset));
        }
        else
        {
            // win
        }

    }

    public void MovePlayerToNode(string nodeName)
    {
        Regex regex = new Regex(@"\d*");
        var matches = regex.Matches(nodeName);
        Node startNode = Nodes.GetNode(int.Parse(matches[0].Value));
        Node endNode = Nodes.GetNode(int.Parse(matches[2].Value));

        GetNodeInScene(startNode).GetComponent<Door>().CloseDoor();
        ChangeFocusableForNodesIn(Nodes.GetSubSection(startNode), false);

        _currentNode = endNode;

        Destroy(_player);

        GameObject endNodeGameObject = GetNodeInScene(endNode);        

        DrawPlayer();
    }
    void Update()
    {

    }
}

public static class Extension
{
    public static List<T> GetComponentsInChildrenRecursively<T>(this Transform _transform, List<T> _componentList)
    {
        foreach (Transform t in _transform)
        {
            T[] components = t.GetComponents<T>();

            foreach (T component in components)
            {
                if (component != null)
                {
                    _componentList.Add(component);
                }
            }

            GetComponentsInChildrenRecursively<T>(t, _componentList);
        }

        return _componentList;
    }
}
