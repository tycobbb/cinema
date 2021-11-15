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

    // -- tuning --
    [Header("tuning")]
    [Tooltip("the speed of the look in degrees / s")]
    [SerializeField] float m_LookSpeed = 0.1f;

    [Tooltip("the max look in degrees")]
    [SerializeField] float m_LookMax = 3.0f;

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

    [Tooltip("the ambient music")]
    [SerializeField] Musicker m_Ambient;

    [Tooltip("the camera")]
    [SerializeField] Transform m_Camera;

    [Tooltip("the input system input")]
    [SerializeField] PlayerInput m_Input;

    // -- props --
    /// the current shot name
    ShotName m_ShotName;

    /// the current shot
    Transform m_Shot;

    /// the amount the look is rotated
    Vector2 m_LookRotation;

    /// the wind loop
    Loop m_Wind;

    /// the director's actions
    DirectorActions m_Actions;

    // -- lifecycle --
    void Awake() {
        // set props
        m_Actions = new DirectorActions(m_Input);

        // set audio props
        m_Wind = new Loop(10.0f, 2.0f, Tone.I);
    }

    void Start() {
        // show initial shot
        Show(ShotName.House, 0);

        // play audio
        m_Ambient.PlayLoop(m_Wind);
    }

    void Update() {
        ReadLook();

        if (m_Actions.IsCutJustPressed) {
            Cut();
        }
    }

    void FixedUpdate() {
        Look();
    }

    // -- commands --
    /// move the camera to the shot
    void Show(ShotName name, int i) {
        // store scene
        m_ShotName = name;
        m_Shot = name == ShotName.House ? m_HouseShots[i] : m_MonologueShots[i];

        // jump to the correct shot
        m_Camera.position = m_Shot.position;
        m_Camera.rotation = m_Shot.rotation;

        // reset look
        m_LookRotation = Vector2.zero;
    }

    /// read look input
    void ReadLook() {
        var look = m_Actions.Look * m_LookSpeed;

        var r = m_LookRotation;
        r.x = Mathf.Clamp(r.x + look.x, -m_LookMax, m_LookMax);
        r.y = Mathf.Clamp(r.y + look.y, -m_LookMax, m_LookMax);

        m_LookRotation = r;
    }

    /// look around the view
    void Look() {
        var look = m_LookRotation;
		var rotX = Quaternion.AngleAxis(look.x, Vector3.up);
		var rotY = Quaternion.AngleAxis(look.y, Vector3.left);
		m_Camera.localRotation = m_Shot.rotation * rotX * rotY;
    }

    /// cut to the other shot
    void Cut() {
        // get the scene to cut to
        var next = m_ShotName switch {
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
