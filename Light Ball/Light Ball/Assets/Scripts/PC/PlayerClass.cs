using UnityEngine;
using System.Collections;

public class PlayerClass : Photon.MonoBehaviour {

    public class PlayerData
    {
        public int health;
        public int ammo;
        public int selectedBall;

        public PlayerData(int _health, int _ammo, int _selectedBall)
        {
            health = _health;
            ammo = _ammo;
            selectedBall = _selectedBall;
        }
    }


}
