using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class WaitShowDialog : MonoBehaviour
{
    [DropDown(nameof(IDs))] [SerializeField] public int SelectedID;
    [HideInInspector] public List<string> IDs;

    [SerializeField] private string _dialogID;

    private bool _waiting;

    // Start is called before the first frame update
    void Start()
    {
        ModuleManager.GetModule<SaveGameManager>().OnCompletionInfoChanged += EvaluateCompletion;
    }

    private void EvaluateCompletion(string ID, bool isDone)
    {
        if (ID.Equals(IDs[SelectedID]) && isDone)
        {
            _waiting = true;
        }
    }

    private void Update()
    {
        if (_waiting && !ModuleManager.GetModule<MenuManager>().InMenu && ModuleManager.GetModule<MenuManager>().IsEnabled)
        {
            _waiting = false;
            ModuleManager.GetModule<DialogManager>().StartDialog(_dialogID);
        }
    }

    private void OnValidate()
    {
        if (IDs == null)
        {
            IDs = new List<string>();
        }
        else IDs.Clear();
        foreach (FieldInfo field in typeof(CompletionIDs).GetFields())
        {
            string value = (string)field.GetValue(null);
            IDs.Add(value);
        }
    }
}
