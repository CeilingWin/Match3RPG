﻿using System;
using System.Collections.Generic;
using Match3;
using UnityEditor.PackageManager;
using UnityEngine;
using Utils;

namespace Rpg.Ability.PathFinding
{
    public class Node
    {
        public GridPosition pos;
        public int cost;
        public GridPosition prevPos = null;

        public float GetPriority()
        {
            return 1f / cost;
        }
    }

    public class PathFinding : MonoBehaviour
    {
        private Func<GridPosition, bool> isTarget;
        private Func<GridPosition, GridPosition, int> getCost = GridPosition.SquareDistance;
        private Func<GridPosition, bool> canMoveTo;

        private static readonly List<GridPosition> directions = new List<GridPosition>()
        {
            GridPosition.Down,
            GridPosition.Left,
            GridPosition.Right,
            GridPosition.Up
        };

        public List<GridPosition> FindPath(GridPosition startPosition)
        {
            PriorityQueue<Node> opens = new PriorityQueue<Node>();
            Dictionary<GridPosition, Node> closes = new Dictionary<GridPosition, Node>();
            var startNode = new Node() {pos = startPosition, cost = 0, prevPos = null};
            opens.Queue(startNode, startNode.GetPriority());
            while (!opens.IsEmpty())
            {
                var node = opens.Dequeue();
                closes.Add(node.pos, node);
                if (isTarget(node.pos)) return GenPath(closes, node.pos);
                foreach (var direction in directions)
                {
                    var nextPos = node.pos + direction;
                    if (!canMoveTo(nextPos)) continue;
                    if (closes.ContainsKey(nextPos)) continue;
                    var currentCost = node.cost + getCost(node.pos, nextPos);
                    var nodeInOpen = opens.Find(nodeData => nodeData.pos.Equals(nextPos));
                    if (nodeInOpen == null)
                    {
                        var newNode = new Node()
                        {
                            pos = nextPos, cost = currentCost, prevPos = node.pos
                        };
                        opens.Queue(newNode, newNode.GetPriority());
                    }
                    else
                    {
                        if (currentCost < nodeInOpen.cost)
                        {
                            nodeInOpen.prevPos = node.pos;
                            nodeInOpen.cost = currentCost;
                        }
                    }
                }
            }
            return null;
        }

        private List<GridPosition> GenPath(Dictionary<GridPosition, Node> closes, GridPosition lastPos)
        {
            List<GridPosition> path = new List<GridPosition>();
            var currentNode = closes[lastPos];
            while (currentNode.prevPos != null)
            {
                path.Insert(0, currentNode.pos);
                currentNode = closes[currentNode.prevPos];
            }
            return path;
        }
    }
}