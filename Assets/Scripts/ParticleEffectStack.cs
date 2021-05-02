using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[CreateAssetMenu( fileName = "ParticleStack", menuName = "FF/Data/Set/ParticleStack" )]
public class ParticleEffectStack : RunTimeStack< ParticleEffect >
{
	public ParticleEffect prefab;
}
