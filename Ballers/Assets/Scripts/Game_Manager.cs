using Boo.Lang;
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : NetworkBehaviour {
    public Player_LVLARACE LocalPlayer;
    public Text PlayerNameText;
    public GameObject StartPanel;
    public GameObject WinnersPanel;
    public Text Winner;
    public System.Collections.Generic.List<Player_LVLARACE> players = new System.Collections.Generic.List<Player_LVLARACE>();


    // Start is called before the first frame update
    void Start() {
        Winner.text = "Winner";
    }

    // Update is called once per frame
    void Update() {

        if (NetworkManager.singleton.isNetworkActive) {
            CheckForWinner();
            if (LocalPlayer == null) {
                FindLocalPlayer();
            }
            else {
                ShowReadyMenu();
                CheckFornewPlayer();
                
            }
        }



    }

    public void ReadyButtonHandler() {
        LocalPlayer.SendReadyToServer(PlayerNameText.text);
    }

    void ShowReadyMenu() {
        if (NetworkManager.singleton.mode == NetworkManagerMode.ServerOnly)
            return;

        if (LocalPlayer.isReady) {
            StartPanel.SetActive(false);
            return;
        }


        StartPanel.SetActive(true);

    }

    void FindLocalPlayer() {
        //Check to see if the player is loaded in yet
        if (ClientScene.localPlayer == null)
            return;

        LocalPlayer = ClientScene.localPlayer.GetComponent<Player_LVLARACE>();
    }



    void CheckFornewPlayer() {  // adds new player in a list when they spawn or connect
        foreach (KeyValuePair<uint, NetworkIdentity> kvp in NetworkIdentity.spawned) {
            Player_LVLARACE comp = kvp.Value.GetComponent<Player_LVLARACE>();

            //Add if new
            if (comp != null && !players.Contains(comp)) {
                players.Add(comp);
            }
        }

    }
 
    void CheckForWinner() { // checks if a player have crossed the finish line (not yet completed but it's working!)

        foreach (Player_LVLARACE player in players) {
            //Debug.Log("player : " + player.playerName + "crossed: " + player.Finish_Line_Crossed);
            if (player.Finish_Line_Crossed == true) {
                WinnersPanel.SetActive(true);
                Winner.text = player.playerName;

            }
        }
    }
        



}
