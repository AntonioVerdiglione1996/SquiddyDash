using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerIsNotJumping : MonoBehaviour {
    public GameEvent Event;
    private SquiddyController controller;
	// Use this for initialization
	void Awake () {
        controller = FindObjectOfType<SquiddyController>();

    }
	
	// Update is called once per frame
	void Update () {
		if(!controller.IsJumping)
        {
            Event.Raise();
        }
	}
}
