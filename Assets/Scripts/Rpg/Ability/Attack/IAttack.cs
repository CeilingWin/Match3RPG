using Cysharp.Threading.Tasks;

namespace Rpg.Ability
{
    public interface IAttack
    {
        public UniTask Attack<T>() where T : Units.Unit;
    }
}