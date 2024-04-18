using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// makes a new entry in asset creation menu (in Project tab)
[CreateAssetMenu(fileName = "New_CardDataSO", menuName = "Scriptable Objects/CardSO")] 
public class CardSO : ScriptableObject
{
    // do not set these attributes anywhere other than the editor
    [Header("Character Info")]
    public new string name;
    public string origin;
    public string title;
    public string element;

    [Header("Traits")]
    public string[] traitNames = new string[4];
    public string[] traitDescriptions = new string[4];
    public Trait[] traits;

    [Header("Flavor Text")]
    public string flavorText;
    public Color flavorTextColor = Color.black;

    [Header("Artwork and Colors")]
    public Color outerColor = new Color(0.25f, 0.25f, 0.25f);
    public Color detailColor = Color.gray;
    public Color innerColor = Color.black;
    public Sprite art;

    public int[] traitList = new int[4];
    public override string ToString() {
        return $"CardSO[name=\"{name}\", origin=\"{origin}\", title=\"{title}\", element=\"{element}\", flavorText=\"{flavorText}\"]";
    }
}
