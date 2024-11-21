using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControlPanel : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage;

    public Sprite[] crystals;


    public List<Sprite> crystalButton = new List<Sprite>();
    public List<Button> buttons = new List<Button>();

    public int countGuess;
    public int correctCountGuess;
    public int gameGuess;

    public bool FirstClick, SecondClick;
    public int FirstGuessClick, SecondGuessClick;
    public string FirstGuessCrystal, SecondGuessCrystal;

    void Awake()
    {
        crystals = Resources.LoadAll<Sprite> ("Images/Crystal");
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
            FirstClick = true;
            FirstGuessClick = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            FirstGuessCrystal = crystalButton[FirstGuessClick].name;
            buttons[FirstGuessClick].image.sprite = crystalButton[FirstGuessClick];
        }
        else if (!SecondClick)
        {
            SecondClick = true;
            SecondGuessClick = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            SecondGuessCrystal = crystalButton[SecondGuessClick].name;
            buttons[SecondGuessClick].image.sprite = crystalButton[SecondGuessClick];
            countGuess++;
            StartCoroutine(CheckCrystalAreMatch());
        }    
    }
    IEnumerator CheckCrystalAreMatch()
    {
        yield return new WaitForSeconds(1f);
        if(FirstGuessCrystal == SecondGuessCrystal)
        {
            buttons[FirstGuessClick].interactable = false;
            buttons[SecondGuessClick].interactable = false;

            buttons[FirstGuessClick].image.color =new Color(0,0,0,0);
            buttons[SecondGuessClick].image.color =new Color(0,0,0,0);

            ChecktheMatchFinish();
        }
        else
        {
            buttons[FirstGuessClick].image.sprite = bgImage;
            buttons[SecondGuessClick].image.sprite = bgImage;

        }
        yield return new WaitForSeconds(1f);
        FirstClick = false;
        SecondClick = false;

    }
    void ChecktheMatchFinish()
    {
        correctCountGuess++;
        if(correctCountGuess ==gameGuess)
        {

                int sceneBuild = 1;
            if (SceneManager.GetActiveScene().buildIndex< 4)
            { 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + (sceneBuild));
            }
            else
            {

            }

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
}