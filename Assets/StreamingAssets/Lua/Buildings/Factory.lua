local Factory =
{
    _hp = 300,
    _maxHP = 300
}

function Factory:Start(id)
	InGameDebug.Log("Factory goin' up!")
end

function Factory:Update(id)
	if id == nil then
        Debug.Log("Factory: id was nil")
        return
    end
end

return Factory