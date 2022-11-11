using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialog : MonoBehaviour
{
    void Start()
    {
        if (!ModuleManager.GetModule<SaveGameManager>().GetCompletionInfo(CompletionIDs.TUTORIALGIVEITEMS))
        {
            ModuleManager.GetModule<DialogManager>().StartDialog(CompletionIDs.TUTORIALGIVEITEMS);
        }
    }
}
