-- Base drone from which all other drones are derived.
local BaseDrone =
{
    _name               = "Base Drone",
    _behaviourTree      = "Test",
    _maxHP              = 20,
    _attackRange        = 30.0,
    _sightRange         = 15.0,
    _attacksPerSecond   = 1.0
}

function BaseDrone:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

return BaseDrone

--[[
----- How to create a new drone -----
1. Create a new file in Lua/Actor/Drones (e.g. "LongRangeDrone.lua")
2. Load in the base drone:
    require("BaseDrone")
3. Use BaseDrone:new and set the values you wish to override.

Example of a drone derived from BaseDrone:
LongRangeDrone.lua
    BaseDrone = require("BaseDrone")
    LongRangeDrone = BaseDrone:new
    {
        _attackRange = 10000,
        _sightRange  = 10000
    }
    return LongRangeDrone

    OR to keep things short for simple subtypes
    return require("BaseDrone"):new
    {
        _attackRange = 10000,
        _sightRange  = 10000
    }

You could now also create a subtype of LongRangeDrone:
WeakLongRangeDrone.lua
    LongRangeDrone = require("LongRangeDrone")
    WeakLongRangeDrone = LongRangeDrone:new
    {
        _maxHP = 5
    }
    return WeakLongRangeDrone;
]]