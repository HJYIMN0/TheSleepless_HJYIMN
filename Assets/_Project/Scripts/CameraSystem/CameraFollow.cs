using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float playerDistance = 2.5f;

    [SerializeField] private Transform player;
    [SerializeField] private Camera cam;
    [SerializeField] private CameraController cameraController;

    public void SetCam(Camera cam) => this.cam = cam;

    private void OnEnable()
    {
        if (player == null || cam == null || cameraController == null)
        {
            Debug.LogWarning("Missing component!");
            if (player == null)
            {
                Debug.Log("Player");
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            if (cameraController == null)
            {
                Debug.Log("CameraControl");
                cameraController = GameObject.FindAnyObjectByType<CameraController>(); 
            }
            if (cam == null)
            {
                Debug.Log("CameraControl");
                GameObject.FindAnyObjectByType<CameraController>().activeCam.GetComponent<Camera>().enabled = true;
            }
        }
    }
    private void Start()
    {
        cameraController.onCameraChanged += SetCam;        
    }

    private void FixedUpdate()
    {
        FollowPlayer();        
    }

    public void FollowPlayer()
    {
        Vector3 dir = player.position - cam.transform.position;
        if (dir.magnitude > playerDistance)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, speed * Time.fixedDeltaTime);
        }        
    }

    private void OnDestroy()
    {
        cameraController.onCameraChanged -= SetCam;
    }
}
