using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace InputKeySystem
{
    [Serializable]
    public class KeyEvent : UnityEvent<int> { }

    public class ChangeKeyView : MonoBehaviour
    {
        [SerializeField]
        Button inputButton;
        [SerializeField]
        KeyEvent keyEvent;			// changes keycode function

#if UNITY_EDITOR
        void OnValidate()
        {
            inputButton = transform.GetComponent<Button>();
        }
#endif
        void Awake()
        {
            inputButton.onClick.AddListener(StartCoroutine);
        }

        void StartCoroutine()
        {
            StartCoroutine(WaitForInput());
        }

        IEnumerator WaitForInput()
        {
            while(Input.inputString == "")
            {
                yield return null;
            }
			
            keyEvent.Invoke(System.Convert.ToInt32(Input.inputString[0]));
        }
    }
}
