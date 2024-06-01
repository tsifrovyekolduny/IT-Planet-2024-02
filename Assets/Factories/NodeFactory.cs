using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NodeFactory : Factory
{
    public GameObject HolePrefab;
    public GameObject WallPrefab;
    public override GameObject CreateObject(object nodes)
    {
        List<List<Node>> listOfNode = nodes as List<List<Node>>;

        return gameObject;
    }
}