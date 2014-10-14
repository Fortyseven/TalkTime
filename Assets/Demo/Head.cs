using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour
{
    public AudioClip[] Clips;

    private TalkTime _talktime;

    void Start()
    {
        _talktime = GetComponent<TalkTime>();
    }

    public void OnClip(int id)
    {
        _talktime.Play( Clips[ id ] );
    }
}
