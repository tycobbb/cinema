using UnityEngine;

/// the lead actor
public class Actor: MonoBehaviour {
    // -- statics -
    /// property id for the mouth open pct
    int s_OpenPctId = -1;

    // -- tuning --
    [Header("tuning")]
    [Tooltip("the resistance to acting")]
    [SerializeField] int m_Stubborness;

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the model's mesh renderer")]
    [SerializeField] MeshRenderer m_Mesh;

    [Tooltip("the subtitles")]
    [SerializeField] Subtitles m_Subtitles;

    // -- props --
    /// the mesh materials
    Material m_Material;

    // -- lifecycle --
    void Awake() {
        if (s_OpenPctId == -1) {
            s_OpenPctId = Shader.PropertyToID("_OpenPct");
        }
    }

    void Start() {
        m_Material = m_Mesh.materials[0];
    }

    void Update() {
        // set mouth openness
        if (IsPlaying) {
            m_Material.SetFloat(s_OpenPctId, Mathf.PingPong(Time.time * 4.0f, 1.0f));
        }
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
        m_Subtitles.Play();
    }

    // -- queries --
    /// if the actor is monologuing
    bool IsPlaying {
        get => m_Stubborness <= 0;
    }
}
