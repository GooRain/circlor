using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{

	SpriteRenderer spriteRenderer;
	Camera mainCamera;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		mainCamera = Camera.main;
		transform.position = new Vector3(0, 0, 10);
	}

	public void SetBackground(Sprite newBackground)
	{
		spriteRenderer.sprite = newBackground;
		ScaleBackground();
	}

	public void ScaleBackground()
	{
		transform.localScale = Vector3.one;

		float cameraHeight = mainCamera.orthographicSize * 2;
		Vector2 cameraSize = new Vector2(mainCamera.aspect * cameraHeight, cameraHeight);
		Vector2 backgroundSize = spriteRenderer.sprite.bounds.size;

		Vector2 newScale = transform.localScale;
		newScale *= cameraSize.y / backgroundSize.y;

		transform.localScale = newScale;
	}
}
