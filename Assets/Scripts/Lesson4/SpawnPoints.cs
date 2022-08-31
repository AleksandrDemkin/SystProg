using UnityEngine;
using UnityEngine.Networking;

namespace Lesson4
{
    public class SpawnPoints : NetworkManager
    {
        private void Start()
        {
            playerSpawnMethod = PlayerSpawnMethod.RoundRobin;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (playerSpawnMethod == PlayerSpawnMethod.RoundRobin)
                    playerSpawnMethod = PlayerSpawnMethod.Random;
                
                else playerSpawnMethod = PlayerSpawnMethod.RoundRobin;
            }
        }
    }
}