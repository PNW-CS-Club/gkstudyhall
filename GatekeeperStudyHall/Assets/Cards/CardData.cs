using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardData", menuName = "CardData")] 
// makes a new entry in asset creation menu (in Project tab)
public class CardData : ScriptableObject
{
    // do not set these attributes anywhere other than the editor
    public new string name;
    public string[] actionNames = new string[4];
    public string[] actionDescriptions = new string[4];
    public Sprite art;

    public override string ToString() {
        string actionStr = "{ ";
        for (int i = 0; i < 3; i++) {
            actionStr += $"({i+1}) {actionNames[i]}: {actionDescriptions[i]}; ";
        }
        actionStr += $"(4) {actionNames[3]}: {actionDescriptions[3]} }}";

        return $"CardData[name={name}, art={art}, actions={actionStr}]";
    }
}
