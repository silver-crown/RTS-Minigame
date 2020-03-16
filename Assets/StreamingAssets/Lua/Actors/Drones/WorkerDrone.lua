-- work boy
function Update(id)
    if id == nil then
        Debug.Log("WorkerDrone: id was nil")
        return
    end
    local attackRange = LuaManager.Get(id, "_attackRange")
    LuaManager.Set(id, "_attackRange", attackRange + 1)
end

return
{
    _name               = "Worker Drone",
    _behaviourTree      = "Test",
    _maxHP              = 20,
    _attackRange        = 30.0,
    _sightRange         = 15.0,
    _attacksPerSecond   = 1.0,
}