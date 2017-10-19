using Harmony;
using UnityEngine;
using UnityEngine.Networking;

namespace ProjetSynthese
{
    [AddComponentMenu("Game/Control/PlayerController")]
    public class PlayerController : NetworkGameScript
    {
        [SerializeField] private Menu inventoryMenu;
        [SerializeField] private Menu mapMenu;
        [SerializeField] private Transform weaponHolderTransform;
        [SerializeField] private Transform inventoryTransform;
        [SerializeField] private Camera firstPersonCamera;

        private ActivityStack activityStack;
        private Health health;
        private KeyboardInputSensor keyboardInputSensor;
        private MouseInputSensor mouseInputSensor;
        private PlayerMover playerMover;
        private Inventory inventory;
        private ItemSensor itemSensor;
        private Weapon currentWeapon;
        private DeathCircleHurtEventChannel deathCircleHurtEventChannel;
        private SoldierAnimatorUpdater soldierAnimatorUpdater;

        private Vector2 rotation;
        private bool isInventoryOpen = false;
        private bool isMapOpen = false;
        private bool isFirstPerson = false;

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
                                            [EntityScope] ItemSensor itemSensor,
                                            [EventChannelScope] DeathCircleHurtEventChannel deathCircleHurtEventChannel,
                                            [EntityScope] SoldierAnimatorUpdater soldierAnimatorUpdater)
        {
            this.keyboardInputSensor = keyboardInputSensor;
            this.mouseInputSensor = mouseInputSensor;
            this.activityStack = activityStack;
            this.playerMover = playerMover;
            this.health = health;
            this.inventory = inventory;
            this.itemSensor = itemSensor;
            this.deathCircleHurtEventChannel = deathCircleHurtEventChannel;
            this.soldierAnimatorUpdater = soldierAnimatorUpdater;
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
            keyboardInputSensor.Keyboards.OnChangeViewMode += OnChangeViewMode;

            mouseInputSensor.Mouses.OnFire += OnFire;

            health.OnDeath += OnDeath;

            deathCircleHurtEventChannel.OnEventPublished += OnPlayerOutDeathCircle;

            transform.position = new Vector3(0, 0, 0);
            Camera.main.GetComponent<CameraController>().PlayerToFollow = gameObject;
            rotation = new Vector2();

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
            keyboardInputSensor.Keyboards.OnChangeViewMode -= OnChangeViewMode;

            mouseInputSensor.Mouses.OnFire -= OnFire;

            health.OnDeath -= OnDeath;
        }

        private void FixedUpdate()
        {
            if (!isFirstPerson)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseInputSensor.GetPosition());
                Vector3 distance = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y,
                    mousePos.z - transform.position.z);
                float angle = (Mathf.Atan2(distance.x, distance.z) * 180 / Mathf.PI);
                Vector3 vec3Angle = new Vector3(0, angle, 0);
                playerMover.Rotate(vec3Angle);
                distance.y = transform.position.y;
                soldierAnimatorUpdater.ViewDirection = distance;
                soldierAnimatorUpdater.UpdateAnimator();
            }
            else
            {
                rotation.x += -Input.GetAxis("Mouse Y") * 100 * Time.deltaTime;
                rotation.y += Input.GetAxis("Mouse X") * 100 * Time.deltaTime;
                rotation.x = ClampAngle(rotation.x, -60, 60);

                Quaternion localRotation = Quaternion.Euler(rotation.x, rotation.y, 0.0f);
                firstPersonCamera.transform.rotation = localRotation;
                transform.rotation = Quaternion.Euler(0, rotation.y, 0);

            }
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle <= -360F)
            angle += 360F;
            if (angle >= 360F)
            angle -= 360F;
            return Mathf.Clamp(angle, min, max);
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

        private void OnMoveToward(KeyCode key)
        {
            Matrix4x4 transformMatrix = transform.localToWorldMatrix;
            Vector3 direction = Vector3.zero;

            if (key == KeyCode.W)
            {
                direction = transformMatrix.GetColumn(2);
            }
            else if (key == KeyCode.S)
            {
                direction = -transformMatrix.GetColumn(2);
            }
            else if (key == KeyCode.A)
            {
                direction = -transformMatrix.GetColumn(0);
            }
            else if (key == KeyCode.D)
            {
                direction = transformMatrix.GetColumn(0);
            }
            direction.Normalize();
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
            RpcSetItemHolder(item);
        }

        [ClientRpc]
        private void RpcSetItemHolder(GameObject item)
        {
            if (isLocalPlayer)
            {
                if ((object)item != null)
                {
                    item.layer = LayerMask.NameToLayer(R.S.Layer.EquippedItem);
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
            if ((object)currentWeapon != null)
            {
                currentWeapon.Reload();
            }
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }

        private void OnPlayerOutDeathCircle(DeathCircleHurtEvent deathCircleHurtEvent)
        {
            health.Hit(deathCircleHurtEvent.HurtPoints);
        }

        private void OnChangeViewMode()
        {
            isFirstPerson = !isFirstPerson;
            firstPersonCamera.gameObject.SetActive(isFirstPerson);
        }
    }
}
