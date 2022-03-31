using UnityEngine;

public class MarioControllerEdit2 : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip deathClip; // 사망시 재생할 오디오 클립

    /*마리오의 기본 상태 관리*/
    public float maxSpeed; //최대 속력 변수 
    public float jumpForce = 700f; // 점프 힘
    public float speed = 3f; // 이동 속력


    /*마리오의 파라미터*/
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isDead = false; // 사망 상태
    private bool isWalk = false; // 걷기 상태
    private int jumpCount = 0; // 점프 가능 상태
    // private bool isRight = false; // 오른쪽 방향 지시
    // private bool isLeft = false; // 왼쪽 방향 지시

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

    /////////////
    void update(){
        if (isDead)return;
        if(Input.GetKeyDown(KeyCode.Space)&& jumpCount < 1){
            //Grounded이 false인 경우
            Debug.Log("Jump!");jumpCount++;
            playerAudio.Play(); //4
            playerRigidbody.AddForce(Vector2.up* jumpForce , ForceMode2D.Impulse);
        }
        if(Input.GetKeyUp(KeyCode.Space)){ // 버튼에서 손을 때는 경우 
            // normalized : 벡터 크기를 1로 만든 상태 (단위벡터 : 크기가 1인 벡터)
            // 벡터는 방향과 크기를 동시에 가지는데 크기(- : 왼 , + : 오)를 구별하기 위하여 단위벡터(1,-1)로 방향을 알수 있도록 단위벡터를 곱함 
            playerRigidbody.velocity = new Vector2( 0.5f * playerRigidbody.velocity.normalized.x , playerRigidbody.velocity.y);
        }
        if(Input.GetKeyUp(KeyCode.RightArrow) ||Input.GetKeyUp(KeyCode.LeftArrow)){ // 버튼에서 손을 때는 경우 
            playerRigidbody.velocity = Vector2.zero;
            isWalk = false;
        }
        // //Direction Sprite
        // if(Input.GetButtonDown("Horizontal"))

        
        // //Animation
        // if( Mathf.Abs(playerRigidbody.velocity.x) < 0.2) //속도가 0 == 멈춤 
        //     animator.SetBool("Walk",false); //Walk 변수 : false `
        // else// 이동중 
        //     animator.SetBool("Walk",true);

        animator.SetBool("Grounded", isGrounded);
        
    
        /**/
        // playerRigidbody.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)){
            isWalk = true;
            float h = Input.GetAxisRaw("Horizontal");   
            float xSpeed = h  * speed; //3
            
            if(Input.GetKey(KeyCode.RightArrow)){
                transform.localScale = new Vector3(5, 5, 1); // 오른쪽 바라보기
                // playerRigidbody.velocity= new Vector2(maxSpeed, playerRigidbody.velocity.y); //해당 오브젝트의 속력은 maxSpeed 
                Vector2 newVelocity = new Vector2(xSpeed, 0f);
                playerRigidbody.velocity = newVelocity;
                Debug.Log("오른쪽으로 이동!");

            }
            else if(Input.GetKey(KeyCode.LeftArrow)){
                transform.localScale = new Vector3(-5, 5, 1); // 왼쪽 바라보기
                // playerRigidbody.velocity =  new Vector2(maxSpeed*(-1), playerRigidbody.velocity.y); //y값은 점프의 영향이므로 0으로 제한을 두면 안됨 
                Vector2 newVelocity = new Vector2(xSpeed, 0f);
                playerRigidbody.velocity = newVelocity;
                Debug.Log("왼쪽으로 이동!");
            }
            
        }

        
        // if(playerRigidbody.velocity.x > maxSpeed){  //오른쪽으로 이동 (+) , 최대 속력을 넘으면 
        //     animator.SetBool("Walk",true);
        //     playerRigidbody.velocity = Vector2.zero;
        //     transform.localScale = new Vector3(5, 5, 1); // 오른쪽 바라보기

        //     playerRigidbody.velocity= new Vector2(maxSpeed, playerRigidbody.velocity.y); //해당 오브젝트의 속력은 maxSpeed 
        //     Debug.Log("오른쪽으로 이동!");
        // }
        
        // else if(playerRigidbody.velocity.x < maxSpeed*(-1)){ // 왼쪽으로 이동 (-) 
        //     animator.SetBool("Walk",true);
        //     playerRigidbody.velocity = Vector2.zero;
        //     transform.localScale = new Vector3(-5, 5, 1); // 왼쪽 바라보기

        //     playerRigidbody.velocity =  new Vector2(maxSpeed*(-1), playerRigidbody.velocity.y); //y값은 점프의 영향이므로 0으로 제한을 두면 안됨 
        //     Debug.Log("왼쪽으로 이동!");
        // }

        if(Input.GetKey(KeyCode.LeftShift)){
            /*
            이동키를 누른 상태로 쉬프트를 누르고 있는 경우
            1. 달리기 실행
            2. Run 파라미터 변경
            */
            animator.SetBool("Run",true); //거리가 0.5보다 작아지면 변경
            Debug.Log("Running!");
            int dir = (playerRigidbody.velocity.x > maxSpeed)?1:((playerRigidbody.velocity.x < maxSpeed*(-1))?-1:1);
            playerRigidbody.velocity= new Vector2(maxSpeed*5, playerRigidbody.velocity.y); //해당 오브젝트의 속력은 maxSpeed 
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift)){
                /*
                이동키를 누른 상태로 쉬프트를 땐 경우
                1. 달리기 해제
                2. Run 파라미터 변경
                */
                animator.SetBool("Run",false); //거리가 0.5보다 작아지면 변경
        }

        //Landing Paltform
        Debug.DrawRay(playerRigidbody.position, Vector3.down, new Color(0,1,0)); 
        //빔을 쏨(디버그는 게임상에서보이지 않음 ) 시작위치, 어디로 쏠지, 빔의 색 

         RaycastHit2D rayHit = Physics2D.Raycast(playerRigidbody.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
        //빔의 시작위치, 빔의 방향 , 1:distance , ( 빔에 맞은 오브젝트를 특정 레이어로 한정 지어야할 때 사용 ) 
        // RaycastHit2D : Ray에 닿은 오브젝트 클래스 

        //rayHit는 여러개 맞더라도 처음 맞은 오브젝트의 정보만을 저장(?) 
        if(playerRigidbody.velocity.y < 0){ // 뛰어올랐다가 아래로 떨어질 때만 빔을 쏨 
            if(rayHit.collider != null){ //빔을 맞은 오브젝트가 있을때  -> 맞지않으면 collider도 생성되지않음 
                if(rayHit.distance < 0.5f) 
                    Debug.Log("Grounding!");
                    isGrounded = true;
                    jumpCount = 0;
                    animator.SetBool("Grounded",isGrounded); //거리가 0.5보다 작아지면 변경

            }
        }
        animator.SetBool("Walk", isWalk);
    }
    
    
    /////////////
    private void Die() {

        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;

        playerAudio.Play();
        playerRigidbody.velocity = Vector2.zero;

        isDead = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Dead" && !isDead) Die();
        Debug.Log("OnTriggerEnter2D");
    }

    // private void OnCollisionEnter2D(Collision2D collision) {
        
    //     // 어떤 콜라이더와 닿았으며, 충돌 표면이 위쪽을 보고 있으면
    //     if (collision.contacts[0].normal.y > -3.69f){
    //         Debug.Log("OnCollisionEnter2D");
    //         isGrounded = true;
    //         jumpCount = 0;
    //     }
    // }
    // void OnCollisionEnter2D(Collision2D other) 
    // {
    //     if (other.gameObject.name == "Platform"){
    //         Debug.Log("OnCollisionEnter2D");
    //         isGrounded = true;
    //         jumpCount = 0;
    //     }
    // }

    private void OnCollisionExit2D(Collision2D collision) {
        isGrounded = false;
    }
}
