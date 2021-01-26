using PCAD.Helper;
using PCAD.Model;
using UnityEngine;

namespace PCAD.UI
{
    /// <summary>
    /// The ui for a <see cref="Origin"/> coordinate.
    /// </summary>
    public class OriginUI : MonoBehaviour
    {
        [SerializeField] protected GridLineUI _gridLineUI = null;
        [SerializeField] protected CoordinateGizmoUI _coordinateGizmoUI = null;

        public void UpdateUI(Origin coordinate, CoordinateUI.LayoutInfo layoutInfo, Vector3 direction, float padding,
            float gap,
            CoordinateUIStyle.OriginUIStyle style,bool showGridLine)
        {
            var state = SketchStyle.State.Default;
            _coordinate = coordinate;
            var labelString = coordinate.Parameter.Value.ToString("F");
            gameObject.name = $"Origin:{labelString}";

            var offset = layoutInfo.OrthogonalDirection * (layoutInfo.OrthogonalAnchor + layoutInfo.Index * gap - padding);
            var coordinateUIPositionWorld = direction * coordinate.Value + offset;

            _gridLineUI.UpdateUI(coordinateUIPositionWorld, layoutInfo.OrthogonalDirection, style.GridLineStyle, showGridLine);
            _coordinateGizmoUI.UpdateUI(coordinateUIPositionWorld, direction, style.CoordinateGizmoStyle, style.Colors,
                state, CoordinateGizmoUI.Type.Mark);
        }

        public CoordinateManipulation.ScreenDistance GetScreenDistanceToCoordinate(Vector2 screenPos)
        {
            var distance = _gridLineUI.GetScreenDistanceToLine(screenPos);
            return new CoordinateManipulation.ScreenDistance()
                {Coordinate = _coordinate, ScreenDistanceToCoordinate = distance};
        }

        private Origin _coordinate;
    }
}