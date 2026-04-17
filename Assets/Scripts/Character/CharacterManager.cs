using Unity.VisualScripting;
using UnityEngine;

namespace JBV
{
    public class CharacterManager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void Update()
        {

        }
    }
}
