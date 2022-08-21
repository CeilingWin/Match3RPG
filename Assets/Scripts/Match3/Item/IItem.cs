using Cysharp.Threading.Tasks;
using UnityEngine;
using Material = Enum.Material;

namespace core
{
    public interface IItem
    {
        public Transform Transform { get; }
        public void SetMaterial(Material material);

        public Material GetMaterial();

        public void SetPosition(Vector3 pos);
        public Vector3 GetPosition();
        public UniTask Appear(float delayTime = 0);

        public UniTask Disappear(float delayTime = 0);
    }
}