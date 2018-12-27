using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ItemGPSAR
{
    enum ItemState
    {
        LOOKING,
        PICKUPING
    };

    public class FindItemGPSAR : ARItem
    {
        ItemState state;
        [HideInInspector]
        public Coroutine currentCorotine = null;

        [HideInInspector]
        public Vector3 originalScale;

        [SerializeField]
        Text text;

        [SerializeField]
        protected float pickUpDistance = 0.4f;

        protected override void Awake()
        {
            base.Awake();
            transform.parent.position = new Vector3(transform.parent.position.x,
                                                    8f,
                                                    transform.parent.position.z);
            originalScale = transform.localScale;
        }

        public virtual IEnumerator StateMachine()
        {
            while(true)
            {
                switch (state)
                {
                    case ItemState.LOOKING:
                        Lookint();
                        break;
                    case ItemState.PICKUPING:
                        PickUping();
                        break;
                }
                yield return null;
            }
        }

        protected virtual void Lookint()
        {   ////////////
            try
            {
                text = GameObject.Find("TextDis").GetComponent<Text>();
                text.text = "Distance: " + distanceToTarget.ToString();
            }
            catch
            {
                Debug.Log("Can't find Text object");
            }
            ////////////
            if (Camera.main.name == ARCameraName)
            {
                float distance = distanceToTarget;

                // if player goes to item
                if (distance > pickUpDistance + 0.2f)
                {
                    ChangeItemScale(distance);
                }
                // if player gets in "pick up" radius
                else if (distance <= pickUpDistance && targetTrans)
                {
                    state = ItemState.PICKUPING;
                }
            }
            else
            {
                // if player uses not AR camera - change item scale to original
                transform.localScale = originalScale;
            }
        }

        public override void PickUping()
        {
            if (currentCorotine != null && Camera.main.name == ARCameraName)
            {
                StopCoroutine(currentCorotine);
            }
            base.PickUping();
        }
    }
}
