using UnityEngine;
using UnityEngine.InputSystem;

/// a wrapper for the director's input asset
public class DirectorActions{
    // -- props --
    /// the cut action
    InputAction m_Cut;

    // -- lifetime --
    public DirectorActions(InputActionAsset asset) {
        var actions = asset.FindActionMap("Player");
        m_Cut = actions.FindAction("Cut");
    }

    // -- queries --
    public bool IsCutJustPressed {
        get => m_Cut.WasPressedThisFrame();
    }
}
