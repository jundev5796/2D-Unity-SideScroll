using UnityEngine;

public class Hit_Lazer : MonoBehaviour
{
    float Speed = 50f;
    Vector2 MousePos;
    Transform tr;
    Vector3 dir;

    float angle;
    Vector3 dirNo;

    void Start()
    {
        tr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        MousePos = Input.mousePosition;
        MousePos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector3 Pos = new Vector3(MousePos.x, MousePos.y, 0);
        dir = Pos - tr.position;

        // 바라보는 각도구하기
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // normalized 단위벡터
        dirNo = new Vector3(dir.x, dir.y, 0).normalized;

        Destroy(gameObject, 4f);
    }

    void Update()
    {
        // 회전적용
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // move
        transform.position += dirNo * Speed * Time.deltaTime;
    }
}
