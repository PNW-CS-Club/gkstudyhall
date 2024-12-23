using System;
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
    [SerializeField] NetworkSetup netSetup;
    
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

    NetworkLogic netLogic;
    GameObject netPlayerParent;
    bool isNetStuffInitialized = false;

    void Awake()
    {
        netSetup.OnRootSpawned += InitNetworkStuff;
    }

    void InitNetworkStuff(NetworkObject netRootObj)
    {
        var root = netRootObj.GetComponent<NetworkRoot>();
        
        netLogic = root.netLogic;
        netPlayerParent = root.netPlayerParent;
        
        netLogic.OnLog += AddDebugLine;
        netLogic.AfterConnectionEvent += RespondToClientConnectionEvent;

        // ensures we only do the initialization process one time for this MonoBehaviour instance
        netSetup.OnRootSpawned -= InitNetworkStuff;
        isNetStuffInitialized = true;
    }

    void Start()
    {
        debugDisplay.text = "";
        
        // show HostOrJoin UI elements
        foreach (GameObject element in GetUIStateElements(NetworkUIState.HostOrJoin)) 
            element.SetActive(true);
        
        // some logic to make the UpdateNumPlayers function replace the underscore in playerListHeader
        int splitIndex = playerListHeader.text.IndexOf('_');
        if (splitIndex == -1) Debug.LogError("playerListHeader.text must contain a '_' character");
        string startText = playerListHeader.text.Substring(0, splitIndex);
        string endText = playerListHeader.text.Substring(splitIndex + 1);
        UpdateNumPlayers = (x => playerListHeader.text = startText + x + endText);
        
        UpdateNumPlayers(0);
    }

    void Update()
    {
        // update anything that doesn't directly rely on network init
        menuTitle.text = GetUIStateTitle(uiState);

        if (isNetStuffInitialized)
        {
            attemptIpDisplay.text = netLogic.Ip;
            ipDisplay.text = showIp ? netLogic.Ip : "XXX.XXX.XXX.XXX";

            // update the displays that show the network players
            var netPlayers = netPlayerParent.GetComponentsInChildren<NetworkPlayer>();
            UpdateNumPlayers(netPlayers.Length);
            playerListDisplay.text = netPlayers
                .Select(go => go.username.Value.ToString()) // get their usernames
                .Aggregate("", (a, b) => a + b + '\n', s => s.TrimEnd()); // concatenate them
        }
    }

    void OnDestroy()
    {
        // remember to cancel all your subscriptions before you die
        if (isNetStuffInitialized)
        {
            netLogic.OnLog -= AddDebugLine;
            netLogic.AfterConnectionEvent -= RespondToClientConnectionEvent;
        }
        else
        {
            netSetup.OnRootSpawned -= InitNetworkStuff;
        }
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
