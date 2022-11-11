using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class GiveItemOnLevelCompletion : MonoBehaviour
{
    [DropDown(nameof(IDs))] [SerializeField] public int SelectedIDOnCompletion;
    [DropDown(nameof(IDs))] [SerializeField] public int SelectedIDOnItemGiven;
    [HideInInspector] public List<string> IDs;
    [SerializeField] private BoundInventory _inventory;
    [SerializeField] private List<Item> _itemsToAdd;


    private void Update()
    {
        if (ModuleManager.GetModule<SaveGameManager>().GetCompletionInfo(IDs[SelectedIDOnCompletion]) && !ModuleManager.GetModule<SaveGameManager>().GetCompletionInfo(IDs[SelectedIDOnItemGiven]))
        {
            ModuleManager.GetModule<SaveGameManager>().SetCompletionInfo(IDs[SelectedIDOnItemGiven], true);
            foreach (Item item in _itemsToAdd)
            {
                item.OnValidate();
                GameObject obj = Instantiate(item.gameObject);
                _inventory.AddItem(obj.GetComponent<Item>(), 1);
            }
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
