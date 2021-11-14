using System.Collections;
using TMPro;
using UnityEngine;

/// the video subtitles
public class Subtitles: MonoBehaviour {
    // -- tuning --
    [Header("tuning")]
    [Tooltip("the words / sec reading speed")]
    [SerializeField] float m_Wps = 5;

    [Tooltip("the delay between lines")]
    [SerializeField] AnimationCurve m_Delay = CurveExt.One();

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the subtitle text")]
    [SerializeField] TMP_Text m_Subtitle;

    [Tooltip("file")]
    [SerializeField] TextAsset m_ScriptFile;

    // -- props --
    /// the script lines
    string[] m_Lines;

    // -- lifecycle --
    void Awake() {
        /// set props
        m_Lines = DecodeLines();
    }

    // -- commands --
    /// start autoplaying subtitles
    public void Play() {
        StartCoroutine(PlayAsync());
    }

    /// start autoplaying subtitles
    IEnumerator PlayAsync() {
        var i = 0;

        while (true) {
            var line = m_Lines[i];
            Debug.Log($"play {line}");
            m_Subtitle.text = m_Lines[i];

            // get reading time
            var words = line.Trim().Split(' ');
            var duration = words.Length / m_Wps;

            // add delay
            duration += m_Delay.Sample();

            // advance line
            i = (i + 1) % m_Lines.Length;

            // wait until next line
            yield return new WaitForSeconds(duration);
        }
    }

    // -- queries --
    /// decode the script lines
    string[] DecodeLines() {
        var lines = m_ScriptFile.text.Split('\n');

        for (var i = 0; i < lines.Length; i++) {
            lines[i] = lines[i].Trim();
        }

        return lines;
    }
}
