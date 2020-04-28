using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class SpawnMarine : NetworkBehaviour
{
    [SerializeField]
    public GameObject MarinePrefab;

    // could also run this OnStartAuthority
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.hasAuthority)
        {
            CmdSpawnMarine();
        }
    }

    [Command]
    private void CmdSpawnMarine()
    {
        GameObject marine = Instantiate(MarinePrefab);
        NetworkServer.Spawn(marine, base.connectionToClient);
    }

}
