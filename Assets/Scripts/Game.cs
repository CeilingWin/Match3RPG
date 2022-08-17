using System.Threading;
using Cysharp.Threading.Tasks;
using InputControl;
using Match3;
using Rpg;
using Unit;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;

    public float ItemSize;
    [FormerlySerializedAs("inputHandler")] [SerializeField]
    private InputHandler InputHandler;

    private ISlot[,] slots;

    private Match3Module match3Module;
    private RpgModule rpgModule;

    // Start is called before the first frame update
    void Start()
    {
        match3Module = new Match3Module(0, ItemPrefab);
        rpgModule = new RpgModule();
        Init(this.GetCancellationTokenOnDestroy());
    }

    private async UniTask Init(CancellationToken cancellationToken)
    {
        await match3Module.Init(cancellationToken);
    }

    // private void FillItem()
    // {
    //     for (var row = 0; row < NumRow; row++)
    //     {
    //         for (var column = 0; column < NumColumn; column++)
    //         {
    //             var slot = slots[row, column];
    //             if (slot.GetState().CanContainItem())
    //             {
    //                 var item = CreateNewItem(row, column);
    //                 item.SetContentId(Random.Range(0, 4));
    //                 slot.SetItem(item);
    //             }
    //         }
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
    }
}