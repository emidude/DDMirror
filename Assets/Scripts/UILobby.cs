using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    [SerializeField] InputField joinMatchInut;
    [SerializeField] Button joinButton;
    [SerializeField] Button hostButton;
    public void Host()
    {
        joinMatchInut.interactable = false;
        joinButton.interactable = false;
        hostButton.interactable = false;

        LobbyPlayer.localPlayer.HostGame();
    }

    public void Join()
    {
        joinMatchInut.interactable = false;
        joinButton.interactable = false;
        hostButton.interactable = false;
    }
}
