
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    public Text textScore, textMoves;

    //Для бара ХП врага
    public int ScoreToWin = 100;
    //Для бара ХП игрока
    public int PlayerHP = 100;

    public GameObject panelWin;
    public GameObject panelGameOver;
    public Text gTextScore, gTextWinScore;

    private void Awake()
    {
        instance = this;
    }

    public void Score(int value)
    {
        ScoreToWin += value;
        textScore.text = "Score to win: " + ScoreToWin.ToString();
        if (ScoreToWin <= 0)
        {
            WinGame();
        }
        textScore.text = "Score to win: " + ScoreToWin.ToString();
    }

    public void Moves(int value)
    {
        PlayerHP += value;
        if (PlayerHP <= 0)
        {
            GameOver();
        }
        textMoves.text = "Player HP: " + PlayerHP.ToString();

    }
    private void GameOver()
    {
        if (ScoreToWin > PlayerPrefs.GetInt("Score"))
        {
            PlayerPrefs.SetInt("Score", ScoreToWin);
            gTextWinScore.text = "XP: " + ScoreToWin.ToString();
        }
        else
        {
            gTextWinScore.text = "XP: " + PlayerPrefs.GetInt("XP: ");
        }

        gTextScore.text = "XP: " + ScoreToWin.ToString();

        panelGameOver.SetActive(true);

    }



    private void WinGame()
    {
        if (PlayerHP > PlayerPrefs.GetInt("XP"))
        {
            PlayerPrefs.SetInt("XP", PlayerHP);
            gTextWinScore.text = "XP: " + PlayerPrefs.GetInt("XP: ");
        }
        else
        {
            gTextWinScore.text = "HP: " + PlayerPrefs.GetInt("XP: ");
        }

        gTextWinScore.text = "XP: " + PlayerHP.ToString(); 

        panelWin.SetActive(true);

    }

}
