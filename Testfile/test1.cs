public interface IItem{
    void Use(GameObject target);
}

public class Test1 : MonoBehaviour, IItem {
    public int cnt = 30;

    public void Use(GameObject target){
        Debug.Log("변수의 값 증가"+cnt);
    }
}