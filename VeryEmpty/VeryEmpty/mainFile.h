#pragma once 

extern "C" 
{
	// Physics World function
	_declspec(dllexport) void CreatePhysicsWorld();
	_declspec(dllexport) void DestroyPhysicsWorld();

	// Particle functions
	_declspec(dllexport) void UpdateParticle(float& x, float& y, float& z, float dt, int element);
	_declspec(dllexport) int AddParticle(float mass, float x, float y, float z, float rX, float rY, float rZ, float rW);

	// Force functions
	_declspec(dllexport) void AddForce(float forceX, float forceY, float forceZ, int element);
}