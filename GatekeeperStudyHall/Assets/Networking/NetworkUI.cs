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

    /// The canonical instance of this class (more than one cannot exist simultaneously)
    public static NetworkUI Instance;

    public NetworkUIState UiState { get; private set; } = NetworkUIState.HostOrJoin;
    NetworkSetup netSetup;
    bool showIp = false;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("A NetworkUI component already exists! Destroying self...");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        Logger.OnLog += AddDebugLine;
    }

    void OnDestroy()
    {
        // remember to cancel all your subscriptions before you die
        Logger.OnLog -= AddDebugLine;
    }
    
    void AddDebugLine(string line) => debugDisplay.text += line + "\n";

    void Start()
    {
        netSetup = NetworkSetup.Instance;
        
        debugDisplay.text = "";
        
        // show HostOrJoin UI elements
        foreach (GameObject element in GetUIStateElements(NetworkUIState.HostOrJoin)) 
            element.SetActive(true);
    }

    void Update()
    {
        // update anything that doesn't directly rely on network init
        menuTitle.text = GetUIStateTitle(UiState);
        attemptIpDisplay.text = netSetup.HostIp;
        ipDisplay.text = showIp ? netSetup.HostIp : "XXX.XXX.XXX.XXX";

        var netRoot = NetworkRoot.Instance;
        if (netRoot != null)
        {
            // update the displays that show the network players
            playerListHeader.text = $"Connected Players [{netRoot.PlayerCount}/4]";
            playerListDisplay.text = netRoot.netPlayers
                .Select(go => go.username.Value.ToString()) // get their usernames
                .Aggregate("", (a, b) => a + b + '\n', s => s.TrimEnd()); // concatenate them
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
        Logger.Log("Copied IP to clipboard.");
    }

    public void Shutdown() => NetworkRoot.Instance.netLogic.ShutdownHost();

    public void StartGame() => NetworkRoot.Instance.netLogic.ToCharSelect_Rpc();

    public void RespondToClientConnectionEvent(NetworkManager nwm, ConnectionEventData data)
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
    
    public void ChangeUIState(NetworkUIState newState)
    {
        foreach (GameObject element in GetUIStateElements(UiState)) 
            element.SetActive(false);
        
        foreach (GameObject element in GetUIStateElements(newState)) 
            element.SetActive(true);

        UiState = newState;
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
}
