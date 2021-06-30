using UnityEngine;

[
    CreateAssetMenu(
        fileName = "PlayerStats",
        menuName = "ScriptableObjects/PlayerStats",
        order = 2)
]
public class PlayerStats : ScriptableObject
{
   public int playerID;

   public string playerName;

   public Color playerAccent;

   public Sprite playerAvatar;

   public RuntimeAnimatorController animatorController;

   public PlayerInventory inventory;

   public int coins;

   public int completedSwabTests;
}
