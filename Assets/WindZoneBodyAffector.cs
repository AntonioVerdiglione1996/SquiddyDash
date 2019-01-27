﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZoneBodyAffector : MonoBehaviour {
    public Rigidbody AffectedBody;
    public ExtendedWindZone Windzone;
    public ForceMode Mode = ForceMode.Force;
    private void FixedUpdate()
    {
        AffectedBody.AddForce(Windzone.WindVelocity3, Mode);
    }
}
