﻿using System.Collections.Generic;
using PCAD.Helper;
using PCAD.Model;
using PCAD.UserInput;
using UnityEngine;

namespace PCAD.UI
{
    /// <summary>
    /// Root component for all uis of the coordinate layer.
    /// </summary>
    public class CoordinateSystemUI : MonoBehaviour, CoordinateManipulation.IScreenDistanceCalculatorProvider
    {
        Vec<CoordinateManipulation.IScreenDistanceCalculator> CoordinateManipulation.IScreenDistanceCalculatorProvider.
            GetProvidersForAxis() =>
            new Vec<CoordinateManipulation.IScreenDistanceCalculator>(a => _axisUIs[a]);

        [SerializeField] AxisUI _axisUIPrefab;
        [SerializeField] AnchorUI _anchorUIPrefab;

        private Vec<AxisUI> _axisUIs;
        private AnchorUI _anchorUI;
        private readonly Vec<Vector3> _embedding = new Vec<Vector3>(Vector3.right, Vector3.up, Vector3.forward);
        
        public void Initialize()
        {
            _axisUIs = new Vec<AxisUI>(a =>
            {
                var ui = Instantiate(_axisUIPrefab, transform);
                ui.Initialize(_embedding[a], $"{a} - AxisUI");
                
                //quick fix: disable y axis ui
                if(a==Vec.AxisID.Y)
                    ui.gameObject.SetActive(false);
                
                return ui;
            });

            _anchorUI = Instantiate(_anchorUIPrefab, transform);
        }

        public void UpdateUI(CoordinateSystem cs, CoordinateUIStyle coordinateUIStyle,
            NumpadInput.Model keyboardInput, Coordinate draggedCoordinate,
            (Coordinate coordinate, Vec.AxisID axis)? hoveredCoordinate)
        {
            foreach (var a in Vec.XYZ)
            {
                var referencedParameters = new List<Parameter>();
                foreach (var innerA in Vec.XYZ)
                {
                    if (keyboardInput.ParameterReferences[innerA] != null)
                        referencedParameters.Add(keyboardInput.ParameterReferences[innerA]);
                    if(cs.SnappedParameter[innerA]!=null)
                        referencedParameters.Add(cs.SnappedParameter[innerA]);
                }
      
                _axisUIs[a].UpdateCoordinateUIs(
                    cs.Axes[a],
                    _embedding[Vec.GetOrthogonalAxis(a)],
                    cs.Axes[Vec.GetOrthogonalAxis(a)].SmallestValue,
                    coordinateUIStyle,
                    referencedParameters,
                    keyboardInput.ActiveAxis == a,
                    draggedCoordinate, hoveredCoordinate);
            }

            _anchorUI.UpdateUI(cs.Anchor, coordinateUIStyle.Anchor);
        }
    }
}