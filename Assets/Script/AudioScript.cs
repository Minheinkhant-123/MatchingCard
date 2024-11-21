using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioScript : MonoBehaviour
{
    public AudioSource audio;

    public void plays()
    {
        audio.Play();
        Invoke("SceneChange", 2);
    }
    public void SceneChange()
    {
        SceneManager.LoadScene("Level1");
    }
}
