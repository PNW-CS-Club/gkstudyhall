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
    [SerializeField] TMP_Text playerListDisplay;
    [SerializeField] TMP_Text debugDisplay;

    [Header("UI State Groups")] 
    [SerializeField] List<GameObject> hostOrJoinElements;
    [SerializeField] List<GameObject> attemptingJoinElements;
    [SerializeField] List<GameObject> hostingElements;
    [SerializeField] List<GameObject> joiningElements;

    NetworkUIState uiState = NetworkUIState.HostOrJoin;
    bool showIp = false;

    void Start()
    {
        debugDisplay.text = "";
        netLogic.OnLog += AddDebugLine;

        netLogic.OnConnectionEvent += RespondToClientConnectionEvent;
        
        // show HostOrJoin UI elements
        foreach (GameObject element in GetUIStateElements(NetworkUIState.HostOrJoin)) 
            element.SetActive(true);
    }

    void Update()
    {
        // update all the text displays
        attemptIpDisplay.text = netLogic.Ip;
        ipDisplay.text = showIp ? netLogic.Ip : "XXX.XXX.XXX.XXX";
        menuTitle.text = GetUIStateTitle(uiState);
        
        netLogic.usernames = GameObject.FindGameObjectsWithTag("NetPlayer")
            .Select(go => go.GetComponent<NetworkPlayer>().username.Value.ToString())
            .ToList();
        
        playerListDisplay.text = netLogic.usernames
            .Aggregate("", (a, b) => a + b + '\n', s => s.TrimEnd());
    }

    void OnDestroy()
    {
        netLogic.OnLog -= AddDebugLine;
        netLogic.OnConnectionEvent -= RespondToClientConnectionEvent;
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
                if (nwm.IsHost && uiState != NetworkUIState.Hosting)
                    ChangeUIState(NetworkUIState.Hosting);
                else if (!nwm.IsHost)
                    ChangeUIState(NetworkUIState.Joining);
                break;
            
            case ConnectionEvent.ClientDisconnected:
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
