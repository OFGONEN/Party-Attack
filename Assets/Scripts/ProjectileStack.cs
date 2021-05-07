using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[CreateAssetMenu( fileName = "ProjectileStack", menuName = "FF/Data/Set/ProjetileStack" )]
public class ProjectileStack : RunTimeStack<Projectile>
{
	public Projectile prefab;
}
