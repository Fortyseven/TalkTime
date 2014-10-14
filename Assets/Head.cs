using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour
{
    public AudioClip[] Clips;

    void Start()
    {
//        GetComponent<TalkTime>().Play();
    }

    void Update()
    {
    
    }

    public void OnClip1()
    {
        GetComponent<TalkTime>().Play(Clips[0]);
    }
    public void OnClip2()
    {
        GetComponent<TalkTime>().Play(Clips[1]);
    }
    public void OnClip3()
    {
        GetComponent<TalkTime>().Play(Clips[2]);
    }
    public void OnClip4()
    {
        GetComponent<TalkTime>().Play(Clips[3]);
    }
}
