using UnityEngine;
using System.Collections;

public class PlayerClass : Photon.MonoBehaviour {

    public class PlayerData
    {
        public int health;
        public int armour;
        public int ammo;
        public int selectedBall;


        public PlayerData(int _health, int _armour, int _ammo, int _selectedBall)
        {
            health = _health;
            armour = _armour;
            ammo = _ammo;
            selectedBall = _selectedBall;
        }
    }

  }
