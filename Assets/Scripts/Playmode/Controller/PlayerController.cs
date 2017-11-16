using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Harmony;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace ProjetSynthese
{
    public delegate void UseEventHandler(bool isDoingSomething);

    public delegate void ChangeModeEventHandler(bool isPlayerInFirstPerson);

    [AddComponentMenu("Game/Control/PlayerController")]
    public class PlayerController : NetworkGameScript, ISwim, IProtection
    {
        [Tooltip("Le menu de l'inventaire du joueur.")]
        [SerializeField]
        private Menu inventoryMenu;

        [Tooltip("Le menu de la map du joueur.")]
        [SerializeField]
        private Menu mapMenu;
        
        [Tooltip("Le transform contenant les armes du joueur.")]
        [SerializeField]
        private Transform weaponHolderTransform;

        [Tooltip("Le transform contenant les items du joueur.")]
        [SerializeField]
        private Transform inventoryHolderTransform;

        [Tooltip("La camera en première personne du joueur")]
        [SerializeField]
        private Camera firstPersonCamera;

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
        private BoostHealEventChannel boostHealEventChannel;
        private PlayerDeathEventChannel playerDeathEventChannel;
        private SoldierAnimatorUpdater soldierAnimatorUpdater;
        private RectTransform endGamePanel;

        private Vector2 rotation = Vector2.zero;
        private bool isInventoryOpen = false;
        private bool isMapOpen = false;
        private bool isFirstPerson = false;
        private bool canCameraMove = true;
        private bool isSwimming = false;

        public bool IsSwimming
        {
            get { return isSwimming; }
            set
            {
                isSwimming = value;
                Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
                if (isSwimming)
                {
                    playerMover.SwitchSwimOn();
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, -2, gameObject.transform.position.z);
                    rigidbody.useGravity = false;
                }
                else
                {
                    playerMover.SwitchSwimOff();
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
                    rigidbody.useGravity = true;
                }
            }
        }

        public event UseEventHandler OnUse;
        public event ChangeModeEventHandler OnChangeMode;

        public Transform GetWeaponHolderTransform()
        {
            return weaponHolderTransform;
        }

        public Transform GetInventoryHolderTransform()
        {
            return inventoryHolderTransform;
        }

        public Inventory GetInventory()
        {
            return inventory;
        }

        public Weapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public void TakeItemFromInventory(GameObject item)
        {
            TakeItem(item);
            CmdTakeItem(item, networkIdentity);
        }

        private void InjectPlayerController([ApplicationScope] KeyboardInputSensor keyboardInputSensor,
                                            [ApplicationScope] MouseInputSensor mouseInputSensor,
                                            [ApplicationScope] ActivityStack activityStack,
                                            [EntityScope] PlayerMover playerMover,
                                            [EntityScope] Health health,
                                            [EntityScope] Inventory inventory,
                                            [EntityScope] ItemSensor itemSensor,
                                            [EntityScope] SoldierAnimatorUpdater soldierAnimatorUpdater,
                                            [GameObjectScope] NetworkIdentity networkIdentity,
                                            [EventChannelScope] DeathCircleHurtEventChannel deathCircleHurtEventChannel,
                                            [EventChannelScope] BoostHealEventChannel boostHealEventChannel,
                                            [EventChannelScope] PlayerDeathEventChannel playerDeathEventChannel)
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
            this.boostHealEventChannel = boostHealEventChannel;
            this.soldierAnimatorUpdater = soldierAnimatorUpdater;
            this.playerDeathEventChannel = playerDeathEventChannel;
        }

        private void Start()
        {
            InjectDependencies("InjectPlayerController");

            if (!isLocalPlayer)
            {
                return;
            }

            endGamePanel = GameObject.FindGameObjectWithTag(R.S.Tag.EndGamePanel).GetAllChildrens()[0].GetComponent<RectTransform>();

            keyboardInputSensor.Keyboards.OnMove += OnMoveToward;
            keyboardInputSensor.Keyboards.OnToggleInventory += OnToggleInventory;
            keyboardInputSensor.Keyboards.OnInteract += OnInteract;
            keyboardInputSensor.Keyboards.OnSwitchSprintOn += playerMover.SwitchSprintOn;
            keyboardInputSensor.Keyboards.OnSwitchSprintOff += playerMover.SwitchSprintOff;
            keyboardInputSensor.Keyboards.OnSwitchPrimaryWeapon += OnSwitchPrimaryWeapon;
            keyboardInputSensor.Keyboards.OnSwitchSecondaryWeapon += OnSwitchSecondaryWeapon;
            keyboardInputSensor.Keyboards.OnSwitchThridWeapon += OnSwitchThirdWeapon;
            keyboardInputSensor.Keyboards.OnToggleMap += OnToggleMap;
            keyboardInputSensor.Keyboards.OnTogglePause += OnPause;
            keyboardInputSensor.Keyboards.OnReload += OnReload;
            keyboardInputSensor.Keyboards.OnChangeViewMode += OnChangeViewMode;

            mouseInputSensor.Mouses.OnFire += OnFire;

            playerDeathEventChannel.OnEventPublished += OnDeath;
            deathCircleHurtEventChannel.OnEventPublished += OnPlayerOutDeathCircle;
            boostHealEventChannel.OnEventPublished += OnBoostHeal;

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

            keyboardInputSensor.Keyboards.OnMove -= OnMoveToward;
            keyboardInputSensor.Keyboards.OnToggleInventory -= OnToggleInventory;
            keyboardInputSensor.Keyboards.OnInteract -= OnInteract;
            keyboardInputSensor.Keyboards.OnSwitchSprintOn -= playerMover.SwitchSprintOn;
            keyboardInputSensor.Keyboards.OnSwitchSprintOff -= playerMover.SwitchSprintOff;
            keyboardInputSensor.Keyboards.OnSwitchPrimaryWeapon -= OnSwitchPrimaryWeapon;
            keyboardInputSensor.Keyboards.OnSwitchSecondaryWeapon -= OnSwitchSecondaryWeapon;
            keyboardInputSensor.Keyboards.OnSwitchThridWeapon -= OnSwitchThirdWeapon;
            keyboardInputSensor.Keyboards.OnToggleMap -= OnToggleMap;
            keyboardInputSensor.Keyboards.OnTogglePause -= OnPause;
            keyboardInputSensor.Keyboards.OnReload -= OnReload;
            keyboardInputSensor.Keyboards.OnChangeViewMode -= OnChangeViewMode;

            mouseInputSensor.Mouses.OnFire -= OnFire;

            playerDeathEventChannel.OnEventPublished -= OnDeath;
            deathCircleHurtEventChannel.OnEventPublished -= OnPlayerOutDeathCircle;
            boostHealEventChannel.OnEventPublished -= OnBoostHeal;
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
                }
                else
                {
                    rotation.x += -Input.GetAxis("Mouse Y") * 100 * Time.deltaTime;
                    rotation.y += Input.GetAxis("Mouse X") * 100 * Time.deltaTime;
                    rotation.x = Maths.ClampAngle(rotation.x, -60, 60);

                    Quaternion localRotation = Quaternion.Euler(rotation.x, rotation.y, 0.0f);
                    firstPersonCamera.transform.rotation = localRotation;
                    transform.rotation = Quaternion.Euler(0, rotation.y, 0);

                    soldierAnimatorUpdater.ViewDirection = transform.localToWorldMatrix.GetColumn(2);
                }
            }
            soldierAnimatorUpdater.UpdateAnimator();
        }

        private void OnSwitchPrimaryWeapon()
        {
            SetCurrentWeaponActive(false);
            ObjectContainedInventory weapon = inventory.GetPrimaryWeapon();
            currentWeapon = weapon == null ? null : weapon.GetItem() as Weapon;
            SetCurrentWeaponActive(true);
            inventory.NotifyInventoryChange();
        }

        private void OnSwitchSecondaryWeapon()
        {
            SetCurrentWeaponActive(false);
            ObjectContainedInventory weapon = inventory.GetSecondaryWeapon();
            currentWeapon = weapon == null ? null : weapon.GetItem() as Weapon;
            SetCurrentWeaponActive(true);
            inventory.NotifyInventoryChange();
        }

        private void OnSwitchThirdWeapon()
        {
        }

        private void SetCurrentWeaponActive(bool isActive)
        {
            if ((object) currentWeapon != null)
            {
                currentWeapon.gameObject.SetActive(isActive);
                currentWeapon.transform.position = weaponHolderTransform.position;
                currentWeapon.transform.rotation = weaponHolderTransform.rotation;
                currentWeapon.transform.Rotate(93, 0, 0);
            }
        }

        private void OnMoveToward(List<KeyCode> key)
        {
            Matrix4x4 transformMatrix = transform.localToWorldMatrix;
            Vector3 direction = Vector3.zero;

            if (isFirstPerson)
            {
                //transformMatrix.GetColumn(3) : Colonne ayant les données du vecteur "Position"
                //transformMatrix.GetColumn(2) : Colonne ayant les données du vecteur "Foward"
                //transformMatrix.GetColumn(1) : Colonne ayant les données du vecteur "Up"
                //transformMatrix.GetColumn(0) : Colonne ayant les données du vecteur "Right"
                if (ListExtension.GetLastIndex(key) == ActionKey.Instance.MoveFoward)
                {
                    direction = transformMatrix.GetColumn(2);                   
                }
                else if (ListExtension.GetLastIndex(key) == ActionKey.Instance.MoveBackward)
                {
                    direction = -transformMatrix.GetColumn(2);
                }
                else if (ListExtension.GetLastIndex(key) == ActionKey.Instance.MoveLeft)
                {
                    direction = -transformMatrix.GetColumn(0);
                }
                else if (ListExtension.GetLastIndex(key) == ActionKey.Instance.MoveRight)
                {
                    direction = transformMatrix.GetColumn(0);
                }               
            }
            else
            {
                foreach (KeyCode k in key)
                {
                    if (k == ActionKey.Instance.MoveFoward)
                    {
                        direction += Vector3.forward;
                    }
                    else if (k == ActionKey.Instance.MoveBackward)
                    {
                        direction += Vector3.back;
                    }
                    else if (k == ActionKey.Instance.MoveLeft)
                    {
                        direction += Vector3.left;
                    }
                    else if (k == ActionKey.Instance.MoveRight)
                    {
                        direction += Vector3.right;
                    }
                }
            }

            direction.Normalize();
            playerMover.Move(direction);

            soldierAnimatorUpdater.MouvementDirection = direction;
        }

        private void OnFire()
        {
            if ((object) currentWeapon != null)
            {
                soldierAnimatorUpdater.Shoot();
                currentWeapon.Use();
            }
        }

        private void OnInteract()
        {
            GameObject item = itemSensor.GetItemNearest();
            TakeItem(item);
            CmdTakeItem(item, networkIdentity);

            if (OnUse != null) OnUse(true);
        }

        [Command]
        private void CmdTakeItem(GameObject item, NetworkIdentity identity)
        {
            RpcTakeItem(item, identity);
        }

        [ClientRpc]
        private void RpcTakeItem(GameObject item, NetworkIdentity identity)
        {
            if ((object) item != null)
            {
                item.gameObject.layer = LayerMask.NameToLayer(R.S.Layer.EquippedItem);
                List<GameObject> allItems = item.gameObject.GetAllChildrens().ToList();
                allItems.ForEach(obj => obj.layer = LayerMask.NameToLayer(R.S.Layer.EquippedItem));

                if (item.GetComponent<Item>() is Weapon)
                {
                    item.transform.SetParent(identity.GetComponent<PlayerController>().GetWeaponHolderTransform());
                }
                else
                {
                    item.transform.SetParent(identity.GetComponent<PlayerController>().GetInventoryHolderTransform());
                }

                item.SetActive(false);
            }
        }

        private void TakeItem(GameObject item)
        {
            if ((object) item != null)
            {
                int layer = LayerMask.NameToLayer(R.S.Layer.EquippedItem);
                item.gameObject.layer = layer;
                List<GameObject> allItems = item.gameObject.GetAllChildrens().ToList();
                allItems.ForEach(obj => obj.layer = layer);

                inventory.Add(item, gameObject);

                if (item.GetComponent<Item>() is Weapon)
                {
                    item.transform.SetParent(weaponHolderTransform);
                }
                else
                {
                    item.transform.SetParent(inventoryHolderTransform);
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
            if ((object) currentWeapon != null)
            {
                currentWeapon.Reload(inventory);
            }
        }

        private void OnDeath(PlayerDeathEvent playerDeathEvent)
        {
            endGamePanel.gameObject.SetActive(true);
            CmdDestroy(gameObject);
            Destroy(gameObject);            
        }

        private void OnPlayerOutDeathCircle(DeathCircleHurtEvent deathCircleHurtEvent)
        {
            health.Hit(deathCircleHurtEvent.HurtPoints);
        }

        private void OnChangeViewMode()
        {
            isFirstPerson = !isFirstPerson;

            if (OnChangeMode != null) OnChangeMode(isFirstPerson);
            firstPersonCamera.gameObject.SetActive(isFirstPerson);
            SetCursor(!isFirstPerson, isFirstPerson);
        }

        private void SetCursor(bool isVisible, bool isLock)
        {
            Cursor.visible = isVisible;
            Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void OnBoostHeal(BoostHealEvent boostHealEvent)
        {
            health.Heal(boostHealEvent.HealthPoints);
        }

        public Item[] GetInventoryProtection()
        {
            ObjectContainedInventory helmet = inventory.GetHelmet();
            ObjectContainedInventory vest = inventory.GetVest();
            return new[] {helmet == null ? null : vest.GetItem(), vest == null ? null : vest.GetItem()};
        }

        private void OnPause()
        {
        }
    }
}