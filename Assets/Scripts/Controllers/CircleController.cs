using UnityEngine;

public class CircleController : MonoBehaviour
{

	[SerializeField]
	private GameObject prefabBlock;
	//[SerializeField]
	private float radius = 2.5f;

	//[SerializeField]
	private float rotationSpeed = 0.15f;
	//[SerializeField]
	private float rotationLerpSpeed = 4f;

	private Vector3 theSpeed;
	private Vector3 avgSpeed;
	//private bool isDragging = true;
	private float movePos;

	/// <summary>
	/// Level Settings
	/// </summary>
	private int amountOfBlocks;
	private float blockYScale = 1f;

	private GameObject[] blocks;
	private float[] offset;
	private float previousPos = 0;

	private void Start()
	{
		LoadPrefs();
		LoadLevelSettings();
		InitBlocks();
		RotateBlocks();
	}

	private void LoadPrefs()
	{
		float defaultRotationSpeed = rotationSpeed;
		float defaultRotationLerpSpeed = rotationLerpSpeed;
		if(PlayerPrefs.HasKey("Sensitivity"))
		{
			rotationSpeed = PlayerPrefs.GetFloat("Sensitivity");
			rotationLerpSpeed = defaultRotationLerpSpeed / defaultRotationSpeed * rotationSpeed;
		}
		else
		{
			PlayerPrefs.SetFloat("Sensitivity", defaultRotationSpeed);
			rotationSpeed = PlayerPrefs.GetFloat("Sensitivity");
			rotationLerpSpeed = defaultRotationLerpSpeed / defaultRotationSpeed * rotationSpeed;
		}
	}

	private void LoadLevelSettings()
	{
		blockYScale = LevelMaster.ins.GetBlockYScale();
		amountOfBlocks = LevelMaster.ins.GetAmountOfBlocks();
		radius = LevelMaster.ins.GetRadius();
	}

	private void InitBlocks()
	{
		offset = new float[amountOfBlocks];
		blocks = new GameObject[amountOfBlocks];
		float step = (2 * Mathf.PI) / amountOfBlocks;
		for(int i = 0; i < amountOfBlocks; i++)
		{
			blocks[i] = Instantiate(prefabBlock);
			blocks[i].transform.parent = transform;
			float scaleY = (2 * Mathf.PI) / amountOfBlocks * blockYScale;
			float scaleX = blocks[i].transform.localScale.x * scaleY;
			blocks[i].transform.localScale = new Vector3(scaleX, scaleY, 1);
			offset[i] = step * i;

			float posX = Mathf.Cos(offset[i]) * radius;
			float posY = Mathf.Sin(offset[i]) * radius;
			Vector3 pos = new Vector2(posX, posY);
			BlockController blockController = blocks[i].GetComponent<BlockController>();
			blocks[i].transform.position = pos;
			blockController.SetColor(i);
		}
		transform.Rotate(new Vector3(0, 0, 270));
	}

	private void Update()
	{
		if(Input.touchCount > 0)
		{
			TouchFirstMove();
		}

		if(Input.touchCount <= 0)
		{
			TouchIdle();
		}

		if(GameMaster.ins.inEditMode)
		{
			if(Input.GetMouseButton(0))
			{
				MouseLeftButton();
			}

			if(Input.GetMouseButtonUp(0))
			{
				MouseLeftButtonUp();
			}
			previousPos = Input.mousePosition.x;
		}
		transform.Rotate(Camera.main.transform.forward * theSpeed.x * rotationSpeed, Space.World);

	}

	private void RotateBlocks()
	{
		for(int i = 0; i < amountOfBlocks; i++)
		{
			Vector2 rot = transform.position - blocks[i].transform.position;
			float rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
			blocks[i].transform.rotation = Quaternion.AngleAxis(rotZ, Vector3.forward);
		}
	}

	private void RotateCircle(Vector3 touchPos)
	{
		Vector3 rot = touchPos - transform.position;
		float rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(rotZ, Vector3.forward);
	}

	private void MouseLeftButton()
	{
		movePos = Input.mousePosition.x - previousPos;
		theSpeed = new Vector2(movePos, 0.0f);
		avgSpeed = Vector2.Lerp(avgSpeed, theSpeed, Time.deltaTime);
	}

	private void MouseLeftButtonUp()
	{
		float i = Time.deltaTime * rotationLerpSpeed;
		theSpeed = Vector2.Lerp(theSpeed, Vector2.zero, i);
	}

	private void TouchFirstMove()
	{
		Touch touch0 = Input.GetTouch(0);

		if(touch0.phase == TouchPhase.Began)
		{
			//isDragging = true;
		}

		if(touch0.phase == TouchPhase.Moved)
		{
			//isDragging = true;
			movePos = touch0.deltaPosition.x;
			theSpeed = new Vector2(movePos, 0.0f);
			avgSpeed = Vector2.Lerp(avgSpeed, theSpeed, Time.deltaTime);
		}

		if(touch0.phase == TouchPhase.Stationary)
		{
			//isDragging = false;
			float i = Time.deltaTime * rotationLerpSpeed;
			theSpeed = Vector2.Lerp(theSpeed, Vector2.zero, i);
		}
	}

	private void TouchIdle()
	{
		//isDragging = false;
		float i = Time.deltaTime * rotationLerpSpeed;
		theSpeed = Vector2.Lerp(theSpeed, Vector2.zero, i);
	}

}
