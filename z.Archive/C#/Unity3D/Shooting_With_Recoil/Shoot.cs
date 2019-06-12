using UnityEngine;

public class Shoot : MonoBehaviour {

    [SerializeField]
    float offset = 0.01f;
    [SerializeField]
    float shootDistance = 100;
    [SerializeField]
    float gunRecoil = 10f;
    [SerializeField]
    float timeToShoot = 1f;

    float currentShootTime = 1f;
    float sparyoffset = 1;

    [SerializeField]
    Transform firePosition;
    [SerializeField]
    Transform rotati;
    [SerializeField]
    GameObject bulletHole;
    [SerializeField]
    Animation fireAnimation;

    Vector3 forwardVector;
    bool fire = false;


    private void Awake()
    {
        forwardVector = transform.parent.forward;
    }

    void Update () {
        currentShootTime += Time.deltaTime;
		if(Input.GetMouseButton(0))
        {
            if(currentShootTime >= timeToShoot)
            {
                RaycastHit hitObject;
                var random1 = Random.Range(-offset, offset);
                var random2 = Random.Range(-offset, offset);
                var random3 = Random.Range(-offset, offset);
                if (Physics.Raycast(firePosition.position, firePosition.forward + new Vector3(random1, random2, random3) * sparyoffset, out hitObject, shootDistance))
                {
                    sparyoffset = 5;
                    forwardVector = transform.parent.forward;
                    fireAnimation.Play();
                    Instantiate(bulletHole, hitObject.point, Quaternion.FromToRotation(Vector3.up, hitObject.normal));
                    currentShootTime = 0;
                    transform.parent.forward = firePosition.forward + new Vector3(Mathf.Abs(random1), Mathf.Abs(random2), Mathf.Abs(random3)) * gunRecoil;
                    fire = true;
                }
            }
        }
        else
        {
            sparyoffset = 1;
        }

        if (transform.parent.forward != forwardVector && Input.GetAxis("Mouse Y") == 0 && fire)
        {
            transform.parent.forward = Vector3.Lerp(transform.parent.forward, forwardVector, 10f * Time.deltaTime);
        }
        else
        {
            fire = false;
        }
    }
}
