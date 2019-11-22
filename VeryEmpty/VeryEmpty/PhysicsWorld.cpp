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

int PhysicsWorld::AddParticle3D(float mass, float boxHeight, float boxLength, float x, float y, float z, float rX ,float rY,float rZ, float rW)
{
	particles.push_back(Particle3D(mass,x,y,z,rX,rY,rZ,rW));
	int particleSize = static_cast<int>(particles.size()) - 1;
	return particleSize;
}

void PhysicsWorld::AddForceToParticle(float forceX, float forceY, float forceZ, int element) 
{
	particles[element].AddForce(forceX, forceY, forceZ);
}

void PhysicsWorld::ShowMeDaMonie(int(&no)[]) 
{
	no[0] += 1;
}

void PhysicsWorld::UpdateParticleRotation(int element, float& rotX, float& rotY, float& rotZ, float& rotW, float dt)
{
	particles[element].UpdateParticleRotation(rotX, rotY, rotZ, rotW, dt);
}

void PhysicsWorld::AddForceAtPoint(float posX, float posY, float posZ, float forceX, float forceY, float forceZ, float element) 
{
	particles[element].AddForceAtPoint(posX, posY, posZ, forceX, forceY, forceZ);
}