using Harmony;
using UnityEngine;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/PlayerController")]
    public class PlayerController : NetworkGameScript
    {
        [SerializeField] private Menu inventoryMenu;
        [SerializeField] private Menu mapMenu;
        [SerializeField] private Transform weaponHolderTransform;
        [SerializeField] private Transform inventoryTransform;

        private ActivityStack activityStack;
        private Health health;
        private KeyboardInputSensor keyboardInputSensor;
        private MouseInputSensor mouseInputSensor;
        private PlayerMover playerMover;
        private Inventory inventory;
        private ItemSensor itemSensor;

        private Weapon currentWeapon;

        private bool isInventoryOpen = false;
        private bool isMapOpen = false;

        public Transform GetWeaponHolderTransform()
        {
            return weaponHolderTransform;
        }

        public Transform GetInventoryTransform()
        {
            return inventoryTransform;
        }

        public Inventory GetInventory()
        {
            return inventory;
        }

        public Weapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

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

        private void Start()
        {
            if (!isLocalPlayer)
            {
                Destroy(this);
                return;
            }

            InjectDependencies("InjectPlayerController");

            keyboardInputSensor.Keyboards.OnMoveToward += OnMoveToward;
            keyboardInputSensor.Keyboards.OnToggleInventory += OnToggleInventory;
            keyboardInputSensor.Keyboards.OnPickup += OnPickup;
            keyboardInputSensor.Keyboards.OnSwitchSprintOn += OnSwitchSprintOn;
            keyboardInputSensor.Keyboards.OnSwitchSprintOff += OnSwitchSprintOff;
            keyboardInputSensor.Keyboards.OnSwitchPrimaryWeapon += OnSwitchPrimaryWeapon;
            keyboardInputSensor.Keyboards.OnSwitchSecondaryWeapon += OnSwitchSecondaryWeapon;
            keyboardInputSensor.Keyboards.OnSwitchThridWeapon += OnSwitchThirdWeapon;
            keyboardInputSensor.Keyboards.OnToggleMap += OnToggleMap;
            keyboardInputSensor.Keyboards.OnReload += OnReload;

            mouseInputSensor.Mouses.OnFire += OnFire;

            health.OnDeath += OnDeath;

            Camera.main.GetComponent<CameraController>().PlayerToFollow = gameObject;

            inventory.NotifyInventoryChange();
        }

        private void OnDestroy()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            keyboardInputSensor.Keyboards.OnMoveToward -= OnMoveToward;
            keyboardInputSensor.Keyboards.OnToggleInventory -= OnToggleInventory;
            keyboardInputSensor.Keyboards.OnPickup -= OnPickup;
            keyboardInputSensor.Keyboards.OnSwitchSprintOn -= OnSwitchSprintOn;
            keyboardInputSensor.Keyboards.OnSwitchSprintOff -= OnSwitchSprintOff;
            keyboardInputSensor.Keyboards.OnSwitchPrimaryWeapon -= OnSwitchPrimaryWeapon;
            keyboardInputSensor.Keyboards.OnSwitchSecondaryWeapon -= OnSwitchSecondaryWeapon;
            keyboardInputSensor.Keyboards.OnSwitchThridWeapon -= OnSwitchThirdWeapon;
            keyboardInputSensor.Keyboards.OnToggleMap -= OnToggleMap;
            keyboardInputSensor.Keyboards.OnReload -= OnReload;

            mouseInputSensor.Mouses.OnFire -= OnFire;

            health.OnDeath -= OnDeath;
        }

        private void FixedUpdate()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseInputSensor.GetPosition());
            Vector3 distance = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, mousePos.z - transform.position.z);
            float angle = (Mathf.Atan2(distance.x, distance.z) * 180 / Mathf.PI);

            playerMover.Rotate(angle);
        }

        private void OnSwitchPrimaryWeapon()
        {
            SetCurrentWeaponActive(false);
            Cell weapon = inventory.GetPrimaryWeapon();
            currentWeapon = weapon == null ? null : weapon.GetItem() as Weapon;
            SetCurrentWeaponActive(true);
            inventory.NotifyInventoryChange();
        }

        private void OnSwitchSecondaryWeapon()
        {
            SetCurrentWeaponActive(false);
            Cell weapon = inventory.GetSecondaryWeapon();
            currentWeapon = weapon == null ? null : weapon.GetItem() as Weapon;
            SetCurrentWeaponActive(true);
            inventory.NotifyInventoryChange();
        }

        private void SetCurrentWeaponActive(bool isActive)
        {
            if ((object)currentWeapon != null)
            { 
                currentWeapon.gameObject.SetActive(isActive);
                currentWeapon.transform.position = weaponHolderTransform.position;
                currentWeapon.transform.rotation = weaponHolderTransform.rotation;
                currentWeapon.transform.Rotate(90, 0, 0);
            }
        }

        private void OnSwitchThirdWeapon()
        {
            //Take the grenade
            //currentWeapon = inventory.GetThirdWeapon().GetItem() as Weapon;
        }

        private void OnSwitchSprintOn()
        {
            playerMover.SwitchSprintOn();
        }

        private void OnSwitchSprintOff()
        {
            playerMover.SwitchSprintOff();
        }

        private void OnMoveToward(Vector3 direction)
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

                if (item.GetComponent<Item>() is Weapon)
                {
                    item.transform.SetParent(weaponHolderTransform);
                }
                else
                {
                    item.transform.SetParent(inventoryTransform);
                }

                item.SetActive(false);
            }
        }

        private void OnToggleInventory()
        {
            if (!isInventoryOpen)
            {
                activityStack.StartMenu(inventoryMenu);
                isInventoryOpen = true;
            }
            else
            {
                activityStack.StopCurrentMenu();
                isInventoryOpen = false;
            }
        }

        private void OnToggleMap()
        {
            if (!isMapOpen)
            {
                activityStack.StartMenu(mapMenu);
                isMapOpen = true;
            }
            else
            {
                activityStack.StopCurrentMenu();
                isMapOpen = false;
            }
        }

        private void OnReload()
        {
            if((object)currentWeapon != null)
            {
                currentWeapon.Reload();
            }
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
