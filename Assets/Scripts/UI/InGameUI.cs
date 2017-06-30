using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{

	[SerializeField]
	private GameObject gameUI;
	[SerializeField]
	private GameObject gameOverUI;
	[SerializeField]
	private GameObject pauseUI;

	[SerializeField]
	private GameObject prefabHeart;

	[Header("Texts")]
	[SerializeField]
	private Text pauseText;
	[SerializeField]
	private Text textFinalScore;
	[SerializeField]
	private Text textHighScore;
	[SerializeField]
	private Text textScore;

	[Header("Pictures")]
	[SerializeField]
	private GameObject lives;

	public void InitGame()
	{
		UpdateLives(GameMaster.ins.CurrentHealth);
		UpdateScore(GameMaster.ins.Score);
		SwitchToInGame();
	}

	public void PauseGame()
	{
		SwitchToPause();
	}

	public void ChangePauseText(float seconds)
	{
		if(seconds >= 1f)
		{
			pauseText.text = (Mathf.Ceil(seconds) - 1).ToString();
		}
		else
		{
			pauseText.text = "GO!";
		}
	}

	public void GameOver()
	{
		SwitchToGameOver();
		SetHighscore(GameMaster.ins.Score);
	}

	public void UpdateLives(int value)
	{
		int childs = lives.transform.childCount;
		if(childs > value)
		{
			int extra = Mathf.Abs(childs - value);
			for(int i = 0; i < extra; i++)
			{
				Destroy(lives.transform.GetChild(0).gameObject);
			}
		}
		else if(childs < value)
		{
			int extra = Mathf.Abs(childs - value);
			for(int i = 0; i < extra; i++)
			{
				GameObject newHeart = Instantiate(prefabHeart);
				RectTransform rt = newHeart.GetComponent<RectTransform>();
				rt.localScale = rt.lossyScale / (1 / transform.localScale.x);
				//newHeart.GetComponent<Image>().
				rt.SetParent(lives.GetComponent<RectTransform>());
			}
		}
		else
			return;
		//textLives.text = "Lives:" + value;
	}

	public void UpdateScore(int value)
	{
		textScore.text = "" + value;
	}

	public void SetHighscore(int finalScore)
	{
		string key = "Highscore" + LevelMaster.ins.CurrentLevelIndex;
		bool isNewHighscore = false;

		if(PlayerPrefs.HasKey(key))
		{
			if(PlayerPrefs.GetInt(key) <= finalScore)
			{
				isNewHighscore = true;
				PlayerPrefs.SetInt(key, finalScore);
			}
		}
		else
			PlayerPrefs.SetInt(key, finalScore);

		PlayerPrefs.Save();

		if(isNewHighscore)
		{
			textFinalScore.text = "You got " + GameMaster.ins.Score + " points!";
			textHighScore.text = "Your new highscore on this level is " + PlayerPrefs.GetInt(key) + "! Good job!";
		}
		else
		{
			textFinalScore.text = "You got only " + GameMaster.ins.Score + " points :(";
			textHighScore.text = "Your current highscore on this level is " + PlayerPrefs.GetInt(key) + " points.";
		}
	}

	public void Retry()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Exit()
	{
		SceneManager.LoadScene(0);
	}

	private void SwitchToInGame()
	{
		gameUI.SetActive(true);
		gameOverUI.SetActive(false);
		pauseUI.SetActive(false);
	}

	private void SwitchToGameOver()
	{
		gameOverUI.SetActive(true);
		gameUI.SetActive(false);
		pauseUI.SetActive(false);
	}

	private void SwitchToPause()
	{
		pauseUI.SetActive(true);
		gameUI.SetActive(false);
		gameOverUI.SetActive(false);
	}
}
