#pragma once

#include <iostream>
#include <vector>
#include "Particle3D.h"

using namespace std;

class PhysicsWorld
{
public:

	// Constructors
	PhysicsWorld();
	
	// Destructors
	~PhysicsWorld();

	// Mutators
	void UpdateParticle(int element, float &x, float &y, float &z, float dt);
	int AddParticle3D(float mass, float boxHeight, float boxLength, float x, float y, float z, float rX, float rY, float rZ, float rW);
	void AddForceToParticle(float forceX, float forceY, float forceZ, int element);

private:

	// Particle3D vectors
	vector<Particle3D> particles;

};