using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaManager : MonoBehaviour
{
    public PlayerArena LocalPlayer;
    public Text PlayerNameText;
    public GameObject StartMenu , InGameMenu;

    
    private string color; 


    private void Start()
    {
        
    }
    void Update()
    {

        if (NetworkManager.singleton.isNetworkActive)
        {

            if (LocalPlayer == null)
            {
                FindLocalPlayer();
            }
            else
            {
                ShowReadyMenu();

            }
        }

    }

    public void ReadyButtonHandler()
    {
        LocalPlayer.SendReadyToServer(PlayerNameText.text , color);
        
    }
    public void ColorsButtonHandler( Button colorButton)
    {
        color = colorButton.name;
        Debug.Log("ay 7aga");
      
    }

    void ShowReadyMenu()
    {
        if (NetworkManager.singleton.mode == NetworkManagerMode.ServerOnly)
            return;

        if (LocalPlayer.isReady)
        {
            StartMenu.SetActive(false);
            InGameMenu.SetActive(true);
            return;
        }
      


        StartMenu.SetActive(true);

    }

    void FindLocalPlayer()
    {
        //Check to see if the player is loaded in yet
        if (ClientScene.localPlayer == null)
            return;

        LocalPlayer = ClientScene.localPlayer.GetComponent<PlayerArena>();
    }

}
