using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConsoleDisplay : MonoBehaviour
{
    #region VARIABLES 
    private int line;
    [SerializeField]
    [Range(5, 15)]
    int maxLines;
    [SerializeField]
    string[] ContentLine;
    private InputField myInput;
    [SerializeField]
    Text contentInput; //not writable, only to show the previous lines input
    [SerializeField]
    string prevLine, currentLine;
    [SerializeField]
    NodeCreator NodeManager;
    bool WaitingID, WaitingPass, CorrectPass;
    [SerializeField]
    NodeClass tryingNode;
    string word1, word2, unlockedIDs;
    [SerializeField]
    GameObject LRenderPref;
    LineRenderer oldLR;
    [SerializeField]
    float Bitcoins;
    int mins,secs;
    [SerializeField]Text TimeText;
    [SerializeField]AudioSource myAudios,RuskyAudio;
    [SerializeField]PlayerAnimations player;
    #endregion


    void Start()
    {
        //mins = 0.7 * NodeManager._nodes;
        Bitcoins = PlayerPrefs.GetFloat("money");
        if (Bitcoins < 1)
        {
            Bitcoins = 600;
        }
        ContentLine = new string[maxLines];
        myInput = GetComponent<InputField>();
        newLine(true);
        NodeManager.currentNode.mySprite.GetComponent<Image>().color = Color.green;
        NodeManager.currentNode.mySprite.transform.GetComponentInChildren<Text>().text = NodeManager.currentNode.ID.ToString("[0000]");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            newLine(false);
        }
        //time -= 1 * Time.deltaTime;
        TimeText.text = Time.time.ToString("000")+"/600";
        if (Time.timeSinceLevelLoad > 600 && Time.timeSinceLevelLoad<610)
        {
            myInput.text = "LOSER LOSER LOSER LOSER LOSER LOSER LOSER LOSER ";
            newLine(true);
        }
    }
    private void newLine(bool LineReaded)
    {
        line++;
        //read previous line and delete
        prevLine = myInput.text;
        contentInput.text = ""; // delete prev content
        myInput.text = ""; //delete the line
        int _length = ContentLine.Length;
        //print(_length);
        for (int i = 0; i < _length; i++)
        {
            if (i + 1 < _length) //check if the new index is in the array length
            {
                ContentLine[i] = ContentLine[i + 1];
            }
            else
            {
                ContentLine[i] = prevLine;
            }
            if (ContentLine[i] != null)
            {
                contentInput.text += ">" + SystemInfo.deviceName.ToLower() + "/main/directory/" + ContentLine[i] + "\n"; //build the text with all content
            }
            else
            {
                contentInput.text += ContentLine[i] + "\n";
            }
        }

        //read the line 
        myInput.ActivateInputField();
        if (LineReaded) return;
        ReadLine(prevLine);
    }
    private void ReadLine(string _input)
    {
        switch (_input) // COMMANDS
        {
            default: // IMPORTANT
                DisplayDefault(_input);
                break;
            case "help": //display all commands (max 11)
                DisplayHelp(1);
                WaitingPass = false;
                break;
            case "help2":
                DisplayHelp(2);
                WaitingPass = false;
                break;
            case "exit":
                Application.Quit();
                Debug.LogWarning("QUIT");
                WaitingPass = false;
                break;
            case "clear":
                int _length = ContentLine.Length;
                for (int i = 0; i < _length; i++)
                {
                    ContentLine[i] = null;
                }
                contentInput.text = "";
                WaitingPass = false;
                return;
            case "current":
                DisplayCurrent();
                WaitingPass = false;
                break;
            case "openports":
                DisplayAvailable();
                WaitingPass = false;
                break;
            #region GET HINTS
            case "getCredentials":
                if (Bitcoins >= 70)
                {
                    Bitcoins -= 70;
                    if (tryingNode == null)
                    {
                        DisplayError(1);
                        break;
                    }
                    DisplayCredentials(tryingNode.ID);
                }
                else
                {
                    DisplayError(2);
                }
                WaitingPass = false;
                break;
            case "getData":
                if (Bitcoins >= 90)
                {
                    Bitcoins -= 90;
                    if (tryingNode == null)
                    {
                        DisplayError(1);
                        break;
                    }
                    DisplayData(tryingNode.ID);
                }
                else
                {
                    DisplayError(2);
                }
                WaitingPass = false;
                break;
            case "getRegion":
                if (Bitcoins >= 10)
                {
                    Bitcoins -= 10;
                    if (tryingNode == null)
                    {
                        DisplayError(1);
                        break;
                    }
                    DisplayRegion(tryingNode.ID);
                }
                else
                {
                    DisplayError(2);
                }
                WaitingPass = false;
                break;
            #endregion
            case "unlocked":
                DisplayUnlocked();
                WaitingPass = false;
                break;
            case "time":
                DisplayTime();
                WaitingPass = false;
                break;
            case "wallet":
                DisplayWallet();
                WaitingPass = false;
                break;
            case "nodes":
                myInput.text = "actual/nodes/" + NodeManager._nodes;
                newLine(true);
                WaitingPass = false;
                break;
            case "timeleft":
                myInput.text = "";
                newLine(true);
                WaitingPass = false;
                break;
            case "newMission":
                PlayerPrefs.SetFloat("money", Bitcoins);
                SceneManager.LoadScene(0);
                break;
            case "restart":
                PlayerPrefs.SetFloat("money", 600);
                SceneManager.LoadScene(0);
                break;
            case "cd..":
                myInput.text = "Сука Блять Информатик";
                newLine(true);
                break;
            #region CREDITS
            case "credit:joel":
                Application.OpenURL("http://www.joelblasco.com/");
                break;
            case "credit:monica":
                Application.OpenURL("https://areikan80.wixsite.com/monica3d");
                break;
            case "credit:jorge":
                Application.OpenURL("http://jorgemaynero.wixsite.com/portfolio");
                break;
            case "credit:lorena":
                Application.OpenURL("http://lorenanumeroseis.wixsite.com/lorenablasco");
                break;
            case "credit:marcos":
                Application.OpenURL("https://www.instagram.com/un_gamer_mas1/");
                break;
                #endregion
        }
    }
    private void DisplayHelp(int _page)
    {
        switch (_page)
        {
            case 1:
                myInput.text = "access:ID'/info>'Access a concrete node with ID if possible.'";
                newLine(true); //1
                myInput.text = "getCredentials/info>'Get full password but scrambled, cost 70Bc.'";
                newLine(true); //2
                myInput.text = "getData/info>'Get a value related with the password, cost 10Bc'";
                newLine(true); //3
                myInput.text = "getRegion/info>'Get a word related with the password, cost 90Bc.'";
                newLine(true);//4
                myInput.text = "openports/info>'Show which nodes are open in ports.'";
                newLine(true);//5
                myInput.text = "clear/info>'Delete the console.'";
                newLine(true);//6
                myInput.text = "current/info>'Display info of the current status.'";
                newLine(true);//7
                myInput.text = "unlocked/info>'Show all IDs unlocked.'";
                newLine(true);//8
                myInput.text = "nodes/info>'Show the amount of nodes in this system.'";
                newLine(true);//9
                myInput.text = "time/info>'Show the actual time UHC.'";
                newLine(true);//10
                myInput.text = "wallet/info>'Show the amount of money in your virtual wallet.'";
                newLine(true);//11
                myInput.text = "help2/info>'Help page 02/02.'";
                newLine(true);//12
                break;
            case 2:
                myInput.text = "exit/info>'Saves files and close the computer.'";
                newLine(true); //1
                myInput.text = "credit:'name'/info>'Show developer profile.'";
                newLine(true); //2
                myInput.text = "timeleft/info>'Show time left of the current mission.'";
                newLine(true); //3
                myInput.text = "newMission/info>'Load a new mission.'";
                newLine(true);//4
                myInput.text = "<Your job is to unlock all nodes, for this, you>";
                newLine(true);//5
                myInput.text = "<can first see which ports are open. Then you >";
                newLine(true);//6
                myInput.text = "<choose a node and access it with (access:'id')>";
                newLine(true);//7
                myInput.text = "<then, buy a hint, and try to solve the pass,>";
                newLine(true);//8
                myInput.text = "<all passwords are important cities. I hope >";
                newLine(true);//9
                myInput.text = "<you and Mosquito(the hacker) have as much>";
                newLine(true);//10
                myInput.text = "<fun as I had doing this game in the GGJ18!>";
                newLine(true);//11
                break;
        }
    }
    private void TryAccesNode(int _ID)
    {
        for (int i = 0; i < NodeManager._nodes; i++) //Check all nodes in NodeManager...
        {
            if (NodeManager.Nodes[i].ID == _ID) // If we found the ID we are looking for... 
            {
                bool setNode = false; //Create a bool that will allow us to set the node we want if the is is in the list of available.
                //print("ID: " + _ID);
                for (int p = 0; p < NodeManager.currentNode.Port.Length; p++) //Check all ids in the port.
                {
                    //print("PORT[i]: " + NodeManager.currentNode.Port[p]);
                    if (_ID == NodeManager.currentNode.Port[p]) //If we found the ID(input by player) in the list of available:
                    {
                        setNode = true; //set the bool to true.
                    }
                }
                if (setNode) { tryingNode = NodeManager.Nodes[i]; tryingNode.mySprite.GetComponent<Image>().color = Color.yellow; } //if we are allowed to change the node, change it
                else { DisplayError(0); print("EXIT1"); return; } // if the ID entered by the player hasn't been found, we are not allowed to set the trying node.
            }
        }
        if (tryingNode != null && tryingNode.Unlocked)
        {
            AccessComplete(tryingNode);
        }
        else
        {
            AccessDenied(_ID);
        }
    }
    private void AccessComplete(NodeClass newCurrent)
    {
        if (tryingNode.ID == NodeManager.currentNode.ID)
        {
            myInput.text = "current/info/" + NodeManager.currentNode.ID + " <This is the current node>";
            newLine(true);
        }
        else
        {
            if (oldLR != null)
            {
                oldLR.endColor = Color.cyan;
            }
            NodeManager.currentNode.mySprite.GetComponent<Image>().color = Color.cyan;
            Vector3 oldPos = NodeManager.currentNode.mySprite.gameObject.transform.position; //for Line Renderer
            Bitcoins += 30;
            NodeManager.currentNode = newCurrent;
            NodeManager.currentNode.mySprite.transform.GetComponentInChildren<Text>().text = NodeManager.currentNode.ID.ToString("[0000]");
            myInput.text = "current/info/" + NodeManager.currentNode.ID + " <Access completed>";
            newLine(true);
            NodeManager.currentNode.mySprite.GetComponent<Image>().color = Color.green;
            Vector3 newPos = NodeManager.currentNode.mySprite.gameObject.transform.position; //for Line Renderer

            player.teclar_anim();
            RuskyAudio.Play();

            LineRenderer newLR = Instantiate(LRenderPref).GetComponent<LineRenderer>();
            oldPos.z = 2.4f;
            newPos.z = 2.4f;
            newLR.SetPosition(0, oldPos);
            newLR.SetPosition(1, newPos);
            oldLR = newLR;
        }
    }
    private void AccessDenied(int _ID)
    {
        myInput.text = "current/info/" + _ID + " <Access denied, enter a valid password below.>";
        WaitingPass = true;
        newLine(true);
    }
    private void DisplayDefault(string _input)
    {
        //print(WaitingID + " | " + WaitingPass); //CHECK
        #region CHECK IF PASSWORD IS CORRECT
        if (tryingNode != null)
        {
            word1 = _input;
            word2 = tryingNode.thisPassword;
            //print("," + _input + ", | ," + tryingNode.thisPassword + ",");
            char[] c_word1 = word1.ToCharArray();
            char[] c_word2 = word2.ToCharArray();
            for (int a = 0,b=0; a < c_word1.Length&&b<c_word2.Length; a++,b++)
            {
                if (char.Equals(c_word1[a], c_word2[b]))
                {
                    print("true");
                    CorrectPass = true;
                }
                else
                {
                    print("false");
                    CorrectPass = false;
                }
            }

        }
        #endregion
        if (WaitingPass)
        {
            if (CorrectPass)
            {
                myInput.text = tryingNode.ID + "/unlocked.true <Node unlocked, this is your current node now.>";
                newLine(true);
                tryingNode.Unlocked = true;
                AccessComplete(tryingNode);
                WaitingPass = false;
            }
            else
            {
                myAudios.Play();
                if (tryingNode != null) { myInput.text = tryingNode.ID + "/unlocked.false <Password: Login error, syntax does not match.>"; }
                else { DisplayError(0); }
                newLine(true);
                WaitingPass = false;
            }
        }
        else if (_input.Contains("access:"))
        {
            string[] cont = _input.Split(':');
            int result;
            bool parsed = int.TryParse(cont[1], out result);
            if (parsed)
            {
                TryAccesNode(result);
            }
            else
            {
                myAudios.Play();
                myInput.text = "info/ <Access failed, input must be a valid ID.>";
                newLine(true);
            }
        }
        else
        {
            myAudios.Play();
            myInput.text = "/:<" + prevLine + "> was not found.Type 'help' for a list of available commands. ";
            newLine(true);
        }
    }
    private void DisplayCurrent()
    {
        myInput.text = "current/" + NodeManager.currentNode.ID;
        newLine(true);
    }
    private void DisplayAvailable()
    {
        for (int i = 0; i < NodeManager.currentNode.Port.Length; i++)
        {
            myInput.text = "ports/availableIDs/" + NodeManager.currentNode.Port[i];
            newLine(true);
        }
    }
    private void DisplayCredentials(int _ID)
    {
        myInput.text = _ID + "/credentials.exe/" + tryingNode.thisCredential;
        newLine(true);
    }
    private void DisplayData(int _ID)
    {
        myInput.text = _ID + "/data.exe/" + tryingNode.thisData;
        newLine(true);
    }
    private void DisplayRegion(int _ID)
    {
        myInput.text = _ID + "/region.exe/" + tryingNode.thisRegion;
        newLine(true);
    }
    private void DisplayError(int error)
    {
        myAudios.Play();
        switch (error)
        {
            default:
                myInput.text = "/:<Unknown error. Type 'help' for a list of available commands.>";
                newLine(true);
                break;
            case 1:
                myInput.text = "/:<Information accesibilty is null. Type 'help' for a list of available commands.>";
                newLine(true);
                break;
            case 2:
                myInput.text = "/:<  Wallet is empty or there is not enough currency, Type 'restart' to  ";
                newLine(true);
                myInput.text = "/:<  restart the game with 600 Bitcoins.";
                newLine(true);
                break;
        }

    }
    private void DisplayUnlocked()
    {
        unlockedIDs = "";
        for (int i = 0; i < NodeManager.Nodes.Length; i++)
        {
            if (NodeManager.Nodes[i].Unlocked)
            {
                if (i == 0)
                { unlockedIDs = "" + NodeManager.Nodes[i].ID; }
                else
                {
                    unlockedIDs += ", " + NodeManager.Nodes[i].ID;
                }
            }
        }
        myInput.text = "unlocked/(" + unlockedIDs + ")";
        newLine(true);
    }
    private void DisplayTime()
    {
        myInput.text = "" + System.DateTime.Now;
        newLine(true);
    }
    private void DisplayWallet()
    {
        myInput.text = " - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ";
        newLine(true);
        myInput.text = "                                                                       ";
        newLine(true);
        myInput.text = "                                            $  " + Bitcoins + "  $                                            ";
        newLine(true);
        myInput.text = "                                                                       ";
        newLine(true);
        myInput.text = " - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ";
        newLine(true);
    }
}
