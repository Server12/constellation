using System;
using System.Collections.Generic;
using Constellation.Module_1.Components;
using Constellation.Module_1.Data;
using UnityEngine;

namespace Constellation.Module_1.Logic
{
    [RequireComponent(typeof(LineRenderer))]
    public class StarLineView : BaseConstellationView
    {
        [SerializeField] private LineRenderer _lineRenderer;
        
        public int Id { get; private set; }


        public void SetId(StarsPairData pairData)
        {
            gameObject.name = $"Line-from:{pairData.From}_to:{pairData.To}";
            Id = pairData.Id;
        }
        
        public void SetPositions(Vector3 from, Vector3 to)
        {
            _lineRenderer.SetPosition(0, from);
            _lineRenderer.SetPosition(1, to);
        }

        public float Width
        {
            get => _lineRenderer.startWidth;
            set => _lineRenderer.startWidth = _lineRenderer.endWidth = value;
        }
        
    }
}