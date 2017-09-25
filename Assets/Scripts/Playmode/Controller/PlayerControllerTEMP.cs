using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harmony;

public class PlayerControllerTEMP : MonoBehaviour
{
    ProjetSynthese.Item weapon;
    // Use this for initialization
    void Start()
    {
        weapon = GetComponentInChildren<ProjetSynthese.Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector2.left * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(Vector2.right * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weapon.Use();
        }


        Vector2 lookAt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angleBetween = Vector2.SignedAngle(Vector2.up, lookAt);
        transform.localEulerAngles = new Vector3(0, 0, angleBetween);
    }
}
