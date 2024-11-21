using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControlPanel : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage;

    public Sprite[] crystals;

    public Text guessCount,matchCount;

    public AudioSource pickAudio;
    public AudioSource matchAudio;
    public AudioSource wrongPickAudio;
    public AudioSource nextLevelAudio;
    public AudioSource gameEffectAudio;

    public List<Sprite> crystalButton = new List<Sprite>();
    public List<Button> buttons = new List<Button>();

    public int countGuess;
    public int correctCountGuess;
    public int gameGuess;

    public bool FirstClick, SecondClick;
    public int FirstGuessClick, SecondGuessClick;
    public string FirstGuessCrystal, SecondGuessCrystal;
    private string saveFilePath;

    void Awake()
    {
        crystals = Resources.LoadAll<Sprite> ("Images/Crystal");
        saveFilePath = Path.Combine(Application.persistentDataPath, "sceneSave.json");
    }
    void Start()
    {
        GetButtons();
        AddListener();
        AddGameCrystalPuzzle();
        gameGuess = crystalButton.Count / 2;
        ShuffleCrystal(crystalButton);

    }

    void GetButtons()
    {

        GameObject[] gameObj=GameObject.FindGameObjectsWithTag("Button");
        for (int i = 0; i < gameObj.Length; i++)
        {
            buttons.Add(gameObj[i].GetComponent<Button>());
            buttons[i].image.sprite = bgImage;
        }
    }

    void AddListener()
    {
        foreach (Button item in buttons)
        {
            item.onClick.AddListener(()=> GameLogic());
        }
    }
    void AddGameCrystalPuzzle()
    {
        int loop = buttons.Count;
        int index = 0;
        for (int i = 0; i < loop; i++)
        {
            if (index == loop / 2)
            {
                index = 0;
            }
            crystalButton.Add(crystals[index]);

            index++;
        }
    }
    public void GameLogic()
    {
        if (!FirstClick)
        {
            pickAudio.Play();
            FirstClick = true;
            FirstGuessClick = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            FirstGuessCrystal = crystalButton[FirstGuessClick].name;
            buttons[FirstGuessClick].image.sprite = crystalButton[FirstGuessClick];
        }
        else if (!SecondClick)
        {
            pickAudio.Play();
            SecondClick = true;
            SecondGuessClick = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            SecondGuessCrystal = crystalButton[SecondGuessClick].name;
            buttons[SecondGuessClick].image.sprite = crystalButton[SecondGuessClick];
            countGuess++;
            if(guessCount !=null)
            { 
                guessCount.text = countGuess.ToString();
            }
            StartCoroutine(CheckCrystalAreMatch());
        }    
    }
    IEnumerator CheckCrystalAreMatch()
    {
        yield return new WaitForSeconds(1f);
        if(FirstGuessCrystal == SecondGuessCrystal)
        {
            matchAudio.Play();
            buttons[FirstGuessClick].interactable = false;
            buttons[SecondGuessClick].interactable = false;

            buttons[FirstGuessClick].image.color =new Color(0,0,0,0);
            buttons[SecondGuessClick].image.color =new Color(0,0,0,0);

            ChecktheMatchFinish();
        }
        else
        {
            wrongPickAudio.Play();
            buttons[FirstGuessClick].image.sprite = bgImage;
            buttons[SecondGuessClick].image.sprite = bgImage;

        }
        yield return new WaitForSeconds(0f);
        FirstClick = false;
        SecondClick = false;

    }
    void ChecktheMatchFinish()
    {
        correctCountGuess++;
        if (matchCount != null)
        {
            matchCount.text = correctCountGuess.ToString();
        }
        if (correctCountGuess ==gameGuess)
        {
            nextLevelAudio.Play();
            Invoke("ChangeSense", 2);
      
        }
    }
    void ChangeSense()
    {
        int sceneBuild = 1;
        if (SceneManager.GetActiveScene().buildIndex < 4)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + (sceneBuild));
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    void ShuffleCrystal(List<Sprite>  sprites)
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            Sprite cry = sprites[i];
            int RandomIndex = Random.Range(0, sprites.Count);
            sprites[i] = sprites[RandomIndex];
            sprites[RandomIndex] = cry;
        }
    }
    public void SaveScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        File.WriteAllText(saveFilePath, currentSceneName);
        Debug.Log($"Scene '{currentSceneName}' saved!");
    }

    public string LoadScene()
    {
        if (File.Exists(saveFilePath))
        {
            
            string savedSceneName = File.ReadAllText(saveFilePath);
            Debug.Log($"Scene '{savedSceneName}' loaded!");
            return savedSceneName;
        }

        Debug.LogWarning("No saved scene found!");
        return null;
    }

    public void LoadSavedScene()
    {
        string sceneName = LoadScene();
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Failed to load scene. No scene name found.");
        }
    }

    public void ExitScene()
    {
        SceneManager.LoadScene(0);
    }
}
