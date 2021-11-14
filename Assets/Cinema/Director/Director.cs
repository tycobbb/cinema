using UnityEngine;
using UnityEngine.InputSystem;

/// the movie director
public class Director: MonoBehaviour {
    // -- types --
    /// the name of the shot
    enum ShotName {
        House,
        Monologue
    }

    // -- config --
    [Header("config")]
    [Tooltip("the house shot")]
    [SerializeField] Transform[] m_HouseShots;

    [Tooltip("the monologue shots")]
    [SerializeField] Transform[] m_MonologueShots;

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the lead actor")]
    [SerializeField] Actor m_Actor;

    [Tooltip("the camera")]
    [SerializeField] Transform m_Camera;

    [Tooltip("the input system input")]
    [SerializeField] PlayerInput m_Input;

    // -- props --
    /// the current shot name
    ShotName m_Shot;

    /// the director's actions
    DirectorActions m_Actions;

    // -- lifecycle --
    void Awake() {
        // set props
        m_Actions = new DirectorActions(m_Input.actions);
    }

    void Start() {
        // show initial shot
        Show(ShotName.House, 0);
    }

    void Update() {
        if (m_Actions.IsCutJustPressed) {
            Cut();
        }
    }

    // -- commands --
    /// move the camera to the shot
    void Show(ShotName name, int i) {
        // store scene
        m_Shot = name;

        // move to the correct shot
        var shot = name == ShotName.House ? m_HouseShots[i] : m_MonologueShots[i];
        m_Camera.position = shot.position;
        m_Camera.rotation = shot.rotation;
    }

    /// cut to the other shot
    void Cut() {
        // get the scene to cut to
        var next = m_Shot switch {
            ShotName.House => ShotName.Monologue,
            _              => ShotName.House,
        };

        // show its first shot
        Show(next, 0);

        // start the actor's monologue
        if (next == ShotName.Monologue) {
            m_Actor.Play();
        }
    }
}
