using UnityEngine;

[System.Serializable]
public class LevelSettings {

	public int blocksAmount = 5;
	[Range(0.5f, 2f)]
	public float ballGravityScale = 1f;
	public int maxHealth = 3;
	public float blockYScale = 2;
	public float radius = 2.5f;

}
