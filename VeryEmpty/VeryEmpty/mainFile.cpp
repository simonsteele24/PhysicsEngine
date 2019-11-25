#include "PhysicsWorld.h"
#include "mainFile.h"

PhysicsWorld* _physics;

// This function creates the physics world entity
void CreatePhysicsWorld() 
{
	_physics = new PhysicsWorld();
}





// This function destroys the physics world entity
void DestroyPhysicsWorld() 
{
	if (_physics != nullptr) 
	{
		delete(_physics);
	}
}





// This function updates a specific particle in physics world
void UpdateParticle(float& x, float& y, float& z, float dt, int element) 
{
	if (_physics != nullptr) 
	{
		_physics->UpdateParticle(element, x,y,z, dt);
	}
}





// This function adds a force to a given particle
void AddForce(float forceX, float forceY, float forceZ, int element) 
{
	if (_physics != nullptr) 
	{
		_physics->AddForceToParticle(forceX, forceY, forceZ, element);
	}
}





// This functions adds a particle and returns an element to the particle
int AddParticle(float mass, float x, float y, float z, float rX	,float rY, float rZ, float rW)
{
	if (_physics != nullptr) 
	{
		return _physics->AddParticle3D(mass, 0, 0, x, y, z, rX, rY, rZ, rW);
	}
	return 0;
}