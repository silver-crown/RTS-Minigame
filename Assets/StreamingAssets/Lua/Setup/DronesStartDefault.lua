local DronesStartDefault =
{
    _resources = {},
    _drones = {},
}

DronesStartDefault._resources["Crystal"] = 10
DronesStartDefault._resources["Metal"] = 100

DronesStartDefault._drones["BaseDrone"] = 1
DronesStartDefault._drones["WorkerDrone"] = 0
DronesStartDefault._drones["FighterDrone"] = 1
DronesStartDefault._drones["ScoutDrone"] = 0

return DronesStartDefault