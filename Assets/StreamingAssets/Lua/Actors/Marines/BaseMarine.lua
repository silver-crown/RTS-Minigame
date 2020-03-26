local BaseMarine =
{
    _hp = 20
}

function BaseMarine:Start(id)

end

function BaseMarine:Update(id, dt)
    local pos = ObjectBuilder.Get(id).transform.position
    pos.x = pos.x + dt * 0
    pos.y = pos.y + dt * 0
    ObjectBuilder.SetPosition(id, pos.x, pos.y)
end

return BaseMarine