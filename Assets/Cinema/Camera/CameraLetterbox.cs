using UnityEngine;

[ExecuteAlways]
public class CameraLetterbox: MonoBehaviour {
    // -- config --
    [Header("config")]
    [Tooltip("the aspect ratio")]
    [SerializeField] Vector2 m_AspectRatio;

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the camera")]
    [SerializeField] Camera m_Camera;

    // -- lifecycle --
    void Update() {
        // get screen params
        var sw = Screen.width;
        var sh = Screen.height;
        var sa = sw / sh;

        // get camera params
        var ca = m_AspectRatio.x / m_AspectRatio.y;
        var cr = Rect.zero;
        cr.width = sw;
        cr.height = sh;

        // if the camera is wider than the screen, add lettterbox
        if (ca > sa) {
            var h = sw / ca;
            cr.yMin = (sh - h) / 2.0f;
            cr.height = h;
        }
        // otherwise, add pillpox
        else {
            var w = sh * ca;
            cr.xMin = (sw - w) / 2.0f;
            cr.width = w;
        }

        // update state
        m_Camera.pixelRect = cr;
    }
}
