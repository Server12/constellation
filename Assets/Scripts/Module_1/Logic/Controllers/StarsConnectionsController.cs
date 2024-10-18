using System;
using System.Collections.Generic;
using System.Linq;
using Constellation.Module_1.Data;
using NaughtyAttributes;
using UnityEngine;

namespace Constellation.Module_1.Logic.Controllers
{
    public class StarsConnectionsController : MonoBehaviour
    {
        [SerializeField] private Transform _linesHolder;
        [SerializeField] private StarLineView _linePrefab;

        [SerializeField] private float _lineGap = 0.1f;

        [Range(0.001f, 0.03f)] [SerializeField]
        private float _lineWidth = 0.01f;

        private readonly Stack<StarLineView> _linesPool = new Stack<StarLineView>();

        private readonly List<StarLineView> _activeLines =
            new List<StarLineView>();

        private readonly Dictionary<int, StarLineView>
            _createdLines = new Dictionary<int, StarLineView>();

        private readonly Dictionary<int, StarNode> _starsConnectionGraph = new Dictionary<int, StarNode>();

        private readonly List<LinesGroupAnimation> _linesToAnimate = new List<LinesGroupAnimation>();
        
        [ReadOnly] [SerializeField] private float _currentProgress = 0f;
        [ReadOnly] [SerializeField] private int _animationOrderIndex = 0;

        private void SetLinesToPool()
        {
            foreach (var lineRenderer in _activeLines)
            {
                lineRenderer.Alpha = 0f;
                lineRenderer.gameObject.SetActive(false);
                _linesPool.Push(lineRenderer);
            }

            _activeLines.Clear();
        }

        private StarLineView GetOrCreateLine()
        {
            if (_linesPool.Count > 0)
            {
                return _linesPool.Pop();
            }

            return Instantiate(_linePrefab, _linesHolder == null ? transform : _linesHolder);
        }


        private void BuildStarsConnectionGraph(List<Star> constellationStars, ConstellationData data)
        {
            _starsConnectionGraph.Clear();

            foreach (var constellationStar in constellationStars)
            {
                _starsConnectionGraph[constellationStar.Id] = new StarNode(constellationStar);
            }

            foreach (var starsPairData in data.Pairs)
            {
                var starFrom = _starsConnectionGraph[starsPairData.From];
                var starTo = _starsConnectionGraph[starsPairData.To];

                starFrom.Neighbours.Add(starTo);
                starTo.Neighbours.Add(starFrom);
            }
        }


        private int GetConstellationCenterStar(Dictionary<int, StarNode> graph)
        {
            var centerStarId = -1;
            var maxNeighbours = -1;

            foreach (var (id, node) in graph)
            {
                var count = node.Neighbours.Count;
                if (count > maxNeighbours)
                {
                    maxNeighbours = count;
                    centerStarId = id;
                }
            }


            return centerStarId;
        }

        public void Build(ConstellationData data, List<Star> constellationStars, float duration)
        {
            _createdLines.Clear();
            _animationOrderIndex = 0;
            _linesToAnimate.Clear();

            BuildStarsConnectionGraph(constellationStars, data);

            CreateLines(data.Pairs);

            BuildAnimationLines();
        }


        private void BuildAnimationLines()
        {
            Queue<StarNode> queue = new Queue<StarNode>();
            HashSet<int> visited = new HashSet<int>();

            var centralStar = GetConstellationCenterStar(_starsConnectionGraph);
            queue.Enqueue(_starsConnectionGraph[centralStar]);

            visited.Add(centralStar);

            while (queue.Count > 0)
            {
                var countInGroup = queue.Count;

                var lineGroupAnimation = new LinesGroupAnimation();

                for (int i = 0; i < countInGroup; i++)
                {
                    var currentNode = queue.Dequeue();

                    foreach (var neighbor in currentNode.Neighbours)
                    {
                        var key = StarsPairData.GetId(currentNode.StarView.Id, neighbor.StarView.Id);

                        if (_createdLines.TryGetValue(key, out var lineView))
                        {
                            lineGroupAnimation.AddLine(lineView);
                            _createdLines.Remove(key);
                        }

                        if (!visited.Contains(neighbor.StarView.Id))
                        {
                            visited.Add(neighbor.StarView.Id);
                            queue.Enqueue(neighbor);
                        }
                    }
                }

                if (lineGroupAnimation.Count > 0)
                    _linesToAnimate.Add(lineGroupAnimation);
            }
        }

        private void CreateLines(StarsPairData[] pairs)
        {
            SetLinesToPool();

            foreach (var starsPairData in pairs)
            {
                var fromStar = _starsConnectionGraph[starsPairData.From].StarView.transform.position;
                var toStar = _starsConnectionGraph[starsPairData.To].StarView.transform.position;

                Vector3 dir = (toStar - fromStar).normalized;

                var offsetStart = fromStar + dir * _lineGap;
                var offsetEnd = toStar - dir * _lineGap;

                var line = GetOrCreateLine();
                line.SetPositions(offsetStart, offsetEnd);
                line.Width = _lineWidth;
                line.Alpha = 0f;

                line.SetId(starsPairData);

                line.gameObject.SetActive(true);
                _createdLines.Add(line.Id, line);

                _activeLines.Add(line);
            }
        }
        

        public void UpdateAnimation(float progress, float duration)
        {
            _currentProgress = progress;
            
            int groupCount = Mathf.Max(1, _linesToAnimate.Count);
            float groupDuration = duration / groupCount;

            int newGroupIndex = Mathf.FloorToInt(progress * groupCount);
            newGroupIndex = Mathf.Clamp(newGroupIndex, 0, groupCount - 1);

            if (newGroupIndex != _animationOrderIndex)
            {
                _animationOrderIndex = newGroupIndex;
            }

            float groupProgress = ((progress * groupCount) - _animationOrderIndex) / groupDuration;
            groupProgress = Mathf.Clamp01(groupProgress);

            _linesToAnimate[_animationOrderIndex].UpdateAlpha(groupProgress);
        }

        public void UpdateLineWidth()
        {
            foreach (var activeLine in _activeLines)
            {
                activeLine.Width = _lineWidth;
            }
        }

        public class LinesGroupAnimation
        {
            private readonly List<StarLineView> _lines = new List<StarLineView>();

            public int Count => _lines.Count;

            public void AddLine(StarLineView lineView)
            {
                _lines.Add(lineView);
            }

            public void UpdateAlpha(float progress)
            {
                foreach (var lineView in _lines)
                {
                    lineView.Alpha = Mathf.LerpUnclamped(0f, 1f, progress);
                }
            }
        }
    }
}