using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeClass : MonoBehaviour {
    #region VARIABLES
    public NodeCreator myCreator;
    public int NodeNum; // the number where the node info come from n*1 for password,n*2 for anagram and n*3 for data and n*4 for region
    private TextAsset infoFile;
    private string[] info;
    [SerializeField] public int ID, SecurityLevel;
    public int[] Port;
    public string thisPassword, thisCredential, thisData, thisRegion;
    public bool Unlocked;
    public GameObject mySprite;
    public int NodeInfoStartIndex;
    #endregion

    private void Start()
    {
        if (NodeNum == 1) Unlocked = true;
        //GET RESOURCES
        infoFile = Resources.Load("nodesinfo", typeof(TextAsset)) as TextAsset;
        info = infoFile.text.Split('\n');

        //SET NODE INFO
        thisPassword =   info[NodeInfoStartIndex];
        thisCredential = info[NodeInfoStartIndex+1];
        thisData =       info[NodeInfoStartIndex+2];
        thisRegion =     info[NodeInfoStartIndex+3];

        #region RANDOM :D
        int newRandom = Random.Range(0, 15);
        switch (newRandom)
        {
            default:
                SecurityLevel = 2;
                break;
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
                SecurityLevel = 3;
                break;
            case 13:
            case 14:
            case 15:
                SecurityLevel = 1;
                break;
        }
        #endregion
        Port = new int[SecurityLevel]; //of this depend how many ways the node can be accessed
        for (int _port = 0; _port < Port.Length; _port++)
        {
            myCreator.RequestPort(_port,this);
        }
    }
}
