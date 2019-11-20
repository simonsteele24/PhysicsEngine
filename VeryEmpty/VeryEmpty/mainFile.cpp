#include "PhysicsWorld.h"
#include "mainFile.h"

PhysicsWorld* _physics;

void CreatePhysicsWorld() 
{
	_physics = new PhysicsWorld();
}

void DestroyPhysicsWorld() 
{
	if (_physics != nullptr) 
	{
		delete(_physics);
	}
}

void UpdatePhysicsWorld(float dt) 
{
	if (_physics != nullptr) 
	{
		//_physics->Update(dt);
	}
}

int GetCollision() 
{
	return 12;
}

void ChangePosition(float& x, float& y, float& z) 
{
	x++;
	y++;
	z++;
}

void UpdateParticle(float& x, float& y, float& z, float dt, int element) 
{
	if (_physics != nullptr) 
	{
		_physics->UpdateParticle(element, x,y,z, dt);
	}
}

void AddForce(float forceX, float forceY, float forceZ, int element) 
{
	if (_physics != nullptr) 
	{
		_physics->AddForceToParticle(forceX, forceY, forceZ, element);
	}
}

int AddParticle(float mass, float x, float y, float z)
{
	if (_physics != nullptr) 
	{
		return _physics->AddParticle3D(mass, 0, 0, x, y, z);
	}
	return 0;
}