#pragma once

#include <iostream>
#include <vector>
#include "Particle3D.h"

using namespace std;

class PhysicsWorld
{
public:
	PhysicsWorld();
	~PhysicsWorld();

	void UpdateParticle(int element, float &x, float &y, float &z, float dt);

	int AddParticle3D(float mass, float boxHeight, float boxLength, float x, float y, float z);
	void AddForceToParticle(float forceX, float forceY, float forceZ, int element);

private:

	vector<Particle3D> particles;

};