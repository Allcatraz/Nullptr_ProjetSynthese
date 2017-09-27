﻿using Harmony;
using UnityEngine;
using Time = UnityEngine.Time;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Actuator/PlayerMover")]
    public class PlayerMover : GameScript
    {
        [SerializeField] private float moveSpeed;

        private Transform topParentTransform;

        private void InjectImpulseMover([TopParentScope] Transform topParentTransform)
        {
            this.topParentTransform = topParentTransform;
        }

        private void Awake()
        {
            InjectDependencies("InjectImpulseMover");
        }

        public void Move(Vector3 direction)
        {
            topParentTransform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }

        public void Rotate()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 distance = new Vector3(mousePos.x - topParentTransform.position.x, mousePos.y - topParentTransform.position.y, mousePos.z - topParentTransform.position.z);
            float angle = (Mathf.Atan2(distance.x, distance.z) * 180 / Mathf.PI);
            topParentTransform.eulerAngles = new Vector3(0, angle, 0);
        }
    }
}