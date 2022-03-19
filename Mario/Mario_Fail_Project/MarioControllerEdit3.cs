using UnityEngine;

public class MarioControllerEdit3 : MonoBehaviour
{
    public AudioClip deathClip; // 사망시 재생할 오디오 클립

    /*마리오의 기본 상태 관리*/
    public float maxSpeed; //최대 속력 변수 
    public float jumpForce = 700f; // 점프 힘
    public float speed = 3f; // 이동 속력

    /*마리오의 파라미터*/
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isDead = false; // 사망 상태
    private bool isWalk = false; // 걷기 상태
    private bool isRunning = false; // 걷기 상태

    /*마리오의 변수*/
    private int jumpCount = 0; // 점프 가능 상태

    private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
    private Animator animator; // 사용할 애니메이터 컴포넌트
    private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트
    private Transform Payertransform;
    // private SpriteRenderer PlayerRenderer; //방향전환을 위한 변수 

    void Start()
    {
        // 게임 오브젝트로부터 사용할 컴포넌트들을 가져와 변수에 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
        Payertransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        // PlayerRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float xSpeed = xInput * speed; //3

        if (isDead)return;

        ///////////////점프
        if(Input.GetKeyDown(KeyCode.Space)&& jumpCount < 1){
            Debug.Log("Jump!");jumpCount++;
            playerAudio.Play(); //4
            playerRigidbody.AddForce(new Vector2(0,jumpForce));
            // playerRigidbody.AddForce(Vector2.up* jumpForce , ForceMode2D.Impulse);
        }
        if(Input.GetKeyUp(KeyCode.Space)){ // 버튼에서 손을 때는 경우 

            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.normalized.x , playerRigidbody.velocity.normalized.y* 0.5f);
        }
        ///////////////방향키
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)){
            if(Input.GetKey(KeyCode.LeftArrow))transform.localScale = new Vector3(-5, 5, 1); // 왼쪽 바라보기
            else if(Input.GetKey(KeyCode.RightArrow))transform.localScale = new Vector3(5, 5, 1); // 왼쪽 바라보기

            playerRigidbody.velocity = new Vector2(xSpeed , playerRigidbody.velocity.normalized.y);
            isWalk = true; //2

            if(Input.GetKey(KeyCode.LeftShift)){
                isRunning = true;
                Debug.Log("Running!");
                // newVelocity = new Vector2(300f, 0f);
                playerRigidbody.velocity = new Vector2(xSpeed*3, playerRigidbody.velocity.normalized.y);
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift)){
                isRunning = false;
            }
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)){
            playerRigidbody.velocity = Vector2.zero;
            isWalk = false;
            isRunning = false;
        }
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Run", isRunning);
        animator.SetBool("Walk", isWalk);
        
    }

    private void Die() {

        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;

        playerAudio.Play();
        playerRigidbody.velocity = Vector2.zero;

        isDead = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Dead" && !isDead) Die();
        // Debug.Log("OnTriggerEnter2D");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        
        // void OnCollisionEnter(Collision collision) { Vector3 pos = collision.contacts[0].point; }
        Vector2 pos = collision.contacts[0].point; 
        Debug.Log("충돌 위치[x]: "+pos.x);
        Debug.Log("충돌 위치[y]: "+pos.y);
        // 어떤 콜라이더와 닿았으며, 충돌 표면이 위쪽을 보고 있으면
        if (collision.contacts[0].normal.y > -6.45f)
        {
            Debug.Log("Collision!");
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        isGrounded = false;
    }
}
