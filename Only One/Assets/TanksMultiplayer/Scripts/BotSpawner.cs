/*  This file is part of the "Tanks Multiplayer" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using System.Collections;
using UnityEngine;
using Photon;

namespace TanksMP
{          
    /// <summary>
    /// Responsible for spawning AI bots when in offline mode, otherwise gets disabled.
    /// </summary>
	public class BotSpawner : PunBehaviour
    {                
        /// <summary>
        /// Amount of bots to spawn across all teams.
        /// </summary>
        public int maxBots;
        
        /// <summary>
        /// Selection of bot prefabs to choose from.
        /// </summary>
        public GameObject[] prefabs;
        
        
        void Awake()
        {
            //disabled when not in offline mode
            if ((NetworkMode)PlayerPrefs.GetInt(PrefsKeys.networkMode) != NetworkMode.Offline)
                this.enabled = false;
        }


        IEnumerator Start()
        {
            //wait a second for all script to initialize
            yield return new WaitForSeconds(1);

            GameManager.Left = Random.Range(6, 10);

            //loop over bot count
			for(int i = 0; i < GameManager.Left; i++)
            {
                //randomly choose bot from array of bot prefabs
                //spawn bot across the simulated private network
                int randIndex = Random.Range(0, prefabs.Length);
                GameObject obj = PhotonNetwork.Instantiate(prefabs[randIndex].name, Vector3.zero, Quaternion.identity, 0);

                //let the local host determine the team assignment
                Player p = obj.GetComponent<Player>();
                p.GetView().SetTeam(1);

                //increase corresponding team size
                PhotonNetwork.room.AddSize(p.GetView().GetTeam(), +1);

                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
