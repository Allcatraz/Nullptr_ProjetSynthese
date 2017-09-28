using System.Runtime.InteropServices;
using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/PlayerController")]
    public class PlayerController : GameScript
    {
        private ActivityStack activityStack;
        private Health health;
        private KeyboardInputSensor keyboardInputSensor;
        private MouseInputSensor mouseInputSensor;
        private PlayerMover playerMover;
        private Inventory inventory;
        private ItemSensor itemSensor;

        private Weapon currentWeapon;

        private bool isInventoryOpen = false;

        private void Awake()
        {
            playerMover = GetComponentInChildren<PlayerMover>();
            keyboardInputSensor = GetComponent<KeyboardInputSensor>();
            mouseInputSensor = GetComponent<MouseInputSensor>();

            keyboardInputSensor.Keyboards.OnMove += OnMove;
            keyboardInputSensor.Keyboards.OnInventoryAction += InventoryAction;
            keyboardInputSensor.Keyboards.OnPickup += OnPickup;
            keyboardInputSensor.Keyboards.OnSwitchSprintOn += OnSwitchSprintOn;
            keyboardInputSensor.Keyboards.OnSwitchSprintOff += OnSwitchSprintOff;

            mouseInputSensor.Mouses.OnFire += OnFire;

            Camera.main.GetComponent<CameraController>().PlayerToFollow = gameObject;
        }

        private void OnDestroy()
        {
            keyboardInputSensor.Keyboards.OnMove -= OnMove;
            keyboardInputSensor.Keyboards.OnInventoryAction -= InventoryAction;
            keyboardInputSensor.Keyboards.OnPickup -= OnPickup;
            keyboardInputSensor.Keyboards.OnSwitchSprintOn -= OnSwitchSprintOn;
            keyboardInputSensor.Keyboards.OnSwitchSprintOff -= OnSwitchSprintOff;

            mouseInputSensor.Mouses.OnFire -= OnFire;
        }

        private void Update()
        {
            playerMover.Rotate();
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
            if ((object) item != null)
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
                //activityStack.StartMenu(inventoryMenu);
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
