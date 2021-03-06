﻿using UnityEngine;

[RequireComponent( typeof( AudioSource ) )]
public class TalkTime : MonoBehaviour
{
    public AudioClip Clip;
    public int WindowSize = 1024;
    public float HeadBopMagnitude = 1.0f;
    // The frame index is used to represent loudness, with 0 being
    // the quietest (silent), and SpriteFrames.Length-1 being the loudest
    public GameObject[] SpriteFrames;


    private float _time_started;
    private bool _is_playing;

    private AudioSource _audiosource;
    private float[] _samples;
    private float _max_amplitude;

    private float _org_y;

    public void Start()
    {
        _audiosource = GetComponent<AudioSource>();
        _is_playing = false;
        _max_amplitude = 0;

        if ( SpriteFrames.Length == 0 ) {
            throw new UnityException( "No sprite frames defined" );
        }
        for ( int i = 0; i < SpriteFrames.Length; i++ ) {
            if ( SpriteFrames[ i ] == null ) {
                throw new UnityException( "Sprite frame " + i + " was null" );
            }
        }

        _org_y = transform.position.y;
    }

    /// <summary>
    /// Plays the attached AudioClip.
    /// </summary>
    public void Play()
    {
        StartPlayback();
    }

    /// <summary>
    /// Plays a specific AudioClip
    /// </summary>
    /// <param name="clip">A user provided AudioClip.</param>
    public void Play( AudioClip clip )
    {
        Clip = clip;
        Play();
    }

    public void Update()
    {
        if ( !_is_playing )
            return;

        float duration = Time.time - _time_started;
        int sample_position = Mathf.RoundToInt( Clip.samples * ( duration / Clip.length ) );
        ;
        float avg = GetAverageFromWindow( sample_position );

        if ( duration >= Clip.length ) {
            _is_playing = false;
        }

        SetFrame( avg );
        DoHeadBop( avg );
    }

    private void DoHeadBop( float avg )
    {
        Vector3 pos = transform.position;
        pos.y = _org_y + ( avg / 10 * HeadBopMagnitude );
        transform.position = pos;
    }

    public void SetFrame( float avg )
    {
        int frame = Mathf.RoundToInt( avg * ( SpriteFrames.Length - 1 ) );

        for ( int i = 0; i < SpriteFrames.Length; i++ ) {
            SpriteFrames[i].SetActive(i == frame);
        }
    }

    public float GetAverageFromWindow( int sample_pos )
    {
        float sum = 0;
        int c = 0;

        float sz = sample_pos + WindowSize;
        if ( sz > _samples.Length ) {
            sz = _samples.Length - sample_pos;
        }

        for ( int i = sample_pos; i < sz; i++ ) {
            sum += Mathf.Clamp( Mathf.Abs( _samples[ i ] ) * 10, 0, 1.0f ); // amp
            c++;
        }
        if ( c == 0 )
            return 0;
        return ( sum / c ) / _max_amplitude;
    }

    public void StartPlayback()
    {
        _audiosource.clip = Clip;
        _samples = new float[ Clip.samples * Clip.channels ];
        Clip.GetData( _samples, 0 );

        // Find loudest sample
        for ( int i = 0; i < _samples.Length; i++ ) {
            float samp = Mathf.Abs( _samples[ i ] );
            if ( samp > _max_amplitude )
                _max_amplitude = samp;
        }

        _audiosource.Play();
        _time_started = Time.time;
        _is_playing = true;
    }
}
