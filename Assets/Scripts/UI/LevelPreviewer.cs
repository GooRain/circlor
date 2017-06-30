using UnityEngine;
using UnityEngine.UI;

public class LevelPreviewer : MonoBehaviour
{

	[SerializeField]
	private Text highscoreText;
	[SerializeField]
	private Text difficultyText;

	private string[] difficulties = { "Very easy", "Easy", "Medium", "Hard", "Very hard", "Extreme", "Cookiezi" };

	private string difficulty;
	private int highscore;
	private int index;
	private RectTransform myRectTransform;

	public int Index
	{
		get
		{
			return index;
		}

		set
		{
			index = value;
		}
	}

	private void Start()
	{
		//myRectTransform = GetComponent<RectTransform>();
		//myRectTransform.anchorMin = Vector2.zero;
		//myRectTransform.anchorMax = Vector2.one;
		//myRectTransform.anchoredPosition = Vector2.zero;
		//myRectTransform.sizeDelta = Vector2.zero;
		//myRectTransform.localScale = Vector3.one;
		//myRectTransform.position = new Vector3(myRectTransform.position.x + (index * Screen.width), myRectTransform.position.y);
		//newRect.pivot = new Vector2(0.5f, 0.5f);
		//newRect.offsetMin = newRect.offsetMax = Vector2.zero;

		//newLevel.localScale = Vector3.one;
		//newLevel.position = Vector3.zero;
	}

	public void SetDifficulty(int index)
	{
		difficultyText.text = "Difficulty: " + difficulties[index];
	}

	public void SetHighscore(int value)
	{
		highscore = value;
		highscoreText.text = "Highscore: " + highscore;
	}
}
