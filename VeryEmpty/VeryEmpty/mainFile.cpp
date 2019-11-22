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

int AddParticle(float mass, float x, float y, float z, float rX	,float rY, float rZ, float rW)
{
	if (_physics != nullptr) 
	{
		return _physics->AddParticle3D(mass, 0, 0, x, y, z, rX, rY, rZ, rW);
	}
	return 0;
}

void yeet(int(&x)[]) 
{
	if (_physics != nullptr) 
	{
		_physics->ShowMeDaMonie(x);
	}
}

void UpdateRotation(float& rotX, float& rotY, float& rotZ, float& rotW, float dt, float element) 
{
	if (_physics != nullptr) 
	{
		_physics->UpdateParticleRotation(element, rotX, rotY, rotZ, rotW, dt);
	}
}

void AddForceAtPoint(float posX, float posY, float posZ, float forceX, float forceY, float forceZ, float element) 
{
	if (_physics != nullptr) 
	{
		_physics->AddForceAtPoint(posX, posY, posZ, forceX, forceY, forceZ, element);
	}
}