-- -- mxparser documentation can be found here http://mathparser.org/
-- Logistic curve that is high when the resource is near 0 and curves down when it approaches 100
-- Intended for the "Gather Resources" action

return
{
	_utilityFunction = "f(x) = 1/(1+e^(10*(x-0.5)))"
}