-- Do not touch. Touching *will* break the game.
-- Auto-generated by LuaBindingsGenerator.cs. TODO: Document methods.

BBInput = {}
BBInput.get_Profiles = function() end
BBInput.LoadProfiles = function() end
BBInput.Initialize = function() end
BBInput.SetActiveProfile = function(profileName__String) end
BBInput.ActivatePreviousProfile = function() end
BBInput.AddOnAxisPressed = function(axisName__String, action__DynValue, priority__Int32) end
BBInput.AddOnAxisHeld = function(axisName__String, action__DynValue, priority__Int32) end
BBInput.AddOnAxisReleased = function(axisName__String, action__DynValue, priority__Int32) end
BBInput.ClearAllAxesInProfile = function(profileName__String) end
SoundManager = {}
SoundManager.Initialize = function() end
SoundManager.CreateAudioSource = function(clipKey__String) end
SoundManager.Set = function(id__Int32, key__String, value__DynValue) end
SoundManager.PlayMusic = function(id__Int32) end
SoundManager.PlayMusic = function(file__String) end
SoundManager.PlayEffect = function(id__Int32) end
SoundManager.PlayEffect = function(file__String) end
StreamingAssetsDatabase = {}
Assets = StreamingAssetsDatabase
StreamingAssetsDatabase.get_ActiveModules = function() end
StreamingAssetsDatabase.get_OnSoundsDoneLoading = function() end
StreamingAssetsDatabase.set_OnSoundsDoneLoading = function(value__Action) end
StreamingAssetsDatabase.GetDialogue = function(key__String) end
StreamingAssetsDatabase.GetTexture = function(key__String) end
StreamingAssetsDatabase.GetTextures = function(key__String) end
StreamingAssetsDatabase.GetSound = function(key__String) end
StreamingAssetsDatabase.GetDef = function(key__String) end
StreamingAssetsDatabase.GetString = function(key__String) end
StreamingAssetsDatabase.GetStrings = function(key__String) end
StreamingAssetsDatabase.GetNumber = function(key__String) end
StreamingAssetsDatabase.GetSetting = function(key__String) end
StreamingAssetsDatabase.AddSettingToBeChanged = function(key__String, value__String) end
StreamingAssetsDatabase.ApplySettingChanges = function() end
StreamingAssetsDatabase.LoadModules = function() end
StreamingAssetsDatabase.LoadActiveModules = function() end
StreamingAssetsDatabase.LoadTexturesFromActiveModules = function() end
StreamingAssetsDatabase.LoadScriptsFromActiveModules = function() end
StreamingAssetsDatabase.LoadScriptsFromDirectory = function(directory__String) end
StreamingAssetsDatabase.LoadDialogueFromActiveModules = function() end
StreamingAssetsDatabase.LoadSoundsFromActiveModules = function() end
StreamingAssetsDatabase.LoadDefsFromActiveModules = function() end
StreamingAssetsDatabase.LoadDefsFromDirectory = function(directory__String) end
StreamingAssetsDatabase.LoadSettings = function() end
StreamingAssetsDatabase.AddOnSettingChangedListener = function(key__String, action__Action`1) end
StreamingAssetsDatabase.AddOnSettingChangedListener = function(key__String, action__DynValue) end
InGameDebug = {}
Console = InGameDebug
InGameDebug.get_LogObjectCreated = function() end
InGameDebug.set_LogObjectCreated = function(value__Boolean) end
InGameDebug.Log = function(message__Object, context__Object) end
InGameDebug.SetTheme = function(key__String) end
InGameDebug.Help = function() end
InGameDebug.Clear = function() end
LuaInput = {}
Input = LuaInput
LuaInput.get_MouseX = function() end
LuaInput.get_MouseY = function() end
LuaInput.get_MouseWorldX = function() end
LuaInput.get_MouseWorldY = function() end
LuaInput.get_DeltaX = function() end
LuaInput.get_DeltaY = function() end
LuaManager = {}
LuaManager.get__onValueSet = function() end
LuaManager.set__onValueSet = function(value__Dictionary`2) end
LuaManager.get_ActiveLuaManager = function() end
LuaManager.set_ActiveLuaManager = function(value__LuaManager) end
LuaManager.get_OnLuaObjectSetUp = function() end
LuaManager.set_OnLuaObjectSetUp = function(value__Action`1) end
LuaManager.get_OnDestroyLuaObject = function() end
LuaManager.set_OnDestroyLuaObject = function(value__Action`1) end
LuaManager.get_GlobalScript = function() end
LuaManager.get_LoadUpdateEachFrame = function() end
LuaManager.set_LoadUpdateEachFrame = function(value__Boolean) end
LuaManager.ReloadScripts = function() end
LuaManager.AddOnValueSetListener = function(id__Int32, key__String, action__Action`1) end
LuaManager.AddOnValueSetListener = function(id__Int32, key__String, action__DynValue) end
LuaManager.Set = function(id__Int32, key__String, value__DynValue) end
LuaManager.Get = function(id__Int32, key__String) end
LuaManager.GetLuaObjectId = function(id__Int32) end
LuaManager.DoString = function(code__String) end
LuaManager.Call = function(function__DynValue) end
LuaManager.Call = function(function__DynValue, args__DynValue[]) end
LuaManager.CreateScript = function() end
ObjectBuilder = {}
ObjectBuilder.Instantiate = function(path__String) end
ObjectBuilder.InstantiateUIElement = function() end
ObjectBuilder.InstantiateUIElement = function(path__String) end
ObjectBuilder.Create = function(x__Single, y__Single) end
ObjectBuilder.GetChildIds = function(id__Int32) end
ObjectBuilder.Get = function(id__Int32) end
ObjectBuilder.GetPosition = function(id__Int32) end
ObjectBuilder.CreateLuaObject = function(type__String) end
ObjectBuilder.AddExternalGameObject = function(gameObject__GameObject) end
ObjectBuilder.GetId = function(gameObject__GameObject) end
ObjectBuilder.Destroy = function(id__Int32) end
ObjectBuilder.SetName = function(id__Int32, name__String) end
ObjectBuilder.AddLuaObjectComponent = function(id__Int32, type__String) end
ObjectBuilder.AddTimedAction = function(id__Int32, interval__Single, action__DynValue) end
ObjectBuilder.SetTexture = function(id__Int32, textureKey__String) end
ObjectBuilder.SetColor = function(id__Int32, r__Int32, g__Int32, b__Int32, a__Int32) end
ObjectBuilder.SetSortingLayer = function(id__Int32, layer__String) end
ObjectBuilder.SetScale = function(id__Int32, x__Single, y__Single, z__Single) end
ObjectBuilder.SetPosition = function(id__Int32, x__Single, y__Single) end
ObjectBuilder.SetPosition = function(id__Int32, x__Single, y__Single, z__Single) end
ObjectBuilder.Translate = function(id__Int32, x__Single, y__Single, z__Single) end
ObjectBuilder.SetParent = function(childId__Int32, parentId__Int32) end
ObjectBuilder.ToggleEnabled = function(id__Int32) end
ObjectBuilder.DontDestroyOnLoad = function(id__Int32) end
UI = {}
UI.Instantiate = function(path__String) end
UI.Instantiate = function(path__String, x__Single, y__Single) end
UI.Create = function(path__String) end
UI.Create = function(path__String, x__Single, y__Single) end
UI.GetWorldCorners = function(id__Int32) end
UI.GetInputField = function(id__Int32) end
UI.CreateLuaObject = function(path__String) end
UI.CreateSlider = function(settingsKey__String) end
UI.SetParent = function(childId__Int32, parentId__Int32) end
UI.SetAnchor = function(id__Int32, presetStr__String) end
UI.SetAnchors = function(id__Int32, minX__Single, maxX__Single, minY__Single, maxY__Single) end
UI.SetPivot = function(id__Int32, presetStr__String) end
UI.SetPivot = function(id__Int32, x__Single, y__Single) end
UI.SetSize = function(id__Int32, x__Single, y__Single) end
UI.SetWidth = function(id__Int32, width__Single) end
UI.SetHeight = function(id__Int32, height__Single) end
UI.SetPosition = function(id__Int32, x__Single, y__Single) end
UI.SetSizeDelta = function(id__Int32, x__Single, y__Single) end
UI.SetText = function(id__Int32, str__String) end
UI.SetTextAlignment = function(id__Int32, anchor__String) end
UI.SetAllTexts = function(id__Int32, str__String) end
UI.SetTextColor = function(id__Int32, r__Int32, g__Int32, b__Int32, a__Int32) end
UI.SetFontSize = function(id__Int32, size__Int32) end
UI.SetFont = function(id__Int32, fontName__String) end
UI.SetImage = function(id__Int32, key__String) end
UI.SetColor = function(id__Int32, r__Int32, g__Int32, b__Int32, a__Int32) end
UI.ForceUpdateCanvases = function() end
UI.AddOnClick = function(id__Int32, function__DynValue) end
UI.SetOnClick = function(id__Int32, function__DynValue) end
UI.ClearOnClick = function(id__Int32) end
UI.AddOnPointerEnter = function(id__Int32, function__DynValue) end
UI.AddOnPointerExit = function(id__Int32, function__DynValue) end
Drone = {}
Drone.get_TargetResourceType = function() end
Drone.get_ID = function() end
Drone.get_CentralIntelligence = function() end
Drone.set_CentralIntelligence = function(value__CentralIntelligence) end
Drone.Awake = function() end
Drone.Start = function() end
Drone.Update = function() end
Drone.Attack = function() end
Drone.Initialize = function(type__String, id__Int32) end
Drone.SetType = function(type__String) end
Drone.ReceiveMessage = function(message__String) end
Drone.SetTargetDepot = function(target__GameObject) end
Drone.CalculatePowerLevel = function() end
Drone.Create = function(type__String, x__Single, y__Single, z__Single) end
UtilityAction = {}
UtilityAction.Invoke = function() end
UtilityAction.AddFactor = function(factor__Func`1) end
UtilityAction.AddFactor = function(factor__DynValue) end
UtilityAction.GetUtility = function() end
UtilityAction.Create = function(action__Action) end
UtilityAction.Create = function(action__DynValue) end
UtilitySelector = {}
UtilitySelector.AddAction = function(action__UtilityAction) end
UtilitySelector.GetBestAction = function() end
UtilitySelector.GetWorstAction = function() end
UtilitySelector.GetRandomAction = function() end
UtilitySelector.GetSortedActions = function() end
UtilitySelector.InvokeBestAction = function() end
UtilitySelector.InvokeWorstAction = function() end
UtilitySelector.InvokeRandomAction = function() end
UtilitySelector.Create = function() end
DroneStaticMethods = {}
DroneStaticMethods.Create = function(type__String, x__Single, y__Single, z__Single) end
DroneStaticMethods.Create = function(count__Int32, type__String, x__Single, y__Single, z__Single) end
RTSLuaManager = {}
RTSLuaManager.CreateScript = function() end
