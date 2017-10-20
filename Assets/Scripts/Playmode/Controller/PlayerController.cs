using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Castle.Core.Internal;
using Harmony;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    public delegate void UseEventHandler(bool isDoingSomething);

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
        private NetworkIdentity networkIdentity;
        private DeathCircleHurtEventChannel deathCircleHurtEventChannel;
        private SoldierAnimatorUpdater soldierAnimatorUpdater;

        private Vector2 rotation = Vector2.zero;
        private bool isInventoryOpen = false;
        private bool isMapOpen = false;
        private bool isFirstPerson = false;
        private bool canCameraMove = true;
        private bool isDoingSomething = false;

        public UseEventHandler OnUse;

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
                                            [GameObjectScope] NetworkIdentity networkIdentity,
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
            this.networkIdentity = networkIdentity;
            this.deathCircleHurtEventChannel = deathCircleHurtEventChannel;
            this.soldierAnimatorUpdater = soldierAnimatorUpdater;
        }

        private void Start()
        {
            InjectDependencies("InjectPlayerController");

            if (!isLocalPlayer)
            {
                return;
            }

            keyboardInputSensor.Keyboards.OnMoveToward += OnMoveToward;
            keyboardInputSensor.Keyboards.OnToggleInventory += OnToggleInventory;
            keyboardInputSensor.Keyboards.OnInteract += OnInteract;
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
            keyboardInputSensor.Keyboards.OnInteract -= OnInteract;
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

            deathCircleHurtEventChannel.OnEventPublished -= OnPlayerOutDeathCircle;
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            if (canCameraMove)
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
        }

        private float ClampAngle(float angle, float min, float max)
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

            switch (key)
            {
                case KeyCode.W:
                    direction = transformMatrix.GetColumn(2);
                    break;
                case KeyCode.S:
                    direction = -transformMatrix.GetColumn(2);
                    break;
                case KeyCode.A:
                    direction = -transformMatrix.GetColumn(0);
                    break;
                case KeyCode.D:
                    direction = transformMatrix.GetColumn(0);
                    break;
            }

            direction.Normalize();
            playerMover.Move(direction);

            soldierAnimatorUpdater.MouvementDirection = direction;
        }

        private void OnFire()
        {
            if ((object)currentWeapon != null)
                currentWeapon.Use();
        }

        private void OnInteract()
        {
            GameObject item = itemSensor.GetItemNearest();
            CmdTakeItem(item);

            if (item == null)
            {
                isDoingSomething = false;
            }
            else
            {
                isDoingSomething = true;
            }

            if (OnUse != null) OnUse(isDoingSomething);
        }

        [Command]
        private void CmdTakeItem(GameObject item)
        {
            networkIdentity.AssignClientAuthority(connectionToClient);
            RpcTakeItem(item);
            networkIdentity.RemoveClientAuthority(connectionToClient);
        }

        [ClientRpc]
        private void RpcTakeItem(GameObject item)
        {
            if (!isLocalPlayer)
            {
                return;
            }

            if ((object)item != null)
            {
                item.gameObject.layer = LayerMask.NameToLayer(R.S.Layer.EquippedItem);
                List<GameObject> allItems = item.gameObject.GetAllChildrens().ToList();
                allItems.ForEach(obj => obj.layer = LayerMask.NameToLayer(R.S.Layer.EquippedItem));

                inventory.Add(item, gameObject);

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
            if (isInventoryOpen)
            {
                activityStack.StopCurrentMenu();
            }
            else
            {
                activityStack.StartMenu(inventoryMenu);
            }
            isInventoryOpen = !isInventoryOpen;

            canCameraMove = !isInventoryOpen;
            if (isFirstPerson)
            {
                SetCursor(isInventoryOpen, !isInventoryOpen);
            }
        }

        private void OnToggleMap()
        {
            if (isMapOpen)
            {
                activityStack.StopCurrentMenu();
            }
            else
            {
                activityStack.StartMenu(mapMenu);
            }
            isMapOpen = !isMapOpen;

            canCameraMove = !isMapOpen;
            if (isFirstPerson)
            {
                SetCursor(isMapOpen, !isMapOpen);
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
            SetCursor(isFirstPerson, isFirstPerson);
        }

        private void SetCursor(bool isVisible, bool isLock)
        {
            Cursor.visible = isVisible;
            Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
