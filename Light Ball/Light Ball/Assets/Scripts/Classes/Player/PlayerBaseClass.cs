using UnityEngine;
using System.Collections;

public class PlayerBaseClass : MonoBehaviour, I_TakeDamage
{

	//class to hold player stats
	public class PlayerStats
	{
		public int health;
		public int ammo;

		//constructor to initialise above variables
		public PlayerStats(int _health, int _ammo)
		{
			health = _health;
			ammo = _ammo;
		}
	}

	//initialise class and variables with values
	PlayerStats stats = new PlayerStats (100, 100);

	//controller is assigned here so its easier to assign and not stored in class
	public int controller;

	//take damage interace 
	public void TakeDamage(int _damage, GameObject _sender)
	{
	}

}
