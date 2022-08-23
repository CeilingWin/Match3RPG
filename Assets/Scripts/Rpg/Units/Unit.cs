using System;
using Match3;
using Rpg.Ability;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rpg.Units
{
    public class Unit : MonoBehaviour
    {
        private Animator animator;
        private GridPosition gridPosition;

        private void Awake()
        {
        }

        public void Start()
        {
            animator = GetComponent<Animator>();
            var t = this.AddComponent<Move>();
            t.Test();
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
    }
}