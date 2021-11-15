using UnityEngine;
using UnityEngine.InputSystem;

/// a wrapper for the director's player input
public class DirectorActions{
    // -- props --
    /// the cut action
    InputAction m_Cut;

    /// the look action
    InputAction m_Look;

    // -- lifetime --
    public DirectorActions(PlayerInput input) {
        m_Cut = input.currentActionMap["Cut"];
        m_Look = input.currentActionMap["Look"];
        Debug.Log($"look {m_Look}");
    }

    // -- queries --
    // the look direction
    public Vector2 Look {
        get => m_Look.ReadValue<Vector2>();
    }

    /// if cut is pressed this frame
    public bool IsCutJustPressed {
        get => m_Cut.WasPressedThisFrame();
    }
}
