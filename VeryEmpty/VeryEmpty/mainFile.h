#pragma once 

extern "C" 
{


	_declspec(dllexport) void CreatePhysicsWorld();
	_declspec(dllexport) void DestroyPhysicsWorld();
	_declspec(dllexport) void UpdatePhysicsWorld(float dt);
	_declspec(dllexport) int GetCollision();
	_declspec(dllexport) void ChangePosition(float& x, float& y, float& z);
	_declspec(dllexport) void UpdateParticle(float& x, float& y, float& z, float dt, int element);
	_declspec(dllexport) void AddForce(float forceX, float forceY, float forceZ, int element);
	_declspec(dllexport) int AddParticle(float mass,float x, float y, float z);


}