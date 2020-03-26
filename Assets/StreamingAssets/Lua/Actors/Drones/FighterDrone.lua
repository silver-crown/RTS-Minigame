-- Combat Drone
function Start(id)
    LuaManager.AddOnValueSetListener(id, "_sightRange", function(value)
        Console.Log("_sightRange changed ;)")
        Console.Log(value)
    end)
    LuaManager.Set(id, "_sightRange", 420.69)
end

function Update()
    return "hello"
end

return
{
    _name               = "Combat Drone",
    _behaviourTree      = "CombatDroneBT",
    _maxHP              = 20,
    _attackRange        = 30.0,
    _sightRange         = 35.0,
    _fireRate           = 1.0,
    _damage             = 2.0,
}