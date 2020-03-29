local ScoutDrone = 
{
    -- general drone stats
    _name               = "Scout",
    _behaviourTree      = "ScoutBehaviour",
    _maxHP              = 10,
    _attackRange        = 10.0,
    _sightRange         = 60.0,
    _attacksPerSecond   = 1.0,

    -- scout stats
    _lastTimeChunkWasScouted = {},
}

function ScoutDrone:Start(id)
end

function ScoutDrone:Update(id, dt)
end

return ScoutDrone