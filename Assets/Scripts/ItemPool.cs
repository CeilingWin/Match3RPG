using System.Collections.Generic;
using core;
using UnityEngine;
using Object = UnityEngine.Object;

public class ItemPool
{
    private readonly GameObject itemPrefab;

    private readonly Queue<IItem> items;

    private static ItemPool ins;

    public static ItemPool GetIns()
    {
        if (ins == null)
        {
            ins = new ItemPool();
        }

        return ins;
    }

    private ItemPool()
    {
        itemPrefab = Resources.Load("Prefabs/Item") as GameObject;
        items = new Queue<IItem>();
    }

    public IItem GetItem()
    {
        var item = items.Count == 0 ? Object.Instantiate(itemPrefab).GetComponent<IItem>() : items.Dequeue();
        item.Transform.gameObject.SetActive(true);
        return item;
    }

    public void ReleaseItem(IItem item)
    {
        item.Transform.gameObject.SetActive(false);
        items.Enqueue(item);
    }
}
