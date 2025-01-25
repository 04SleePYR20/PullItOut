using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource _BGMSource;
    public AudioSource _SFXSource;

    public BGM_Data[] bGM_Datas;
    public List<SFX_Data> sFX_Datas;

    Dictionary<string, AudioClip> SFXDictionary;
    Dictionary<string, AudioClip> BGMDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Initialise dictionary
        SFXDictionary = new Dictionary<string, AudioClip>();
        BGMDictionary = new Dictionary<string, AudioClip>();

        UpdateSFX();

        //Adding SFX data into Dictionary<>
        for (int i = 0; i < sFX_Datas.Count; i++)
        {
            SFXDictionary.Add(sFX_Datas[i]._ClipName, sFX_Datas[i]._SFX);
        }

        //Adding BGM data into Dictionary<>
        for (int i = 0; i < bGM_Datas.Length; i++)
        {
            BGMDictionary.Add(bGM_Datas[i]._Name, bGM_Datas[i]._BGM);
        }
    }

    private void Start()
    {
        
    }

    public void UpdateSFX()
    {
        var clips_SFX = Resources.LoadAll("SFX", typeof(AudioClip)).Cast<AudioClip>().ToArray();

        sFX_Datas = new List<SFX_Data>();

        for (int i = 0; i < clips_SFX.Length; i++)
        {
            SFX_Data sFX_Data = new SFX_Data();

            sFX_Data._SFX = clips_SFX[i];
            sFX_Data._ClipName = clips_SFX[i].name;

            sFX_Datas.Add(sFX_Data);
        }
    }

    public void PlayBGM()
    {
        _BGMSource.Stop();

        AudioClip audioClip;

        if (BGMDictionary.TryGetValue(SceneManager.GetActiveScene().name, out audioClip))
        {
            _BGMSource.clip = audioClip;
            _BGMSource.Play();
        }
    }

    public void PlaySFX(string clipName)
    {
        AudioClip audioClip;

        if (SFXDictionary.TryGetValue(clipName, out audioClip))
        {
            _SFXSource.Stop();
            _SFXSource.clip = audioClip;
            _SFXSource.Play();
        }
    }
}

[System.Serializable]
public class BGM_Data
{
    public string _Name;
    public AudioClip _BGM;
}

public class SFX_Data
{
    public string _ClipName;
    public AudioClip _SFX;
}
