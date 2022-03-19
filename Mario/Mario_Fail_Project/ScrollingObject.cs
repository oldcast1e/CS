using UnityEngine;

public class ScrollingObject : MonoBehaviour {
    public float speed = 2f; // 이동 속도

    private void Update() {
        // 게임 오브젝트를 왼쪽으로 일정 속도로 평행 이동하는 처리
        transform.Translate(Vector3.left*speed *Time.deltaTime);
    }
}