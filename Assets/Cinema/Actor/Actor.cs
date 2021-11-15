using UnityEngine;

/// the lead actor
public class Actor: MonoBehaviour {
    // -- statics -
    /// property id for the mouth open pct
    int s_OpenPctId = -1;

    /// property id for the animator random value
    int s_AnimRandomId = -1;

    // -- tuning --
    [Header("tuning")]
    [Tooltip("the resistance to acting")]
    [SerializeField] int m_Stubborness;

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the model")]
    [SerializeField] Transform m_Model;

    [Tooltip("the model's mesh renderer")]
    [SerializeField] Renderer m_Mesh;

    [Tooltip("the animation controller")]
    [SerializeField] Animator m_Animator;

    [Tooltip("the vocal audio")]
    [SerializeField] AudioSource m_Vocals;

    [Tooltip("the subtitles")]
    [SerializeField] Subtitles m_Subtitles;

    // -- props --
    /// the actor's model pos
    Vector3 m_ModelPos;

    /// the mesh materials
    Material m_Material;

    // -- lifecycle --
    void Awake() {
        // set props
        m_ModelPos = m_Model.position;
        m_Material = m_Mesh.materials[0];

        // set statics
        if (s_OpenPctId == -1) {
            s_OpenPctId = Shader.PropertyToID("_OpenPct");
            s_AnimRandomId = Animator.StringToHash("Random");
        }
    }

    void Update() {
        if (!IsPlaying) {
            return;
        }

        // set mouth openness
        m_Material.SetFloat(s_OpenPctId, Mathf.PingPong(Time.time * 4.0f, 1.0f));

        // set animator props
        m_Animator.SetFloat(s_AnimRandomId, Random.value);
    }

    void FixedUpdate() {
        if (!IsPlaying) {
            return;
        }

        // stay in one place (animations move the actor)
        m_Model.position = m_ModelPos;
    }

    // -- commands --
    /// start the actor's monologue
    public void Play() {
        m_Stubborness -= 1;

        // see if we're ready to act
        if (m_Stubborness != 0) {
            return;
        }

        // start monologue
        m_Vocals.Play();
        m_Subtitles.Play();
        m_Animator.enabled = true;
    }

    // -- queries --
    /// if the actor is monologuing
    bool IsPlaying {
        get => m_Stubborness <= 0;
    }
}
