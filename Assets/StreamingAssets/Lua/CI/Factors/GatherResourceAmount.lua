-- mxparser documentation can be found here http://mathparser.org/
-- Logistic curve that is high when the resource is near 0 and curves down when it approaches 100
return 
{
	_utilityFunction = "f(x) = 1/(1+e^((x-50)/10))"
}