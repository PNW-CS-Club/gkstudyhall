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

    public static NetworkUIState uiState = NetworkUIState.HostOrJoin;
    NetworkUIState prevState = uiState;
    bool showIp = false;
    Action<int> UpdateNumPlayers;

    NetworkLogic netLogic;
    NetworkRoot netRoot;
    bool isNetStuffInitialized = false;

    void Awake()
    {
        netSetup.OnRootSpawned += InitNetworkStuff;
        netSetup.OnLog += AddDebugLine;
    }

    void InitNetworkStuff(NetworkObject netRootObj)
    {
        netRoot = netRootObj.GetComponent<NetworkRoot>();
        netLogic = netRoot.netLogic;
        
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
        if ( prevState != uiState ) {
            // update anything that doesn't directly rely on network init
            menuTitle.text = GetUIStateTitle(uiState);

            ChangeUIState( uiState );
        }

        if (isNetStuffInitialized)
        {
            attemptIpDisplay.text = netSetup.HostIp;
            ipDisplay.text = showIp ? netSetup.HostIp : "XXX.XXX.XXX.XXX";

            // update the displays that show the network players
            UpdateNumPlayers(netRoot.netPlayers.Count);
            playerListDisplay.text = netRoot.netPlayers
                .Select(go => go.username.Value.ToString()) // get their usernames
                .Aggregate("", (a, b) => a + b + '\n', s => s.TrimEnd()); // concatenate them
        }

        prevState = uiState;
    }

    void OnDestroy()
    {
        // remember to cancel all your subscriptions before you die
        
        netSetup.OnLog -= AddDebugLine;
        
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

    public void TryHosting() {
        bool wasSuccessful = netSetup.StartHost();
        if ( wasSuccessful ) {
            ChangeUIState( NetworkUIState.Hosting );
        }
    }
    public void TryJoining()
    {
        var wasSuccessful = netSetup.StartClient(ipInput.text.Trim());
        if (wasSuccessful) {
            ChangeUIState(NetworkUIState.AttemptingJoin);
        }
    }

    public void CancelJoinAttempt()
    {
        netSetup.ShutdownClient();
        ChangeUIState(NetworkUIState.HostOrJoin);
    }

    public void ShowIp(bool show) => showIp = show;

    public void CopyIpToClipboard()
    {
        GUIUtility.systemCopyBuffer = netSetup.HostIp;
        AddDebugLine("Copied IP to clipboard.");
    }

    public void Shutdown() => netLogic.ShutdownHost();

    public void StartGame() => netLogic.StartGame_Rpc();

    void AddDebugLine(string line) => debugDisplay.text += line + "\n";

    void RespondToClientConnectionEvent(NetworkManager nwm, ConnectionEventData data)
    {
        switch (data.EventType)
        {
            case ConnectionEvent.ClientConnected:
                if (data.ClientId == nwm.LocalClientId)
                    ChangeUIState(NetworkUIState.Joining);
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
    
    public void ChangeUIState(NetworkUIState newState)
    {
        foreach (GameObject element in GetUIStateElements(uiState)) 
            element.SetActive(false);
        
        foreach (GameObject element in GetUIStateElements(newState)) 
            element.SetActive(true);
        
        uiState = newState;
    }
}
