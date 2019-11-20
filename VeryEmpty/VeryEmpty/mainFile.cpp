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
		_physics->Update(dt);
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