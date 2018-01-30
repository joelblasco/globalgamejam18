using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreator : MonoBehaviour
{
    #region VARIABLES
    public int _nodes;
    public NodeClass[] Nodes;
    private GameObject[] Sprites;
    [SerializeField]public GameObject SpriteRoot;
    public NodeClass currentNode;
    #endregion
    private void Start()
    {
        NewMission(3);
    }
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //NewMission(2);
        }
#endif
    }
    public void NewMission(int _difficulty)
    {
        _nodes = _difficulty * 5;
        Nodes = new NodeClass[_nodes];
        Sprites = new GameObject[_nodes];
        for (int i = 0, r = 0; i < _nodes; i++,r+=4)
        {
            //print(i);
            Nodes[i] = gameObject.AddComponent(typeof(NodeClass)) as NodeClass;
            Nodes[i].myCreator = this;
            Nodes[i].NodeNum = i + 1; 
            Nodes[i].ID = Random.Range(1000, 9999);
            Sprites[i] = SpriteRoot.transform.GetChild(i).gameObject;
            Nodes[i].mySprite = Sprites[i];
            Nodes[i].NodeInfoStartIndex = r;
        }
        currentNode = Nodes[0];
    }
    public void RequestPort(int PortRequested, NodeClass RequesterNode)
    {
        int i = Random.Range(0, _nodes); // random node
        int newNodeID = Nodes[i].ID;
        for (int p = 0; p < RequesterNode.Port.Length; p++) //Check all ids in the port.
        {
            if (newNodeID == RequesterNode.Port[p] || newNodeID == RequesterNode.ID) //check if the new ID is already used by the requester, or is like the requester ID
            {
                RequestPort(PortRequested, RequesterNode);
                //print("i found repetition in ID: " + RequesterNode.ID);
                return;
            }
        }
        RequesterNode.Port[PortRequested] = newNodeID;
        //print("NODE REF ID: " + RequesterNode.ID + " | Port assigned: " + RequesterNode.Port[PortRequested]);

    }

}
