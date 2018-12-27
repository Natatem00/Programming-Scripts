using UnityEngine;

namespace MiniGames
{
    public class ARMiniGame : MonoBehaviour
    {

        [SerializeField]
        protected Transform ARObjectTransform;
        protected Transform target;
        [SerializeField]
        protected string ARCameraName = "ARCamera";
        [SerializeField]
        string targetTag = "Player";

        [SerializeField]
        float scaleCoof = 0.1f;

        protected virtual float distanceToTarget
        {
            get
            {
                return Vector2.Distance(new Vector2(target.position.x, target.position.z),
                                        new Vector2(ARObjectTransform.position.x,
                                        transform.position.z));
            }
        }

        protected virtual void Awake()
        {
            target = GameObject.FindGameObjectWithTag(targetTag).transform;
        }

        protected virtual void ChangeScale(float distance, Transform trans)
        {
            float scale = scaleCoof / distance;
            trans.localScale = Vector3.Slerp(trans.localScale, new Vector3(scale, scale, scale), 2f * Time.deltaTime);
        }
    }
}
