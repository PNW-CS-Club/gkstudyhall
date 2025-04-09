using System.Collections.Generic;
using UnityEngine;

public class TraitInfoBox : MonoBehaviour
{
    /*
    at start: show empty dice slot
        show "Rolling trait..."
      
    on 6: show the "6" dice sprite
        show "Lose turn!"
      
    on 5:
        show the "5" dice sprite
        show "Battle"
        show current char + "vs" + empty char slot
        
        on battle opponent selected:
            change empty slot to the chosen char
            
     */

    private int traitRoll = 0;
}
