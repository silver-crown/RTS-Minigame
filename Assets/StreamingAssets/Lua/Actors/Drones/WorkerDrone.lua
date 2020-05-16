local WorkerDrone = 
{
    _name               = "Worker Drone",
    _behaviourTree      = "Mine",
    _status				= "Idle",
    _maxHP              = 20,
    _attackRange        = 30.0,
    _sightRange         = 15.0,
    _attacksPerSecond   = 1.0,
    _miningRange 		= 5.0,
    _miningCooldown 	= 3.0,
    _miningDamage		= 5,
    _inventorySpace     = 50,
    _metalCost 			= 20,
    _crystalCost 		= 5
}

function WorkerDrone:Start(id)
    DroneStaticMethods.AddInventoryComponent(id, 100)
    DroneStaticMethods.AddMinerComponent(id)
    InGameDebug.Log("It's time to work baby!!!")
end
function WorkerDrone:Update(id)
    if id == nil then
        Debug.Log("WorkerDrone: id was nil")
        return
    end
    local attackRange = LuaManager.Get(id, "_attackRange")
    LuaManager.Set(id, "_attackRange", attackRange + 1)
end

return WorkerDrone
