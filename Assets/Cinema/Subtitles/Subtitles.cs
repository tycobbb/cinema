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

    [Tooltip("the delay for a comma")]
    [SerializeField] float m_Comma = 0.4f;

    [Tooltip("the delay for a period")]
    [SerializeField] float m_Period = 1.0f;

    [Tooltip("the delay for a question mark")]
    [SerializeField] float m_QuestionMark = 0.7f;

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
        // set props
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
            m_Subtitle.text = m_Lines[i];

            // get reading time
            var dur = 0.0f;

            // based on number of words
            dur += line.Trim().Split(' ').Length / m_Wps;

            // add punctuation delay
            for (var j = line.Length - 1; j >= 0; j--) {
                var c = line[j];

                var delay = line[j] switch {
                    ',' => m_Comma,
                    '.' => m_Period,
                    '?' => m_QuestionMark,
                    _   => -1.0f
                };

                if (delay == -1.0f) {
                    break;
                }

                dur += delay;
            }

            // add random delay
            dur += m_Delay.Sample();

            // advance line
            i = (i + 1) % m_Lines.Length;

            // wait until next line
            yield return new WaitForSeconds(dur);
        }
    }

    // -- queries --
    /// decode the script lines
    string[] DecodeLines() {
        var lines = m_ScriptFile.text.Split('\n');

        for (var i = 0; i < lines.Length; i++) {
            lines[i] = lines[i].Trim();
            Debug.Log($"line {lines[i]}");
        }

        return lines;
    }
}
