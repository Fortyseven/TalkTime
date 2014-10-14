using UnityEngine;
using System.Collections;

[RequireComponent(typeof( AudioSource ))]
public class TalkTime : MonoBehaviour
{
    public AudioClip Clip;
    public int WINDOW_SIZE = 1024;
    public GameObject[] SpriteFrames;


    private float _clip_length;
    private float _time_started;
    private bool _is_playing;

    private AudioSource _audiosource;
    private float[] _samples;
    private float _max_amplitude;

    private float _org_y;

    void Start()
    {
        _audiosource = GetComponent<AudioSource>();
        _is_playing = false;
        _max_amplitude = 0;

        _org_y = transform.position.y;
    }

    public void Play()
    {
        InitClip();
    }

    public void Play( AudioClip clip )
    {
        this.Clip = clip;
        Play();
    }

    void Update()
    {
        if (!_is_playing) return;

        int sample_position = 0;
        float duration = Time.time - _time_started;
        sample_position = Mathf.RoundToInt(Clip.samples * (duration / Clip.length));

        float avg = GetAverageFromWindow(sample_position) ;

        if (duration >= Clip.length) {
            _is_playing = false;
        }

        int frame = Mathf.RoundToInt(avg / SpriteFrames.Length);
//        Debug.Log(frame + "| avg: " + avg.ToString("F2"));

        for(int i = 0; i < SpriteFrames.Length; i++) {
            SpriteFrames[i].SetActive(false);
            if (i == frame) {
                SpriteFrames[i].SetActive(true);
            }
        }


        //-----------
//        Vector3 pos = transform.position;
//        pos.y = _org_y + avg;
//        transform.position = pos;
        //-----------

    }

    float GetAverageFromWindow(int sample_pos)
    {
        float avg = 0;
        int c = 0;

        float sz = sample_pos + WINDOW_SIZE;
        if (sz > _samples.Length) {
            sz = _samples.Length - sample_pos;
        }

        for(int i = sample_pos; i < sz; i++) {

            //Debug.Log (i + "@"+(1.0f + _samples[i]));
            avg += (1.0f + _samples[i]);
            c++;
        }
        Debug.Log ("sz = "  +sz.ToString() + " : " + c);
        //return Mathf.Clamp((avg/sz) / (_max_amplitude), 0, 1.0f);
        return (avg / sz) / _max_amplitude;
    }

    void InitClip()
    {
        Debug.Log("Samples: " + Clip.samples);
        Debug.Log("Length: " + Clip.length);

        _audiosource.clip = this.Clip;
        _samples = new float[Clip.samples * Clip.channels];
        Clip.GetData(_samples,0);

        // Find loudest sample
        for(int i = 0; i < _samples.Length; i++) {
            float samp = (1.0f + _samples[i]);
            if (samp > _max_amplitude) _max_amplitude = samp;
        }

        Debug.Log("Loudest: " + _max_amplitude);

        _audiosource.Play();
        _time_started = Time.time;
        _is_playing = true;
    }
}
