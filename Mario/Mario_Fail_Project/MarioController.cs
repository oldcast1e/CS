using System;
using UnityEngine;

public class MarioController : MonoBehaviour {
    public AudioClip deathClip; // 사망시 재생할 오디오 클립
    public float jumpForce = 700f; // 점프 힘
    public float speed = 100f; // 이동 속력

    private int jumpCount = 0; // 누적 점프 횟수
    private bool isWalk = false; 
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isRinning = false; // 바닥에 닿았는지 나타냄
    private bool isDead = false; // 사망 상태

    private bool isRight = false; // 오른쪽 방향 지시
    private bool isLeft = false; // 왼쪽 방향 지시

    private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
    private Animator animator; // 사용할 애니메이터 컴포넌트
    private AudioSource playerAudio; // 사용할 오디오 소스 컴포넌트
    private Transform Payertransform;

    private void Start() {
        // 게임 오브젝트로부터 사용할 컴포넌트들을 가져와 변수에 할당
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Payertransform = GetComponent<Transform>();
    }

    private void Update() {

        float xInput = Input.GetAxis("Horizontal");
        

        if (isDead)return;

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1){//스페이스 바  입ㅕ
            // Debug.Log("Input.GetKeyDown(KeyCode.Space)");
            jumpCount++;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            playerAudio.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space) && playerRigidbody.velocity.y > 0){ //스페이스 바를 땐  경ㅜ
            // Debug.Log("Input.GetKeyUp(KeyCode.Space)");
            playerRigidbody.velocity = playerRigidbody.velocity /2f;
        }
        
        if(Input.GetKey(KeyCode.RightArrow)){// 오른쪽 화살표 누르고 있는 동안 실행
            isWalk = true;
            isLeft = false; isRight = true;
            // playerRigidbody.AddForce(new Vector2(0, -60f));


            float xSpeed = xInput * speed;
            Vector2 newVelocity = new Vector2(xSpeed, 0f);
            // // 리지드바디의 속도에 newVelocity를 할당
            playerRigidbody.velocity = newVelocity;

            // Debug.Log("Put on RightArrow!");
            // Debug.Log("isLeft = false; isRight = true;");
            if(Input.GetKey(KeyCode.LeftShift)){
                // Debug.Log("Running!");
                isRinning = true;
                newVelocity = new Vector2(xSpeed*4, 0f);
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightArrow)) isRinning = false;
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow)){//오른쪽 화살표를 땐 경우
            isWalk = false;
            isRinning = false;
            playerRigidbody.velocity = Vector2.zero;
        }

        if(Input.GetKey(KeyCode.LeftArrow)){// 왼 화살표 누르고 있는 동안 실행
            isWalk = true;
            isRight = false;isLeft = true;
            // playerRigidbody.AddForce(new Vector2(speed, 0));
            if(playerRigidbody.velocity.y > 0) playerRigidbody.velocity = transform.up*(-1)*speed/2;


            float xSpeed = xInput * speed;
            Vector2 newVelocity = new Vector2(xSpeed, 0f);
            // // 리지드바디의 속도에 newVelocity를 할당
            playerRigidbody.velocity = newVelocity;

            // Debug.Log("Put on LeftArrow!");
            Debug.Log("isRight = false;isLeft = true;");
            if(Input.GetKey(KeyCode.LeftShift)){
                // Debug.Log("Running!");
                isRinning = true;
                newVelocity = new Vector2(xSpeed*4, 0f);
            }
              else if(Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftArrow)) isRinning = false;
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow)){//왼쪽 화살표를 땐 경우
            isWalk = false;
            isRinning = false;
            playerRigidbody.velocity = Vector2.zero;
        }
            
        

        // 애니메이터의 Grounded 파라미터를 isGrounded 값으로 갱신
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Run", isRinning);
        animator.SetBool("Walk", isWalk);

        animator.SetBool("isRight", isRight);
        animator.SetBool("isLeft", isLeft);
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