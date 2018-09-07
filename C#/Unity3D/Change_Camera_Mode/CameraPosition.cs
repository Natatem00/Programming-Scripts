using UnityEngine;

public class CameraPosition : MonoBehaviour {

    /// <summary>
    /// I try not to use GetComponent in "not prefab objects" in order not to 
    /// complicate the algorithm in terms of a 'Big O', so I use links
    /// </summary>

    //variables where have stored camera rotation
    float currentX = 0f, currentY = 0f; 

    //min and max Y for camera
    const float MAX_Y = 10f, MIN_Y = -50f, First_MAX_Y = 60, First_MIN_Y = -60;

    //camera sensitivity
    [SerializeField]
    float cinsX, cinsY;

    //begin camera position
    Vector3 begin_pos;

    //camera mode
    bool FirstPerson;

    //touch area(links request)
    [SerializeField]
    FixedTouchField touch;

    //required components(links request)
    [SerializeField]
    GameObject player;
    [SerializeField]
    Movement move; //object movement control script


	void Start () {
        begin_pos = new Vector3(0, 2f, -1f);
        cinsX = 0.2f; 
        cinsY = 0.2f;
        FirstPerson = false;
	}

    void Update()
    {   //changes camera mode
        //TODO change on CHANGE_CAMERA_BUTTON
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeCameraMode();
        }

        SetCurrentPositions();

        //if camera in FPS mode
        if (FirstPerson)
        {
            FirstPersonCamera();
        }
    }

    void LateUpdate () {
        //if camera in TPS mode
        if (!FirstPerson)
        {
            ThirdPersonCamera();
        }
    }

    //////////////////////////////////////////////////////////////////

    private void SetCurrentPositions()
    {
        currentX += touch.TouchDist.x * cinsX;
        currentY -= touch.TouchDist.y * cinsY;
    }

    private void ChangeCameraMode()
    {
        FirstPerson = !FirstPerson;
    }

    private void ThirdPersonCamera()
    {
        currentY = Mathf.Clamp(currentY, MIN_Y, MAX_Y);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        gameObject.transform.position = player.transform.position + rotation * begin_pos;
        gameObject.transform.LookAt(new Vector3(player.transform.position.x + 0.2f, player.transform.position.y + 1.6f, player.transform.position.z));
        if (move.joystick.Horizontal > 0) //if object is moving -> change it direction
            move.SetDirection(new Vector3(0, currentX, 0));
    }

    private void FirstPersonCamera()
    {
        currentY = Mathf.Clamp(currentY, First_MIN_Y, First_MAX_Y);
        //camera on player head
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2f, player.transform.position.z);
        move.SetDirection(new Vector3(0, currentX, 0));
        transform.localRotation = Quaternion.Euler(new Vector3(currentY, currentX, 0));
        return;
    }

    //////////////////////////////////////////////////////////////////
}
