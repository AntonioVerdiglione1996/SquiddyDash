using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRecycler : MonoBehaviour
{
    public Transform Character;
    public Transform Obj;
    public SOPool Pool;

    private void OnEnable()
    {
        if (!Character)
        {
            CharacterController controller = FindObjectOfType<CharacterController>();
            if (controller)
            {
                Character = controller.transform;
            }
        }
        if (!Obj)
        {
            Obj = transform.root;
        }
    }
    private void Update()
    {
        if (Character && Obj && Pool)
        {
            if (Character.position.y > Obj.position.y)
            {
                Pool.Recycle(Obj.gameObject);
            }
        }
    }
}
