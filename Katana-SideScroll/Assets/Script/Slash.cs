using UnityEngine;

public class Slash : MonoBehaviour
{
    private GameObject p;
    public Vector3 direction = Vector3.right;

    void Start()
    {
        p = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = p.transform.position;
    }

    public void Des()
    {
        Destroy(gameObject);
    }
}
