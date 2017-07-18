using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

	public static MainMenuUI ins;

	enum MENU { MainMenu, Settings, LevelSelection, Credits };
	MENU currentMenu;

	public bool isEditMode = false;

	[Header("Menu References")]
	[SerializeField]
	private GameObject mainMenu;
	[SerializeField]
	private GameObject settingMenu;
	[SerializeField]
	private GameObject levelSelection;
	[SerializeField]
	private GameObject credits;

	[Header("Level Selection")]
	[SerializeField]
	private GameObject groupOfLevels;
	[SerializeField]
	private Sprite[] previews;

	[Header("Prefabs")]
	[SerializeField]
	private RectTransform prefabLevel;
	[SerializeField]
	private GameObject prefabLevelsHolder;

	[Header("Settings")]
	[SerializeField]
	private Slider sensitivitySlider;
	[SerializeField]
	private Text sensitivitySliderValueText;
	//[SerializeField]
	//private Toggle toggleColorBlind;

	private float defaultSensitivity = 0.15f;
	private int countOfLevels;
	private int[] levelsHighscores;

	public int GetHighscore(int index)
	{
		if(PlayerPrefs.HasKey("Highscore" + index))
		{
			return levelsHighscores[index];
		}
		else
		{
			return 0;
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
		Instantiate(prefabLevelsHolder);
		SwitchToMainMenu();
		InitLevelSelection();
		LoadSettings();
	}

	private void InitLevelSelection()
	{
		Debug.Log("Scenes in build: " + SceneManager.sceneCountInBuildSettings);
		countOfLevels = LevelMaster.ins.LevelsCount;
		levelsHighscores = new int[countOfLevels];
		LoadHighscores();
		for(int i = 0; i < countOfLevels; i++)
		{
			RectTransform newRectTransform = Instantiate(prefabLevel);
			newRectTransform.SetParent(groupOfLevels.transform);

			LevelPreviewer newLevel = newRectTransform.GetComponent<LevelPreviewer>();
			newLevel.SetHighscore(GetHighscore(i));
			newLevel.SetDifficulty((i / 2));
			newLevel.Index = i;

			Image preview = newRectTransform.GetChild(0).GetComponent<Image>();
			preview.sprite = previews[i];

			newRectTransform.anchorMin = Vector2.zero;
			newRectTransform.anchorMax = Vector2.one;
			newRectTransform.anchoredPosition = Vector2.zero;
			newRectTransform.sizeDelta = Vector2.zero;
			newRectTransform.localScale = Vector3.one;
			newRectTransform.anchoredPosition = new Vector3(newRectTransform.anchoredPosition.x + (i * 1080), newRectTransform.anchoredPosition.y);

			//Button newButton = newLevelTransform.GetComponentInChildren<Button>();
			//newButton.GetComponentInChildren<Text>().text = (i + 1).ToString();
			//int index = i + 1;
			//newButton.onClick.AddListener(() => { ButtonStartLevel(index); });
		}
	}

	public void LoadSettings()
	{
		LoadSensitivity();
		LoadColorBlindness();
		//LoadHighscores();
	}

	private void Start()
	{
		AudioMaster.ins.PlaySound("MainMenuTheme");
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(currentMenu != MENU.MainMenu)
			{
				SwitchToMainMenu();
			}
			else
			{
				Application.Quit();
			}
		}
	}

	/// <summary>
	///							Buttons methods
	/// </summary>
	public void ButtonStart()
	{
		PlayHitSound();
		SwitchToLevelSelectionMenu();
	}

	public void ButtonStartLevel()
	{
		PlayHitSound();
		LevelMaster.ins.StartLevel();
	}

	public void ButtonBackToMainMenu()
	{
		PlayHitSound();
		SwitchToMainMenu();
	}

	public void ButtonCredits()
	{
		PlayHitSound();
		SwitchToCredits();
	}

	public void ButtonSettings()
	{
		PlayHitSound();
		LoadSettings();
		SwitchToSettings();
	}

	public void ButtonApplySettings()
	{
		PlayHitSound();
		SaveSettings();
		SwitchToMainMenu();
	}

	public void ButtonCancelSettings()
	{
		PlayHitSound();
		SwitchToMainMenu();
	}

	public void ButtonExit()
	{
		PlayHitSound();
		Application.Quit();
	}

	/// <summary>
	/// 
	/// </summary>

	private void LoadHighscores()
	{
		for(int i = 0; i < countOfLevels; i++)
		{
			string key = "Highscore" + i;
			if(!PlayerPrefs.HasKey(key))
			{
				PlayerPrefs.SetInt(key, 0);
				levelsHighscores[i] = 0;
			}
			else
			{
				levelsHighscores[i] = PlayerPrefs.GetInt(key);
			}
		}
	}

	private void LoadSensitivity()
	{
		string key = "Sensitivity";
		if(PlayerPrefs.HasKey(key))
			sensitivitySlider.value = PlayerPrefs.GetFloat(key) * 100f;
		else
		{
			PlayerPrefs.SetFloat(key, defaultSensitivity);
			sensitivitySlider.value = PlayerPrefs.GetFloat(key) * 100f;
		}
	}

	private void LoadColorBlindness()
	{
		//string key = "ColorBlindness";
		//if(PlayerPrefs.HasKey(key))
		//	toggleColorBlind.isOn = PlayerPrefsX.GetBool(key);
		//else
		//{
		//	PlayerPrefsX.SetBool(key, false);
		//	toggleColorBlind.isOn = PlayerPrefsX.GetBool(key);
		//}
	}

	public void SaveSettings()
	{
		SaveSensitivity();
		SaveColorBlindness();
	}

	private void SaveSensitivity()
	{
		string key = "Sensitivity";
		PlayerPrefs.SetFloat(key, sensitivitySlider.value / 100f);
	}

	private void SaveColorBlindness()
	{
		//string key = "ColorBlindness";
		//PlayerPrefsX.SetBool(key, toggleColorBlind.isOn);
	}

	public void OnSliderValueChange()
	{
		sensitivitySliderValueText.text = "Sensitivity: " + sensitivitySlider.value;
	}

	private void SwitchToMainMenu()
	{
		currentMenu = MENU.MainMenu;
		mainMenu.SetActive(true);
		settingMenu.SetActive(false);
		levelSelection.SetActive(false);
		credits.SetActive(false);
	}

	private void SwitchToSettings()
	{
		currentMenu = MENU.Settings;
		mainMenu.SetActive(false);
		settingMenu.SetActive(true);
		levelSelection.SetActive(false);
		credits.SetActive(false);
	}

	private void SwitchToLevelSelectionMenu()
	{
		currentMenu = MENU.LevelSelection;
		mainMenu.SetActive(false);
		settingMenu.SetActive(false);
		levelSelection.SetActive(true);
		credits.SetActive(false);
	}

	private void SwitchToCredits()
	{
		currentMenu = MENU.Credits;
		mainMenu.SetActive(false);
		settingMenu.SetActive(false);
		levelSelection.SetActive(false);
		credits.SetActive(true);
	}

	private void PlayHitSound()
	{
		AudioMaster.ins.PlaySound("TapMenuButton");
	}
}
