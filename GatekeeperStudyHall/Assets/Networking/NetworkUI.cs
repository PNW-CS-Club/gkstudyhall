using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum NetworkUIState
{
    HostOrJoin, Hosting, Joining
}


public class NetworkUI : MonoBehaviour
{
    [SerializeField] NetworkLogic netLogic;
    [SerializeField] TMP_Text menuTitle;
    [SerializeField] TMP_InputField ipInput;
    [SerializeField] TMP_Text ipDisplay;
    [SerializeField] TMP_Text debugDisplay;

    [Space] 
    [SerializeField] List<GameObject> hostOrJoinElements;
    [SerializeField] List<GameObject> hostingElements;
    [SerializeField] List<GameObject> joiningElements;

    NetworkUIState uiState = NetworkUIState.HostOrJoin;
    
    void Start()
    {
        debugDisplay.text = "";
        netLogic.OnLog += AddDebugLine;
        
        foreach (GameObject element in GetUIStateElements(uiState)) 
            element.SetActive(true);
    }

    void Update()
    {
        ipDisplay.text = netLogic.Ip ?? "XXX.XXX.XXX.XXX";
        menuTitle.text = GetUIStateTitle(uiState);
    }

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
    
    public void ToHostOrJoin() => ChangeUIState(NetworkUIState.HostOrJoin);
    public void ToHosting() => ChangeUIState(NetworkUIState.Hosting);
    public void ToJoining() => ChangeUIState(NetworkUIState.Joining);
}
