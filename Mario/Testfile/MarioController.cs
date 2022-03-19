using UnityEngine;

public class MarioControllerEdit : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip deathClip; // 사망시 재생할 오디오 클립

    /*마리오의 기본 상태 관리*/
    public float jumpForce = 700f; // 점프 힘
    public float speed = 3f; // 이동 속력
    private int jumpCount = 0; // 누적 점프 횟수

    /*마리오의 파라미터*/
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isRinning = false; // 바닥에 닿았는지 나타냄
    private bool isWalk = false; //걷고 있는 지 나타냄
    private bool isDead = false; // 사망 상태
    // private bool isRight = false; // 오른쪽 방향 지시
    // private bool isLeft = false; // 왼쪽 방향 지시
    private bool Flip = false; // 왼쪽 방향 지시

    private SpriteRenderer PlayerRenderer; //방향전환을 위한 변수 

    /*마리오의 컴포넌트*/
    private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
    private Animator animator; // 사용할 애니메이터 컴포넌트
    private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트
    private Transform Payertransform;

    void Start(){
        // 게임 오브젝트로부터 사용할 컴포넌트들을 가져와 변수에 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
        Payertransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        // PlayerRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        float xInput = Input.GetAxis("Horizontal");
        //축 설정
        if (isDead)return;

        /*스페이스 바 관리 조건문*/
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1){
            /*
            [스페이스 바 입력시 다음 문장 실행]

            1. 점프 카운트 증가
            2. 플레이어 리지드 바디 속도를 제로로 변경
            3. 위 쪽 방향으로 힘 발생
            4. 점프 오디오 실행
            */
            jumpCount++; //1
            playerRigidbody.velocity = Vector2.zero; //2
            playerRigidbody.AddForce(new Vector2(0, jumpForce)); //3
            playerAudio.Play(); //4

            if(Input.GetKey(KeyCode.LeftArrow))transform.localScale = new Vector3(-5, 5, 1); // 왼쪽 바라보기
            else if(Input.GetKey(KeyCode.RightArrow))transform.localScale = new Vector3(5, 5, 1); // 왼쪽 바라보기
        }
        else if (Input.GetKeyUp(KeyCode.Space) && playerRigidbody.velocity.y > 0){
            /*스페이스 바를 땐 경우 플레이어의 속도를 반으로 줄임*/
            playerRigidbody.velocity = playerRigidbody.velocity /2f;
        }
        //스페이스 바 관리 조건문 종료

        /*화살표 관리 조건문*/
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)){
            //오른쪽 화살표 또는 왼쪽 화살표를 누른 경우 = 이동키를 누른경우
            /*
            1. 이동키를 눌러야 달리기가 가능
            이때 방향키에 따라 Flip 파라미터를 변화한다.
            -> : Flip = 0
            <- : Flip = 1
            2. Walk 파라미터 변경
            3. x축 방향으로 속도 변경
            */
            if(Input.GetKey(KeyCode.LeftArrow))transform.localScale = new Vector3(-5, 5, 1); // 왼쪽 바라보기
            else if(Input.GetKey(KeyCode.RightArrow))transform.localScale = new Vector3(5, 5, 1); // 왼쪽 바라보기


            if(Input.GetKey(KeyCode.RightArrow)) Flip = false;
            else if(Input.GetKey(KeyCode.LeftArrow)) Flip = true;
            // -1 (왼쪽이면, flipX true(체크))

            animator.SetBool("Flip", Flip);
            isWalk = true; //2

            float xSpeed = xInput * speed; //3
            Vector2 newVelocity = new Vector2(xSpeed, 0f);
            playerRigidbody.velocity = newVelocity;

            if(Input.GetKey(KeyCode.LeftShift)){
                /*
                이동키를 누른 상태로 쉬프트를 누르고 있는 경우
                1. 달리기 실행
                2. Run 파라미터 변경
                */
                isRinning = true;
                Debug.Log("Running!");
                newVelocity = new Vector2(300f, 0f);
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift)){
                /*
                이동키를 누른 상태로 쉬프트를 땐 경우
                1. 달리기 해제
                2. Run 파라미터 변경
                */
                isRinning = false;
            }
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)){
            /*
            이동키를 땐 경우 아래 문장 실행
            1. isWalk 매개변수 변경
            2. isRinning 매개 변수 변경
            3. 플레이어의 속도를 (0,0)으로 변경
            */
            playerRigidbody.velocity = Vector2.zero;
            isWalk = false;
            isRinning = false;
        }         

        // 애니메이터의 Grounded 파라미터를 isGrounded 값으로 갱신
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Run", isRinning);
        animator.SetBool("Walk", isWalk);

        // // animator.SetBool("isRight", isRight);
        // // animator.SetBool("isLeft", isLeft);
    }
     private void Die() {

        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;

        playerAudio.Play();
        playerRigidbody.velocity = Vector2.zero;

        isDead = true;
        //
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Dead" && !isDead) Die();
        // Debug.Log("OnTriggerEnter2D");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Debug.Log("OnCollisionEnter2D");
        // 어떤 콜라이더와 닿았으며, 충돌 표면이 위쪽을 보고 있으면
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        isGrounded = false;
    }
}
