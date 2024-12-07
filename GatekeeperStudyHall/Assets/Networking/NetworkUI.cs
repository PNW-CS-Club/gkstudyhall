using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum NetworkUIState
{
    HostOrJoin, Hosting, Joining
}


public class NetworkUI : MonoBehaviour
{
    [SerializeField] NetworkLogic netLogic;
    
    [Header("UI Elements")]
    [SerializeField] TMP_Text menuTitle;
    [SerializeField] TMP_InputField ipInput;
    [SerializeField] TMP_Text ipDisplay;
    [SerializeField] TMP_Text debugDisplay;

    [Header("UI State Groups")] 
    [SerializeField] List<GameObject> hostOrJoinElements;
    [SerializeField] List<GameObject> hostingElements;
    [SerializeField] List<GameObject> joiningElements;

    NetworkUIState uiState = NetworkUIState.HostOrJoin;
    bool showIp = false;
    
    void Start()
    {
        debugDisplay.text = "";
        netLogic.OnLog += AddDebugLine;
        
        foreach (GameObject element in GetUIStateElements(uiState)) 
            element.SetActive(true);
    }

    void Update()
    {
        ipDisplay.text = showIp ? netLogic.Ip : "XXX.XXX.XXX.XXX";
        menuTitle.text = GetUIStateTitle(uiState);
    }
    
    
    public void ReturnToMainMenu() => _ = SceneManager.LoadSceneAsync("StartScene");

    public void TryHosting()
    {
        if (netLogic.BecomeHost())
            ChangeUIState(NetworkUIState.Hosting);
    }

    public void TryJoining()
    {
        if (netLogic.BecomeClient(ipInput.text.Trim()))
            ChangeUIState(NetworkUIState.Joining);
    }

    public void ShowIp(bool show) => showIp = show;

    public void CopyIpToClipboard()
    {
        GUIUtility.systemCopyBuffer = netLogic.Ip;
        AddDebugLine("Copied IP to clipboard.");
    }

    public void Shutdown()
    {
        netLogic.Shutdown();
        ChangeUIState(NetworkUIState.HostOrJoin);
    }
    
    public void StartGame() => _ = SceneManager.LoadSceneAsync("CharSelectScene");
    
    
    private void AddDebugLine(string line) => debugDisplay.text += line + "\n";
    
    private List<GameObject> GetUIStateElements(NetworkUIState state) => state switch
    {
        NetworkUIState.HostOrJoin => hostOrJoinElements,
        NetworkUIState.Hosting => hostingElements,
        NetworkUIState.Joining => joiningElements,
        _ => throw new System.NotImplementedException()
    };
    
    private string GetUIStateTitle(NetworkUIState state) => state switch
    {
        NetworkUIState.HostOrJoin => "LAN Game",
        NetworkUIState.Hosting => "Host LAN Game",
        NetworkUIState.Joining => "Join LAN Game",
        _ => throw new System.NotImplementedException()
    };
    
    private void ChangeUIState(NetworkUIState newState)
    {
        foreach (GameObject element in GetUIStateElements(uiState)) 
            element.SetActive(false);
        
        foreach (GameObject element in GetUIStateElements(newState)) 
            element.SetActive(true);
        
        uiState = newState;
        AddDebugLine($"Changed UI State to {newState}");
    }
}
