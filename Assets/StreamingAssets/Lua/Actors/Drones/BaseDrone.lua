-- Base drone from which all other drones are derived.
local BaseDrone =
{
    _name               = "Base Drone",
    _behaviourTree      = "EnterBuildingTest",
    _maxHP              = 20,
    _attackRange        = 30.0,
    _sightRange         = 15.0,
    _attacksPerSecond   = 1.0
}

return BaseDrone