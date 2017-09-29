﻿using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/PlayerController")]
    public class PlayerController : NetworkGameScript
    {
        [SerializeField] private Menu inventoryMenu;

        private ActivityStack activityStack;
        private Health health;
        private KeyboardInputSensor keyboardInputSensor;
        private MouseInputSensor mouseInputSensor;
        private PlayerMover playerMover;
        private Inventory inventory;
        private ItemSensor itemSensor;

        private Weapon currentWeapon;

        private bool isInventoryOpen = false;

        private void InjectPlayerController([ApplicationScope] KeyboardInputSensor keyboardInputSensor,
                                            [ApplicationScope] MouseInputSensor mouseInputSensor,
                                            [ApplicationScope] ActivityStack activityStack,
                                            [EntityScope] PlayerMover playerMover,
                                            [EntityScope] Health health,
                                            [EntityScope] Inventory inventory,
                                            [EntityScope] ItemSensor itemSensor)
        {
            this.keyboardInputSensor = keyboardInputSensor;
            this.mouseInputSensor = mouseInputSensor;
            this.activityStack = activityStack;
            this.playerMover = playerMover;
            this.health = health;
            this.inventory = inventory;
            this.itemSensor = itemSensor;
        }

        private void Awake()
        {
            InjectDependencies("InjectPlayerController");

            keyboardInputSensor.Keyboards.OnMove += OnMove;
            keyboardInputSensor.Keyboards.OnToggleInventory += InventoryAction;
            keyboardInputSensor.Keyboards.OnPickup += OnPickup;
            keyboardInputSensor.Keyboards.OnSwitchSprintOn += OnSwitchSprintOn;
            keyboardInputSensor.Keyboards.OnSwitchSprintOff += OnSwitchSprintOff;
            keyboardInputSensor.Keyboards.OnSwitchPrimaryWeapon += OnSwitchPrimaryWeapon;
            keyboardInputSensor.Keyboards.OnSwitchSecondaryWeapon += OnSwitchSecondaryWeapon;

            mouseInputSensor.Mouses.OnFire += OnFire;

            Camera.main.GetComponent<CameraController>().PlayerToFollow = gameObject;

        }

        private void OnDestroy()
        {
            keyboardInputSensor.Keyboards.OnMove -= OnMove;
            keyboardInputSensor.Keyboards.OnToggleInventory -= InventoryAction;
            keyboardInputSensor.Keyboards.OnPickup -= OnPickup;
            keyboardInputSensor.Keyboards.OnSwitchSprintOn -= OnSwitchSprintOn;
            keyboardInputSensor.Keyboards.OnSwitchSprintOff -= OnSwitchSprintOff;
            keyboardInputSensor.Keyboards.OnSwitchPrimaryWeapon += OnSwitchPrimaryWeapon;
            keyboardInputSensor.Keyboards.OnSwitchSecondaryWeapon += OnSwitchSecondaryWeapon;

            mouseInputSensor.Mouses.OnFire -= OnFire;
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            playerMover.Rotate();
        }

        private void OnSwitchPrimaryWeapon()
        {
            currentWeapon = inventory.GetPrimaryWeapon().GetItem() as Weapon;
        }

        private void OnSwitchSecondaryWeapon()
        {
            currentWeapon = inventory.GetSecondaryWeapon().GetItem() as Weapon;
        }

        private void OnSwitchSprintOn()
        {
            playerMover.SwitchSprintOn();
        }

        private void OnSwitchSprintOff()
        {
            playerMover.SwitchSprintOff();
        }

        private void OnMove(Vector3 direction)
        {
            playerMover.Move(direction);
        }

        private void OnFire()
        {
            if ((object)currentWeapon != null)
                currentWeapon.Use();
        }

        private void OnPickup()
        {
            GameObject item = itemSensor.GetItemNearest();
            if ((object)item != null)
            {
                inventory.Add(item);
                item.SetActive(false);
            }
        }

        private void InventoryAction()
        {
            if (!isInventoryOpen)
            {
                StaticInventoryPass.Inventory = inventory;
                activityStack.StartMenu(inventoryMenu);
                isInventoryOpen = true;
            }
            else
            {
                activityStack.StopCurrentMenu();
                isInventoryOpen = false;
            }
        }
    }
}
