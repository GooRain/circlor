using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{

	public static GameMaster ins;

	private enum GameStates { STARTING, INGAME, PAUSED, GAMEOVER };

	private GameStates currentGameState = GameStates.STARTING;

	public bool inEditMode;

	[Header("Prefabs To Spawn")]
	[SerializeField]
	private InGameUI prefabGameUI;
	[SerializeField]
	private CircleController prefabCircleController;
	[SerializeField]
	private BallController prefabBallController;
	[SerializeField]
	private BackgroundScaler prefabBackgroundScaler;

	[Header("Game Settings")]
	[SerializeField]
	Vector3 ballSpawnPosition;

	[Header("Colors")]
	[SerializeField]
	private Color[] colors;
	[SerializeField]
	private Color[] colorsBlindness;

	private Color[] inGameColors;
	private bool isColorBlindness = false;

	private int score;
	private int currentHealth;

	private int amountOfBlocks;
	private int maxHealth;
	private float timer;

	public int Score
	{
		get
		{
			return score;
		}
		private set { }
	}
	public int CurrentHealth
	{
		get
		{
			return currentHealth;
		}

		private set
		{
			currentHealth = value;
		}
	}

	private void Awake()
	{
		if(!ins)
		{
			ins = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	/// <summary>
	///				Game objects
	/// </summary>
	private InGameUI gameUI;
	private CircleController circle;
	private BallController ball;
	private BackgroundScaler background;

	private void InitGame()
	{
		LoadLevelSettings();
		InitColors();

		AudioMaster.ins.StopSound("MainMenuTheme");
		score = 0;
		currentHealth = maxHealth;

		gameUI = Instantiate(prefabGameUI);
		circle = Instantiate(prefabCircleController, Vector3.zero, Quaternion.identity);
		ball = Instantiate(prefabBallController, ballSpawnPosition, Quaternion.identity);
		background = Instantiate(prefabBackgroundScaler, Vector3.zero, Quaternion.identity);
		background.ScaleBackground();

		//gameUI.InitGame();
		timer = 4;
		StartDelay();
		//StartCoroutine(Pause(timer));
		//ResumeGame();
	}

	private void LoadLevelSettings()
	{
		amountOfBlocks = LevelMaster.ins.GetAmountOfBlocks();
		maxHealth = LevelMaster.ins.GetMaxHealth();
	}

	private void InitColors()
	{
		inGameColors = new Color[amountOfBlocks];
		isColorBlindness = PlayerPrefsX.GetBool("ColorBlindness");
		if(!isColorBlindness)
		{
			colors = ShuffleArray(colors);
			for(int i = 0; i < amountOfBlocks; i++)
			{
				inGameColors[i] = colors[i];
			}
		}
		else
		{
			colorsBlindness = ShuffleArray(colorsBlindness);
			for(int i = 0; i < amountOfBlocks; i++)
			{
				inGameColors[i] = colorsBlindness[i];
			}
		}
	}

	private void Start()
	{
		InitGame();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))
		{
			SceneManager.LoadScene("Menu");
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			gameUI.gameObject.SetActive(false);
			ball.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			ball.transform.position = Vector3.zero;
			Application.CaptureScreenshot("Assets/Sprites/UI/Previews/" + "Preview" + LevelMaster.ins.CurrentLevelIndex + ".png");
		}

		if(currentGameState == GameStates.STARTING)
		{
			timer -= Time.deltaTime;
			gameUI.ChangePauseText(timer);
			if(timer <= 0) ResumeGame();
		}
	}

	public Color GetColor(int index)
	{
		//int index = Random.Range(0, inGameColors.Length);
		if(index >= 0 && index < amountOfBlocks)
		{
			return inGameColors[index];
		}
		return new Color(0, 0, 0);
	}

	public void AddScore(int amount)
	{
		if(currentGameState == GameStates.INGAME)
		{
			score += amount;
			gameUI.UpdateScore(score);
		}
	}

	public void AddLives(int amount)
	{
		if(currentGameState == GameStates.INGAME && !inEditMode)
		{
			currentHealth += amount;
			if(currentHealth <= 0)
			{
				GameOver();
			}
			else
			{
				gameUI.UpdateLives(currentHealth);
			}
		}
	}

	private void GameOver()
	{
		Debug.Log("GAME OVER");
		currentGameState = GameStates.GAMEOVER;
		gameUI.GameOver();
		AudioMaster.ins.PlaySound("GameOver");
		PauseGame();
	}

	private Color[] ShuffleArray(Color[] array)
	{
		for(int i = array.Length; i > 0; i--)
		{
			int j = UnityEngine.Random.Range(0, i);
			Color k = array[j];
			array[j] = array[i - 1];
			array[i - 1] = k;
		}
		return array;
	}

	private void StartDelay()
	{
		ball.gameObject.SetActive(false);
		gameUI.PauseGame();
		gameUI.ChangePauseText(timer);
		currentGameState = GameStates.STARTING;
	}

	private void PauseGame()
	{
		ball.gameObject.SetActive(false);
		circle.gameObject.SetActive(false);
	}

	private void ResumeGame()
	{
		Time.timeScale = 1;
		ball.gameObject.SetActive(true);
		gameUI.InitGame();
		currentGameState = GameStates.INGAME;
	}

}