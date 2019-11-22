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
	_declspec(dllexport) int AddParticle(float mass,float x, float y, float z, float rX, float rY, float rZ, float rW);
	_declspec(dllexport) void yeet(int(&x)[]);
	_declspec(dllexport) void AddForceAtPoint(float posX, float posY, float posZ, float forceX, float forceY, float forceZ, float element);
	_declspec(dllexport) void UpdateRotation(float& rotX, float& rotY, float& rotZ, float& rotW, float dt, float element);


}