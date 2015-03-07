using UnityEngine;

public class Head : MonoBehaviour
{
    public AudioClip[] Clips;

    private TalkTime _talktime;

    public void Start()
    {
        _talktime = GetComponent<TalkTime>();
    }

    public void OnClip( int id )
    {
        _talktime.Play( Clips[ id ] );
    }
}
