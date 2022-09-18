using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Events;


[RequireComponent(typeof(AudioSource))]

public class AudioHandler : MonoBehaviour
{
    public List<soundInfo> soundList = new List<soundInfo>();

    AudioSource audioSource;

    soundInfo curentSoundInfo;

    float audioLength;

    ContinuousLogger CL;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        CL = gameObject.GetComponent<ContinuousLogger>();
    }


    public void SetAudioToPlay(int ID)
    {   

        for (int i = 0; i < soundList.Count; i++)
        {

            if (soundList[i].ID == ID)
            {

                curentSoundInfo = soundList[i];
                Debug.Log("currebt sound info:" + curentSoundInfo);
                CL.songName = curentSoundInfo.name;

                StartCoroutine(playSequencely());

                return;

            }

        }

    }

    IEnumerator playSequencely()
    {


        yield return null;


        for (int cnt = 0; cnt < curentSoundInfo.clipsToPlay.Length; cnt++)
        {


            audioSource.clip = curentSoundInfo.clipsToPlay[cnt];

            audioSource.Play();


            while (audioSource.isPlaying)
            {


                yield return null;

            }


        }

        //Debug.Log ("Last Audio Is Playing");

        curentSoundInfo.onStop.Invoke();

    }

}


[System.Serializable]

public class soundInfo
{


    public string name;

    public int ID;

    [TextArea(2, 8)] public string About;

    public AudioClip[] clipsToPlay;

    //public float delayBetween;

    public UnityEvent onStop;

}