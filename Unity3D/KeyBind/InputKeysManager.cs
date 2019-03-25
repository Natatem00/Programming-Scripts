using System.Collections.Generic;
using UnityEngine;
namespace InputKeySystem
{
    // use this class to declaration keys names
    public static class KeysNames
    {
        public const string FORWARD_MOVE = "ForwardMove";
        public const string BACKWARD_MOVE = "BackwardMove";
        public const string LEFTWARD_MOVE = "LeftwardMove";
        public const string RIGHTWARD_MOVE = "RightwardMove";
    }
    [DisallowMultipleComponent]
    public class InputKeysManager : MonoBehaviour
    {
        public static InputKeysManager instance = null;

        static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

	void Awake()
        {
            // singleton
            if(instance != null)
            {
                DestroyImmediate(this);
				return;
            }
            else
            {
                instance = this;
            }
        }

        // get
        public static KeyCode ForwardMoveKey
        {
            get
            {
                if (keys.ContainsKey(KeysNames.FORWARD_MOVE))
                {
                    return keys[KeysNames.FORWARD_MOVE];
                }
                else
                {
                    keys.Add(KeysNames.FORWARD_MOVE, KeyCode.W);
                    return keys[KeysNames.FORWARD_MOVE];
                }
            }
        }
        public static KeyCode BackwardMoveKey
        {
            get
            {
                if (keys.ContainsKey(KeysNames.BACKWARD_MOVE))
                {
                    return keys[KeysNames.BACKWARD_MOVE];
                }
                else
                {
                    keys.Add(KeysNames.BACKWARD_MOVE, KeyCode.S);
                    return keys[KeysNames.BACKWARD_MOVE];
                }
            }
        }
        public static KeyCode RightwardMoveKey
        {
            get
            {
                if (keys.ContainsKey(KeysNames.RIGHTWARD_MOVE))
                {
                    return keys[KeysNames.RIGHTWARD_MOVE];
                }
                else
                {
                    keys.Add(KeysNames.RIGHTWARD_MOVE, KeyCode.D);
                    return keys[KeysNames.RIGHTWARD_MOVE];
                }
            }
        }
        public static KeyCode LeftwardMoveKey
        {
            get
            {
                if (keys.ContainsKey(KeysNames.LEFTWARD_MOVE))
                {
                    return keys[KeysNames.LEFTWARD_MOVE];
                }
                else
                {
                    keys.Add(KeysNames.LEFTWARD_MOVE, KeyCode.A);
                    return keys[KeysNames.LEFTWARD_MOVE];
                }
            }
        }
		
        // set
        public void SetForwardKey(int keyCode)
        {
            SetKeyCode(KeysNames.FORWARD_MOVE, (KeyCode)keyCode);
        }
        public void SetBackwardKey(int keyCode)
        {
            SetKeyCode(KeysNames.BACKWARD_MOVE, (KeyCode)keyCode);
        }
        public void SetLeftwardKey(int keyCode)
        {
            SetKeyCode(KeysNames.LEFTWARD_MOVE, (KeyCode)keyCode);
        }
        public void SetRightwardMove(int keyCode)
        {
            SetKeyCode(KeysNames.RIGHTWARD_MOVE, (KeyCode)keyCode);
        }

        void SetKeyCode(string keyName, KeyCode keyCode)
        {
            if (keys.ContainsValue(keyCode))
            {
                foreach (KeyValuePair<string, KeyCode> key in keys)
                {
                    if (key.Value == keyCode)
                    {
                        keys[key.Key] = 0;
                        break;
                    }
                }
            }
            keys[keyName] = keyCode;
        }
    }
}
