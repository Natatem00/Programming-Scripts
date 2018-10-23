using UnityEngine;

public class GrabItems : MonoBehaviour {

    [SerializeField]
    float maxDistance = 0.8f;                 // ray max distance
    float radius = 0.3f;                      // sphere radius

    float timeToUpdate = 0.1f;     
    float currentTime = 0f;

    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    LayerMask layerMask;

    RaycastHit hitObject;

    bool grabbed = false;

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeToUpdate && !grabbed)            // optimization
        {
            FindPickableItem();
        }
        GrabItem();
    }

    void FindPickableItem()
    {
        // find pickable object
        if (Physics.SphereCast(playerTransform.position, radius, transform.forward, out hitObject, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            Debug.Log(hitObject.transform.tag);
            Debug.Log(grabbed);
            if (Input.GetMouseButton(0))
            {
                // pick up object
                hitObject.transform.GetComponent<Pickable>().PickUp();
            }
            else if (Input.GetKey(KeyCode.E))
            {
                grabbed = true;
            }
        }
        currentTime = 0f;
    }

    void GrabItem()
    {
        if (grabbed)
        {
            if (!Input.GetKey(KeyCode.E))
            {
                grabbed = false;
            }
            timeToUpdate = 0f;
            // pick up object
            hitObject.transform.position = playerTransform.position + transform.forward * maxDistance;
        }
        else
        {
            timeToUpdate = 0.1f;
        }
    }
}
