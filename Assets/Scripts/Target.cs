using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event System.Action TargetPositionChanged;

    public LayerMask groundMask;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, groundMask))
            {
                transform.position = hit.point;
                TargetPositionChanged();
            }
        }
    }
}
