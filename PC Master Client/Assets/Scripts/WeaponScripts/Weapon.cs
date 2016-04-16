using UnityEngine;
using System.Collections;

public interface Weapon {

	void Fire ();

	int getFireRate ();
}

public interface Projectile{

	int getDamage ();

	int getLife (); 
}