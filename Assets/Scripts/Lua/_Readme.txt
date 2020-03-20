-- The following uses Benjamin's LUA Documentation Format (bldf? bluadoc?)

ObjectBuilder
    -- Info --
    Provides methods for building and modifying GameObjects from Lua.
    -- Methods --
    ObjectBuilder.Create()
        summary:    Creates an empty GameObject.
        returns:    The id of the created GameObject. Number.
    ObjectBuilder.Create(prefab)
        summary:    Creates a GameObject modelled after a prefab.
        prefab:     The name of the prefab to use.      type: String.
        returns:    The id of the created GameObject.   type: Number.
    ObjectBuilder.AddComponent(id, type, parameters)
        summary:    Adds a component to a GameObject.
        id:         The id of the GameObject.   type: Number.
        type:       The component type.         type: String, see component types.
        parameters: The component parameters.   type: Table, see component types.
    ObjectBuilder.LogComponent(id, type)
        summary:    Logs information about a GameObject's component to the console.
        id:         The id of the GameObject.   type: Number.
        type:       The component type.         type: String, see component types.
    ObjectBuilder.SetName(id, name)
        summary:    Sets a GameObject's name.
        id:         The id of the GameObject.   type: Number.
        name:       The GameObject's new name.  type: String.
    -- Misc --
    Component Types:
        Miner
            Lets the GameObject mine resources.
        Inventory
            Lets the GameObject store in-game items.
            parameters:
                capacity: How much the Inventory can store. type: Number.

LuaManager
    -- Info --
    Provides methods for creating and manipulating LuaObjects.
    A LuaObject is a helper object that associates a Lua table with an id and a script.
    -- Methods --
    LuaManager.CreateLuaObject(path)
        summary:    Creates a LuaObject.
        path:       The path of the Lua file to use.    type: String.
        return:     The id of the LuaObject.            type: Number.
    LuaManager.Set(id, key, value)
        summary:    Sets a table value in a LuaObject. 
        key:        The key of the value we want to set.    type: String.
        value:      The value to associate with the key.    type: String.

Console (alias InGameDebug)
    -- Info --
    Provides in-game debugging capabilities.
    -- Methods --
    Console.Log(message)
        summary:    Logs a message to the console (in-game and Unity editor).
        message:    The message to log.     type: String.
Done
    -- Info --
    Provides methods for creating new drones.
    -- Methods --
    Drone.Create(type, x, y, z)
        summary:    Creates a drone.
        type:       The type of the drone (lua file relative to StreamingAssets/Scripts).   String.
        x:          The drone's x position.                                                 Number.
        y:          The drone's y position.                                                 Number.
        z:          The drone's z position.                                                 Number.
        returns:    The drone's id.                                                         Number.
    Drone.Create(count, type, x, y, z)
        summary:    Creates multiple drones.
        count:      The number of drones to create.                                         Number.
        type:       The type of the drone (lua file relative to StreamingAssets/Scripts).   String.
        x:          The drone's x position.                                                 Number.
        y:          The drone's y position.                                                 Number.
        z:          The drone's z position.                                                 Number.
        returns:    The drone's id.                                                         Number.