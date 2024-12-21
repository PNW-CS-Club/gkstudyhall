using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum NetworkUIState
{
    HostOrJoin, AttemptingJoin, Hosting, Joining
}


public class NetworkUI : MonoBehaviour
{
    [SerializeField] NetworkLogic netLogic;
    
    [Header("UI Elements")]
    [SerializeField] TMP_Text menuTitle;
    [SerializeField] TMP_InputField ipInput;
    [SerializeField] TMP_Text attemptIpDisplay;
    [SerializeField] TMP_Text ipDisplay;
    [SerializeField] TMP_Text playerListHeader;
    [SerializeField] TMP_Text playerListDisplay;
    [SerializeField] TMP_Text debugDisplay;

    [Header("UI State Groups")] 
    [SerializeField] List<GameObject> hostOrJoinElements;
    [SerializeField] List<GameObject> attemptingJoinElements;
    [SerializeField] List<GameObject> hostingElements;
    [SerializeField] List<GameObject> joiningElements;

    NetworkUIState uiState = NetworkUIState.HostOrJoin;
    bool showIp = false;
    System.Action<int> UpdateNumPlayers;

    void Start()
    {
        debugDisplay.text = "";
        netLogic.OnLog += AddDebugLine;

        netLogic.AfterConnectionEvent += RespondToClientConnectionEvent;
        
        // show HostOrJoin UI elements
        foreach (GameObject element in GetUIStateElements(NetworkUIState.HostOrJoin)) 
            element.SetActive(true);
        
        // some logic to make the UpdateNumPlayers function replace the underscore in playerListHeader
        int splitIndex = playerListHeader.text.IndexOf('_');
        if (splitIndex == -1) Debug.LogError("playerListHeader.text must contain a '_' character");
        string startText = playerListHeader.text.Substring(0, splitIndex);
        string endText = playerListHeader.text.Substring(splitIndex + 1);
        UpdateNumPlayers = (x => playerListHeader.text = startText + x + endText);
    }

    void Update()
    {
        // update various text displays
        attemptIpDisplay.text = netLogic.Ip;
        ipDisplay.text = showIp ? netLogic.Ip : "XXX.XXX.XXX.XXX";
        menuTitle.text = GetUIStateTitle(uiState);

        // update the displays that show the network players
        var netPlayers = GameObject.FindGameObjectsWithTag("NetPlayer");
        UpdateNumPlayers(netPlayers.Length);
        playerListDisplay.text = netPlayers
            .Select(go => go.GetComponent<NetworkPlayer>().username.Value.ToString()) // get their usernames
            .Aggregate("", (a, b) => a + b + '\n', s => s.TrimEnd()); // concatenate them
    }

    void OnDestroy()
    {
        // technically not necessary if networkUI and netLogic are always destroyed at the same time
        netLogic.OnLog -= AddDebugLine;
        netLogic.AfterConnectionEvent -= RespondToClientConnectionEvent;
    }


    public void ReturnToMainMenu() => _ = SceneManager.LoadSceneAsync("StartScene");

    public void TryHosting() => netLogic.StartHost();
    public void TryJoining()
    {
        var wasSuccessful = netLogic.StartClient(ipInput.text.Trim());
        if (wasSuccessful) ChangeUIState(NetworkUIState.AttemptingJoin);
    }

    public void CancelJoinAttempt()
    {
        netLogic.Shutdown();
        ChangeUIState(NetworkUIState.HostOrJoin);
    }

    public void ShowIp(bool show) => showIp = show;

    public void CopyIpToClipboard()
    {
        GUIUtility.systemCopyBuffer = netLogic.Ip;
        AddDebugLine("Copied IP to clipboard.");
    }

    public void Shutdown() => netLogic.Shutdown();

    public void StartGame() => netLogic.StartGame_Rpc();


    void AddDebugLine(string line) => debugDisplay.text += line + "\n";

    void RespondToClientConnectionEvent(NetworkManager nwm, ConnectionEventData data)
    {
        switch (data.EventType)
        {
            case ConnectionEvent.ClientConnected:
                if (data.ClientId == nwm.LocalClientId)
                    ChangeUIState(nwm.IsHost ? NetworkUIState.Hosting : NetworkUIState.Joining);
                break;
            
            case ConnectionEvent.ClientDisconnected:
                if (data.ClientId == nwm.LocalClientId)
                    ChangeUIState(NetworkUIState.HostOrJoin);
                break;
            
            case ConnectionEvent.PeerConnected:
            case ConnectionEvent.PeerDisconnected:
                break;
        }
    }
    
    List<GameObject> GetUIStateElements(NetworkUIState state) => state switch
    {
        NetworkUIState.HostOrJoin => hostOrJoinElements,
        NetworkUIState.AttemptingJoin => attemptingJoinElements,
        NetworkUIState.Hosting => hostingElements,
        NetworkUIState.Joining => joiningElements,
        _ => throw new System.NotImplementedException()
    };
    
    string GetUIStateTitle(NetworkUIState state) => state switch
    {
        NetworkUIState.HostOrJoin => "LAN Game",
        NetworkUIState.AttemptingJoin => "Join LAN Game",
        NetworkUIState.Hosting => "Host LAN Game",
        NetworkUIState.Joining => "Join LAN Game",
        _ => throw new System.NotImplementedException()
    };
    
    void ChangeUIState(NetworkUIState newState)
    {
        foreach (GameObject element in GetUIStateElements(uiState)) 
            element.SetActive(false);
        
        foreach (GameObject element in GetUIStateElements(newState)) 
            element.SetActive(true);
        
        uiState = newState;
    }
}
