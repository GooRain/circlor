using UnityEngine;

public class BlockController : MonoBehaviour {

	[SerializeField]
	private GameObject graphics;
	[SerializeField]
	private SpriteRenderer coloredPart;
	[SerializeField]
	private Transform collisionChecker;

	private int colorIndex;

	public void SetColorIndex(int index)
	{
		colorIndex = index;
	}

	public int GetColorIndex()
	{
		return colorIndex;
	}

	public void SetColor(int index)
	{
		SetColorIndex(index);
		coloredPart.color = GameMaster.ins.GetColor(index);
	}

	public Color GetColor()
	{
		return coloredPart.color;
	}

	public float GetCollisionCheckerPosY()
	{
		return collisionChecker.position.y;
	}
}
