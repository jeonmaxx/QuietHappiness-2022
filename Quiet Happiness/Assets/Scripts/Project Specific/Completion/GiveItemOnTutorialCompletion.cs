using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class GiveItemOnTutorialCompletion : MonoBehaviour
{
    [DropDown(nameof(IDs))] [SerializeField] public int SelectedID;
    [HideInInspector] public List<string> IDs;
    [SerializeField] private BoundInventory _inventory;
    [SerializeField] private List<Item> _itemsToAdd;

    // Start is called before the first frame update
    void Start()
    {
        ModuleManager.GetModule<SaveGameManager>().OnCompletionInfoChanged += EvaluateCompletion;
    }

    private void EvaluateCompletion(string ID, bool isDone)
    {
        if (ID.Equals(IDs[SelectedID]))
        {
            foreach (Item item in _itemsToAdd)
            {
                item.OnValidate();
                GameObject obj = Instantiate(item.gameObject);
                _inventory.AddItem(obj.GetComponent<Item>(), 1);

                if (ID.Equals(CompletionIDs.TUTORIALGIVEITEMS))
                {
                    GetComponent<ItemPlacedListener>().ItemToPlace = obj.GetComponent<Item>();
                }
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
