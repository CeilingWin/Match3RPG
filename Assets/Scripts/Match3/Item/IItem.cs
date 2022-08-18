using Cysharp.Threading.Tasks;
using UnityEngine;

namespace core
{
    public interface IItem
    {
        public void SetContentId(int id);

        public int GetContentId();

        public void SetPosition(Vector3 pos);
        public UniTask Appear(float delayTime = 0);

        public UniTask Disappear(float delayTime = 0);
    }
}