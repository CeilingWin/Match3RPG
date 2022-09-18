using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Match3;
using Rpg.Ability;
using Rpg.Effects;
using Unity.VisualScripting;
using UnityEngine;

namespace Rpg.Units
{
    public abstract class Unit : MonoBehaviour
    {
        public Vector3 defaultDirection;
        public float delayAttack;
        private GridPosition gridPosition;
        private List<Effect> effects;

        protected virtual void Start()
        {
            effects = new List<Effect>();
            this.AddComponent<Stat>();
            this.AddComponent<Move>();
        }

        public abstract bool IsDied();

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetGridPosition(GridPosition gridPosition)
        {
            this.gridPosition = gridPosition;
            transform.position = Game.instance.Match3Module.IndexToWorldPosition(gridPosition);
        }

        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }

        public UniTask UpdateUnit()
        {
            effects.ForEach(effect => effect.Update());
            effects = effects.Where(effect => effect.isActive).ToList();
            return UniTask.CompletedTask;
        }

        public void TakeEffect(Effect effect)
        {
            effect.SetTarget(this);
            effect.Perform();
            effects.Add(effect);
        }

        public abstract UniTask Attack();

        public abstract UniTask Die();
        
        public abstract UniTask TakeDamage(int damage);

        public abstract void SortUnits(List<Unit> units);
    }
}