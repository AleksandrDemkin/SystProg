using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Lesson4
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        private GameObject playerCharacter;

        private void Start()
        {
            SpawnCharacter();
        }

        private void SpawnCharacter()
        {
            if (!isServer)
            {
                return;
            }

            playerCharacter = Instantiate(playerPrefab, transform);
            NetworkServer.SpawnWithClientAuthority(playerCharacter,
                connectionToClient);
        }
    }
}
