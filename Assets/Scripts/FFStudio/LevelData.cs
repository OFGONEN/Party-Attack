using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace FFStudio
{
	[ CreateAssetMenu( fileName = "LevelData", menuName = "FF/Data/LevelData" )]
	public class LevelData : ScriptableObject
    {
        [Scene()]
		public int sceneIndex;
		public bool isGroundLevel = true;
		public int humanCount;

		public GameObject cameraPrefab;
		public Material skyboxMaterial;

	}
}
