#include "PhysicsWorld.h"

// This function is the constructor for the class
PhysicsWorld::PhysicsWorld()
{
}





// This function is the destructor for the class
PhysicsWorld::~PhysicsWorld()
{
}





// This function updates a particle and gets the new position for the particle
void PhysicsWorld::UpdateParticle(int element, float &x, float &y, float &z, float dt)
{
	particles[element].UpdateParticle(dt, x, y, z);
}





// This function adds a new particle 3D to the array
int PhysicsWorld::AddParticle3D(float mass, float boxHeight, float boxLength, float x, float y, float z, float rX ,float rY,float rZ, float rW)
{
	particles.push_back(Particle3D(mass,x,y,z,rX,rY,rZ,rW));
	int particleSize = static_cast<int>(particles.size()) - 1;
	return particleSize;
}





// This function adds a force to a specific particle 
void PhysicsWorld::AddForceToParticle(float forceX, float forceY, float forceZ, int element) 
{
	particles[element].AddForce(forceX, forceY, forceZ);
}