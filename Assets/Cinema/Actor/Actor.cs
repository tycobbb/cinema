using UnityEngine;

/// the lead actor
public class Actor: MonoBehaviour {
    // -- statics -
    /// property id for the mouth open pct
    int s_OpenPctId = -1;

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the model's mesh renderer")]
    [SerializeField] MeshRenderer m_Mesh;

    [Tooltip("the subtitles")]
    [SerializeField] Subtitles m_Subtitles;

    // -- props --
    /// the mesh materials
    Material m_Material;

    /// if the actor is monologuing
    bool m_IsPlaying;

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
        if (m_IsPlaying) {
            m_Material.SetFloat(s_OpenPctId, Mathf.PingPong(Time.time * 4.0f, 1.0f));
        }
    }

    // -- commands --
    /// start the actor's monologue
    public void Play() {
        if (m_IsPlaying) {
            return;
        }

        m_IsPlaying = true;
        m_Subtitles.Play();
    }
}
