local CombatDrone = 
{
    _name               = "Combat Drone",
    _behaviourTree      = "CombatDroneBT",
    _maxHP              = 20,
    _attackRange        = 30.0,
    _sightRange         = 15.0,
    _fireRate           = 1.0,
}
function CombatDrone:Start(id)
end

-- Combat Drone
function CombatDrone:Update(id, dt)
    return "hello"
end

return CombatDrone