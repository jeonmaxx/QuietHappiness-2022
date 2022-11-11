using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacedListener : MonoBehaviour
{
    [HideInInspector] public Item ItemToPlace;

    private bool _hasListened;

    void Update()
    {
        if (_hasListened)
        {
            return;
        }else if (ModuleManager.GetModule<SaveGameManager>().GetCompletionInfo(CompletionIDs.TUTORIALITEMPLACED))
        {
            _hasListened = true;
        }

        if(ItemToPlace != null && ItemToPlace.Inventory == null)
        {
            ModuleManager.GetModule<SaveGameManager>().SetCompletionInfo(CompletionIDs.TUTORIALITEMPLACED, true);
        }
    }
}
