using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public static class AnimatorUtils
    {
        public static async UniTask PlayAnimationSync(Animator animator, String animationName, String desAnimationName)
        {
            animator.Play(animationName);
            var desAction = Animator.StringToHash(desAnimationName);
            await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == desAction);
        }

        public static async UniTask PlayAnimationSync(Animator animator, int animationHash, int desAnimationHash)
        {
            animator.Play(animationHash);
            await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == desAnimationHash);
        }
    }
}