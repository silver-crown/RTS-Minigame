function Start(id)
	InGameDebug.Log("Metal resource created")
end

function Update(id)
	if id == nil then
		Debug.Log("Metal: id was nil")
		return
	end
	local hp = LuaManager.Get(id, "_maxHP")
end

return
{
    _maxHP = 40,
    _resourceType = "Metal"
}