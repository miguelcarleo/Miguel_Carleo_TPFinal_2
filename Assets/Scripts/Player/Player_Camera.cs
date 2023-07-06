using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player_Camera : MonoBehaviour
{
    AimBaseState currentState;
    public NoAimState noAim = new NoAimState();
    public AimState aim = new AimState();

    private float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    [SerializeField] float mouseSense;

    [HideInInspector] public Animator animator;
    [HideInInspector] public CinemachineVirtualCamera virtualCamera;
    public float adsFov = 40;
    [HideInInspector] public float noAimFov;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10;

    public Transform aimPos;
    [HideInInspector] public Vector3 actualAimPos;
    [SerializeField] float aimSmoothSpeed = 20;
    [SerializeField] LayerMask aimMask;


    private void Start()
    {
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        noAimFov = virtualCamera.m_Lens.FieldOfView;
        animator = GetComponent<Animator>();
        SwitchState(noAim);
    }
    private void Update()
    {
        if (GameManager.isActive)
        {
            xAxis += (Input.GetAxisRaw("Mouse X") * mouseSense);
            yAxis -= (Input.GetAxisRaw("Mouse Y") * mouseSense);
            yAxis = Mathf.Clamp(yAxis, -20, 20);

            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);

            Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCentre);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            {
                aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
            }

            currentState.UpdtateState(this);
        }
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
