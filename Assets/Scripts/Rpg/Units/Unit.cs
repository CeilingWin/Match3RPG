using System;
using Cysharp.Threading.Tasks;
using Match3;
using Rpg.Ability;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rpg.Units
{
    public abstract class Unit : MonoBehaviour
    {
        private GridPosition gridPosition;

        public void Start()
        {
            this.AddComponent<Stat>();
            this.AddComponent<Move>();
        }

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

        public abstract UniTask Attack();
    }
}