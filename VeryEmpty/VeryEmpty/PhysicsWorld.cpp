#include "PhysicsWorld.h"

PhysicsWorld::PhysicsWorld()
{
}

PhysicsWorld::~PhysicsWorld()
{
}

void PhysicsWorld::UpdateParticle(int element, float &x, float &y, float &z, float dt)
{
	particles[element].UpdateParticle(dt, x, y, z);
}

int PhysicsWorld::AddParticle3D(float mass, float boxHeight, float boxLength, float x, float y, float z)
{
	particles.push_back(Particle3D(mass,x,y,z));
	int particleSize = static_cast<int>(particles.size()) - 1;
	return particleSize;
}

void PhysicsWorld::AddForceToParticle(float forceX, float forceY, float forceZ, int element) 
{
	particles[element].AddForce(forceX, forceY, forceZ);
}