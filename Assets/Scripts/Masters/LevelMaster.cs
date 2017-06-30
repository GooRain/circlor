using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMaster : MonoBehaviour
{

	static public LevelMaster ins;

	[SerializeField]
	[Range(0, 10)]
	private int currentLevelIndex;
	[SerializeField]
	private LevelSettings[] settings;

	private LevelSettings currentSetting;
	private int levelsCount;

	public int CurrentLevelIndex
	{
		get
		{
			return currentLevelIndex;
		}

		set
		{
			currentLevelIndex = value;
		}
	}

	public int LevelsCount
	{
		get
		{
			return levelsCount;
		}

		private set
		{
			levelsCount = value;
		}
	}

	private void Awake()
	{
		if(!ins)
		{
			ins = this;
			DontDestroyOnLoad(ins);
		}
		else
		{
			Destroy(gameObject);
		}
		levelsCount = settings.Length;
		UpdateSettings();
	}

	private void Start()
	{

	}

	public LevelSettings GetLevelSettingsByIndex(int index)
	{
		return settings[index];
	}

	public LevelSettings GetCurrentLevelSettings()
	{
		UpdateSettings();
		return currentSetting;
	}

	public void StartLevel()
	{
		UpdateSettings();
		SceneManager.LoadScene("Level");
	}

	private void UpdateSettings()
	{
		Mathf.Clamp(currentLevelIndex, 0, levelsCount - 1);
		currentSetting = settings[currentLevelIndex];
	}

	public float GetBlockYScale()
	{
		return currentSetting.blockYScale;
	}

	public int GetAmountOfBlocks()
	{
		return currentSetting.blocksAmount;
	}

	public float GetBallGravityScale()
	{
		return currentSetting.ballGravityScale;
	}

	public int GetMaxHealth()
	{
		return currentSetting.maxHealth;
	}

	public float GetRadius()
	{
		return currentSetting.radius;
	}
}
