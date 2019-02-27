using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRecycler : ISOPoolable
{
    public Transform Character;

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
    }
    private void Update()
    {
        if (Character)
        {
            if (Character.position.y > Root.transform.position.y)
            {
                Recycle();
            }
        }
    }
}
