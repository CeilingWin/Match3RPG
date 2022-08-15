
using Unit;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;

    [SerializeField] private int NumColumn;
    [SerializeField] private int NumRow;

    public float ItemSize;

    private ISlot[,] slots;

    // Start is called before the first frame update
    void Start()
    {
        this.InitBoard();
        this.FillItem();
    }

    private void InitBoard()
    {
        slots = new ISlot[NumRow, NumColumn];
        for (var row = 0; row < NumRow; row++)
        {
            for (var column = 0; column < NumColumn; column++)
            {
                IStateSlot state = Random.Range(0f, 1f) > 0.8f ? new UnavailableSlotState() : new AvailableSlotState();
                slots[row, column] = new Slot(state);
            }
        }
    }

    private void FillItem()
    {
        for (var row = 0; row < NumRow; row++)
        {
            for (var column = 0; column < NumColumn; column++)
            {
                var slot = slots[row, column];
                if (slot.GetState().CanContainItem())
                {
                    var item = CreateNewItem(row, column);
                    slot.SetItem(item);
                }
            }
        }
    }

    private Vector3 IndexToWorldPosition(int row, int column)
    {
        float z = (-NumRow / 2 + row + 0.5f) * ItemSize;
        float x = (-NumColumn / 2 + column + 0.5f) * ItemSize;
        return new Vector3(x, 0, z);
    }

    private Item CreateNewItem(int row, int column)
    {
        var item = Instantiate(ItemPrefab);
        item.transform.position = IndexToWorldPosition(row, column);
        return item.GetComponent<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        // foreach (Touch touch in Input.touches)
        // {
        //     if (touch.phase == TouchPhase.Began)
        //     {
        //         var ray = Camera.main.ScreenPointToRay(touch.position);
        //         RaycastHit target;
        //         if (Physics.Raycast(ray, out target, float.PositiveInfinity, 1<<4))
        //         {
        //             Debug.Log(target.transform.tag);
        //             Debug.Log(target.point);
        //         }
        //     }
        // }
    }
}