using System.Collections;
using System.Collections.Generic;
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

    // -- config --
    [Header("config")]
    [Tooltip("the movie script")]
    [SerializeField] string[] m_Lines;

    // -- nodes --
    [Header("nodes")]
    [Tooltip("the subtitle text")]
    [SerializeField] TMP_Text m_Subtitle;

    // -- lifecycle --
    void Start() {
        PlaySubtitles();
    }

    // -- commands --
    /// start autoplaying subtitles
    void PlaySubtitles() {
        StartCoroutine(PlaySubtitlesAsync());
    }

    /// start autoplaying subtitles
    IEnumerator PlaySubtitlesAsync() {
        var i = 0;

        while (true) {
            var line = m_Lines[i];
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
}
