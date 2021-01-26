﻿using PCAD.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace PCAD.UI
{
    /// <summary>
    /// A <see cref="MaskableGraphic"/> for drawing grid lines in the ui.
    /// </summary>
    public class GridLineUI : MaskableGraphic
    {
        public void UpdateUI(Vector3 originWorld, Vector3 directionWorld,
            CoordinateUIStyle.GridLineStyle lambdaStyleGridLineStyle, bool isVisible)
        {
            _originScreen = WorldScreenTransformationHelper.WorldToScreenPoint(originWorld);
            _directionScreen = WorldScreenTransformationHelper.WorldToScreenPoint(directionWorld);
            _style = lambdaStyleGridLineStyle;
            _isVisible = isVisible;
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            // quick fix: assume that rectangle is projected on the xz plane
            vh.Clear();
            if (_isVisible)
                UIMeshGenerationHelper.AddScreenSpanningLine(vh, _originScreen, _directionScreen, _style.Width,
                    _style.Color.Value);
        }

        public float GetScreenDistanceToLine(Vector2 screenPosition)
        {
            //https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line
            var o = _originScreen;
            var p = screenPosition;
            var v = _directionScreen.normalized;
            var normalToPoint = (o - p) - Vector2.Dot((o - p), v) * v;


            return normalToPoint.magnitude;
        }

        private CoordinateUIStyle.GridLineStyle _style;
        private bool _isVisible;
        private SketchStyle.State _state;
        private Vector2 _originScreen;
        private Vector2 _directionScreen;
    }
}