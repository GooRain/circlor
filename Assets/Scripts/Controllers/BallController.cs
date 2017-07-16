using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem prefabHitParticle;
    [SerializeField]
    private SpriteRenderer graphics;

    [Range(1, 50)]
    [SerializeField]
    private float maxGravity;
    private float minGravity;

    private Vector3 startPosition;
    private Vector2 startOffset;

    private Rigidbody2D myRigidbody;
    private CircleCollider2D circleCollider;


    private ParticleSystem particleHit;

    private int minimumPrice = 1;
    private int price;

    private int amountOfBlocks;
    private float radius;
    private int colorIndex;

    private Ray checkColissionRay;

    private void Awake()
    {
        startPosition = transform.position;
        particleHit = Instantiate(prefabHitParticle, new Vector2(0, 50), Quaternion.identity);
        myRigidbody = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        radius = LevelMaster.ins.GetRadius();
        startOffset = startPosition;
        startOffset.y -= radius / 2;
        myRigidbody.gravityScale = LevelMaster.ins.GetBallGravityScale();
        minGravity = myRigidbody.gravityScale;
        price = minimumPrice;
        amountOfBlocks = LevelMaster.ins.GetAmountOfBlocks();
        SetColor(0);
    }

    private void SetColor(int index)
    {
        colorIndex = index;
        graphics.color = GameMaster.ins.GetColor(colorIndex);
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -5)
        {
            Fail();

            myRigidbody.velocity = Vector2.zero;
            transform.position = startPosition;
        }
        //Debug.Log(myRigidbody.velocity);
        if (transform.position.y >= startOffset.y && !circleCollider.enabled)
        {
            //myRigidbody.velocity = Vector2.up * CalcVelocity();
            circleCollider.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            BlockController blockController = collision.transform.GetComponent<BlockController>();
            if (transform.position.y > blockController.GetCollisionCheckerPosY())
            {
                //SpriteRenderer otherSpriteRenderer = collision.gameObject.GetComponentInChildren<SpriteRenderer>();
                //hitParticle.transform.position = (collision.transform.position + transform.position) / 2;
                particleHit.transform.position = collision.contacts[0].point;
                var main = particleHit.main;

                circleCollider.enabled = false;

                int blockColorIndex = blockController.GetColorIndex();
                main.startColor = GameMaster.ins.GetColor(blockColorIndex);

                //Debug.Log(degrees + " " + hitParticle.transform.rotation);
                particleHit.Play();

                CheckColor(blockColorIndex);
                myRigidbody.velocity = Vector2.up * CalcVelocity();
                AudioMaster.ins.PlaySound("Hit");
                //GameMaster.ins.PlayHitSound();
            }
        }
    }

    private void CheckColor(int blockColorIndex)
    {
        if (colorIndex == blockColorIndex)
        {
            myRigidbody.gravityScale += 0.01f * price;
            if (myRigidbody.gravityScale >= maxGravity)
                myRigidbody.gravityScale = maxGravity;
            GameMaster.ins.AddScore(price++);
            SetColor(Random.Range(0, amountOfBlocks));
        }
        else
        {
            Fail();
            //sr.color = GM.ins.GetColor(Random.Range(0, GM.ins.blocksAmount));
        }
    }

    private void Fail()
    {
        GameMaster.ins.AddLives(-1);
        price = minimumPrice;
        Debug.Log("Gravity was: " + myRigidbody.gravityScale);
        myRigidbody.gravityScale = minGravity;
    }

    private float CalcVelocity()
    {
        float gravity = Physics.gravity.magnitude * myRigidbody.gravityScale;
        //Debug.Log(rb.gravityScale);
        float distance = Vector2.Distance(transform.position, startPosition);
        float initialVelocity = Mathf.Sqrt(gravity * Mathf.Pow(distance, 2) / 2);
        return initialVelocity;
    }

}