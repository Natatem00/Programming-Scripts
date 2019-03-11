using UnityEngine;

public class PickUpItems : MonoBehaviour {

    [SerializeField]
    float maxDistance = 0.8f;                   // ray max distance
    float radius = 0.7f;                      	// sphere radius

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
        if (!grabbed && currentTime >= timeToUpdate)            // optimization
        {
            FindPickableItem();
        }
    }

    void LateUpdate()
    {
      if(grabbed)
      {
        GrabItem();
      }
    }

    void FindPickableItem()
    {
        // find pickable object
        if (Physics.SphereCast(playerTransform.position, radius, transform.forward, out hitObject, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            Debug.Log(hitObject.transform.tag);
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
        if (!Input.GetKey(KeyCode.E))
        {
            grabbed = false;
            hitObject.rigidbody.freezeRotation = false;
            return;
        }
        // pick up object
        hitObject.rigidbody.freezeRotation = true;
        hitObject.transform.position = playerTransform.position + transform.forward * maxDistance;
    }
}
