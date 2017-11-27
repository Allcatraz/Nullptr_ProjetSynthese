using System.IO;
using UnityEngine;

namespace ProjetSynthese
{
    //BEN_REVIEW : Une petite classe qui ne fait qu'une seule chose...
    //
    //             C'est super!
    
    public class ActionKey : GameScript
    {
        public static ActionKey Instance;

        //BEN_REVIEW : Information en double. Pourquoi est-ce que vos
        //             properties n'utilisent pas directement ce qui se trouve
        //             dans l'attribut "data" ? Ce faisant, la fonction "SetKeyAt"
        //             et "GetKeyAt" deviennent inutiles.
        
        public KeyCode MoveFoward { get; set; }
        public KeyCode MoveBackward { get; set; }
        public KeyCode MoveLeft { get; set; }
        public KeyCode MoveRight { get; set; }

        public KeyCode ToggleInventory { get; set; }
        public KeyCode ToggleMap { get; set; }
        public KeyCode TogglePause { get; set; }

        public KeyCode ToggleSprint { get; set; }

        public KeyCode SwitchToPrimaryWeapon { get; set; }
        public KeyCode SwitchToSecondaryWeapon { get; set; }
        public KeyCode SwitchToThirdWeapon { get; set; }

        public KeyCode Interact { get; set; }
        public KeyCode Reload { get; set; }
        public KeyCode ChangeViewMode { get; set; }

        public KeyCode Fire { get; set; }

        public KeyCode ChangeWeaponSlot { get; set; }
        public KeyCode DropItemTrigger { get; set; }

        private KeyData data;
        private string keyDataFile = "/data.json";

        private void Start()
        {
            Instance = this;
            data = new KeyData();
            LoadKeyData();
        }

        private void OnDestroy()
        {
            SaveKeyData();
        }

        private void LoadKeyData()
        {
            //BEN_CORRECTION : Erreur de logique qui causera plantage.
            //
            //                 StreamingAssets est pas toujours acessible en lecture,
            //                 surtout si les données du jeu se trouvent dans
            //                 "Program Files".
            //
            //                 Utilisez "persistentDataPath" à la place. C'est plus 
            //                 sûr.
            
            //BEN_REVIEW : Utilisez "Path.Combine" pour concaténer des chemins.
            string filePath = Application.streamingAssetsPath + keyDataFile;
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                data = JsonUtility.FromJson<KeyData>(dataAsJson);
                if (data != null)
                {
                    for (int i = 0; i < data.Keys.Length; i++)
                    {
                        SetKeyAt(i + 1, data.Keys[i]);
                    }
                }
            }
            else
            {           
                File.Create(filePath);
            }
        }

        private void SaveKeyData()
        {
            if (data != null)
            {
                data.Keys[0] = MoveFoward;
                data.Keys[1] = MoveBackward;
                data.Keys[2] = MoveLeft;
                data.Keys[3] = MoveRight;
                data.Keys[4] = ToggleInventory;
                data.Keys[5] = ToggleMap;
                data.Keys[6] = TogglePause;
                data.Keys[7] = ToggleSprint;
                data.Keys[8] = SwitchToPrimaryWeapon;
                data.Keys[9] = SwitchToSecondaryWeapon;
                data.Keys[10] = SwitchToThirdWeapon;
                data.Keys[11] = Interact;
                data.Keys[12] = Reload;
                data.Keys[13] = ChangeViewMode;
                data.Keys[14] = Fire;
                data.Keys[15] = ChangeWeaponSlot;
                data.Keys[16] = DropItemTrigger;

                string dataAsJason = JsonUtility.ToJson(data);
                string filePath = Application.streamingAssetsPath + keyDataFile;
                File.WriteAllText(filePath, dataAsJason);
            }
        }

        public void SetKeyAt(int i, KeyCode key)
        {
            switch (i)
            {
                case 1:
                    MoveFoward = key;
                    break;
                case 2:
                    MoveBackward = key;
                    break;
                case 3:
                    MoveLeft = key;
                    break;
                case 4:
                    MoveRight = key;
                    break;
                case 5:
                    ToggleInventory = key;
                    break;
                case 6:
                    ToggleMap = key;
                    break;
                case 7:
                    TogglePause = key;
                    break;
                case 8:
                    ToggleSprint = key;
                    break;
                case 9:
                    SwitchToPrimaryWeapon = key;
                    break;
                case 10:
                    SwitchToSecondaryWeapon = key;
                    break;
                case 11:
                    SwitchToThirdWeapon = key;
                    break;
                case 12:
                    Interact = key;
                    break;
                case 13:
                    Reload = key;
                    break;
                case 14:
                    ChangeViewMode = key;
                    break;
                case 15:
                    Fire = key;
                    break;
                case 16:
                    ChangeWeaponSlot = key;
                    break;
                case 17:
                    DropItemTrigger = key;
                    break;
            }
        }

        public KeyCode GetKeyAt(int i)
        {
            switch (i)
            {
                case 1:
                    return MoveFoward;
                case 2:
                    return MoveBackward;
                case 3:
                    return MoveLeft;
                case 4:
                    return MoveRight;
                case 5:
                    return ToggleInventory;
                case 6:
                    return ToggleMap;
                case 7:
                    return TogglePause;
                case 8:
                    return ToggleSprint;
                case 9:
                    return SwitchToPrimaryWeapon;
                case 10:
                    return SwitchToSecondaryWeapon;
                case 11:
                    return SwitchToThirdWeapon;
                case 12:
                    return Interact;
                case 13:
                    return Reload;
                case 14:
                    return ChangeViewMode;
                case 15:
                    return Fire;
                case 16:
                    return ChangeWeaponSlot;
                case 17:
                    return DropItemTrigger;
                default:
                    return KeyCode.None;
            }
        }
    }
}