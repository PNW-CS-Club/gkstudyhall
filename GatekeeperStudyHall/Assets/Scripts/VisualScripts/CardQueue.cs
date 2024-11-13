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
    [SerializeField] Vector2 healthOffset;

    [SerializeField] PlayerListSO playerListObject;
    List<PlayerSO> playerList; // refers to list in playerListObject
    List<GameObject> cardObjectList; // instances of cardPrefab
    List<GameObject> healthBarList;

    float expandedHeight; // the height of an expanded card
    float collapsedHeight; // the height of a collapsed card
    Vector3 startingOffset; // the offset of the first card in the queue

    int expandedIndex = 0; // determines which card is expanded (RepositionCards relies on this value)

    public GameObject healthBarPrefab;


    void Start()
    {
        playerList = playerListObject.list;
        cardObjectList = new();
        healthBarList = new();

        Vector2 expandedSize = cardPrefab.GetComponent<RectTransform>().sizeDelta;

        // precalculate some values so we can use them in RepositionCards
        expandedHeight = expandedSize.y;
        collapsedHeight = expandedHeight - CardDisplay.COLLAPSE_HEIGHT_DIFF;
        startingOffset = new Vector3(expandedSize.x / 2f, -expandedSize.y / 2f, 0f);

        // create all of the cards and health bars we will start with
        for (int i = 0; i < playerList.Count; i++) 
        {
            GameObject newCard = Instantiate(cardPrefab, transform);
            cardObjectList.Add(newCard);

            GameObject newHealthBar = Instantiate(healthBarPrefab, newCard.transform);
            healthBarList.Add(newHealthBar);
        }

        RepositionCards();
    }

    void Update() 
    {
        // i didn't put this in RepositionCards because it seems like
        // getting the card height isn't accurate right after you set it
        for (int i = 0; i < playerList.Count; i++) 
        {
            RectTransform healthBarRect = healthBarList[i].GetComponent<RectTransform>();
            RectTransform cardRect = cardObjectList[i].GetComponent<RectTransform>();
            healthBarRect.anchoredPosition = healthOffset + new Vector2(0, cardRect.sizeDelta.y / 2.0f);
        }
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


    public void RepositionCards() 
    {
        // the real meat & potatoes of the card queue
        // NOTE: relies on expandedIndex to determine which card should be expanded

        Vector3 offset = startingOffset;

        for (int i = 0; i < playerList.Count; i++) 
        {
            Transform cardTransform = cardObjectList[i].transform;
            CardDisplay cardDisplay = cardTransform.GetComponent<CardDisplay>();
            cardTransform.localPosition = offset;

            cardDisplay.ChangeCardData(playerList[i].card);
            cardDisplay.player = playerList[i];

            HealthDisplay healthDisplay = healthBarList[i].GetComponent<HealthDisplay>();
            healthDisplay.player = playerList[i];

            if (i == expandedIndex) 
            {
                cardDisplay.SetExpanded(true);
                offset.y -= margin + expandedHeight;
            }
            else 
            {
                cardDisplay.SetExpanded(false);
                cardTransform.localPosition += CardDisplay.COLLAPSE_HEIGHT_DIFF / 2f * Vector3.up;
                offset.y -= margin + collapsedHeight;
            }
        }
    }


#if UNITY_EDITOR
    void OnDrawGizmos() {
        float gizmoRadius = 0.25f;

        // this shows the position of the object as green sphere gizmo in the scene
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
        Handles.Label(transform.position + gizmoRadius * Vector3.right, "Card Queue");
    }
#endif
}



#if UNITY_EDITOR
[CustomEditor(typeof(CardQueue))]
public class CardQueueEditor : Editor
{
    // these are for keeping track of state in the immediate mode gui
    // (i.e. our custom editor fields don't store variables so we have to do it ourselves)
    
    int index = 0;

    public override void OnInspectorGUI() {
        // idk why but this fixes the playerList cognitohazard bug
        // https://forum.unity.com/threads/nullreferenceexception-serializedobject-of-serializedproperty-has-been-disposed.1443694/#post-9673649
        this.serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();

        CardQueue cq = (CardQueue) this.target;

        // some helpful controls that let us call CardQueue's methods in the editor while in play mode
        if (Application.isPlaying) 
        {
            EditorGUILayout.LabelField("(Do not manually change playerList in play mode)");
            
            // this section lets the user call ChangeExpandedPlayer
            EditorGUILayout.BeginHorizontal();

            index = EditorGUILayout.IntField(index);

            if (GUILayout.Button("Change Expanded Player")) 
            {
                cq.ChangeExpandedPlayer(index);
            }

            EditorGUILayout.EndHorizontal();
        }
        else 
        {
            EditorGUILayout.LabelField("(Custom controls appear here in play mode)");
        }
    }
}
#endif
