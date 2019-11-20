#pragma once 

extern "C" 
{


	_declspec(dllexport) void CreatePhysicsWorld();
	_declspec(dllexport) void DestroyPhysicsWorld();
	_declspec(dllexport) void UpdatePhysicsWorld(float dt);
	_declspec(dllexport) int GetCollision();
	_declspec(dllexport) void ChangePosition(float& x, float& y, float& z);


}