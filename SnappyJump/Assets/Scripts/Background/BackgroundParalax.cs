using UnityEngine;

public class BackgroundParalax : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;
    private float startpos;
    private float length;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float dist = CameraControll.Instance.VirtualCamera.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startpos + dist, 0, transform.position.z);
    }
}


