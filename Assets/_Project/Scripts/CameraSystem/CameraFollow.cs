using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float speed;

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
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        cam.transform.DORotateQuaternion(targetRotation, speed);
    }

    private void OnDestroy()
    {
        cameraController.onCameraChanged -= SetCam;
    }
}
