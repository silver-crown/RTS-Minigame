local Crystal = 
{
    _maxHP = 40,
    _resourceType = "Crystal"
}

function Crystal:Start(id)
	InGameDebug.Log("Crystal resource created")
end

function Crystal:Update(id)
	if id == nil then
		Debug.Log("Crystal: id was nil")
		return
	end
	local hp = LuaManager.Get(id, "_maxHP")
end

return Crystal