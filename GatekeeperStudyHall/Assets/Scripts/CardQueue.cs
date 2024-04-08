using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CardQueue : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField, Min(0)] float margin = 20f;
    [SerializeField] List<PlayerInfo> playerList; // WARNING: Cognitohazard (looking directly at this list in the inspector may cause errors)
    List<GameObject> cards;

    float expandedHeight;
    float collapsedHeight;
    Vector3 startingOffset;

    int expandedIndex = 0;

    void Start()
    {
        cards = new();

        Vector2 expandedSize = cardPrefab.GetComponent<RectTransform>().sizeDelta;

        expandedHeight = expandedSize.y;
        collapsedHeight = expandedHeight - CardDisplay.COLLAPSE_HEIGHT_DIFF;
        startingOffset = new Vector3(expandedSize.x / 2f, -expandedSize.y / 2f, 0f);

        for (int i = 0; i < playerList.Count; i++) 
        {
            GameObject newCard = Instantiate(cardPrefab, transform);
            cards.Add(newCard);
        }

        RepositionCards();
    }


    public void ChangeExpandedPlayer(int newIndex) 
    {
        if (newIndex < 0 || newIndex >= playerList.Count) 
        {
            Debug.LogError("Bad index: playerList[" + newIndex + "] is invalid.");
            return;
        }

        expandedIndex = newIndex;

        RepositionCards();
    }


    public void Add(PlayerInfo player) 
    { 
        playerList.Add(player);
        GameObject newCard = Instantiate(cardPrefab, transform);
        cards.Add(newCard);

        RepositionCards();
    }


    public void Remove(PlayerInfo player)
    {
        int index = playerList.FindIndex(x => x == player);

        if (index == -1) { return; } // player is not in playerList

        playerList.RemoveAt(index);
        GameObject cardToDestroy = cards[index];
        cards.RemoveAt(index);
        Destroy(cardToDestroy);
        UnityEditor.EditorUtility.SetDirty(gameObject);

        if (playerList.Count == 0) { return; } // no more cards left to display

        expandedIndex %= playerList.Count;

        RepositionCards();
    }


    private void RepositionCards() 
    {
        Vector3 offset = startingOffset;

        for (int i = 0; i < playerList.Count; i++) 
        {
            Transform card = cards[i].transform;
            CardDisplay display = card.GetComponent<CardDisplay>();
            card.localPosition = offset;

            if (playerList[i].card != display.cardData) 
            {
                display.ChangeCardData(playerList[i].card);
            }

            if (i == expandedIndex) 
            {
                display.collapsed = false;
                offset.y -= margin + expandedHeight;
            }
            else 
            {
                display.collapsed = true;
                card.localPosition += CardDisplay.COLLAPSE_HEIGHT_DIFF / 2f * Vector3.up;
                offset.y -= margin + collapsedHeight;
            }
        }
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(CardQueue))]
public class CardQueueEditor : Editor
{
    public override void OnInspectorGUI() {
        // idk why but this fixes the cognitohazard bug
        // https://forum.unity.com/threads/nullreferenceexception-serializedobject-of-serializedproperty-has-been-disposed.1443694/#post-9673649
        this.serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}
#endif
