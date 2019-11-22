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
	void UpdateParticleRotation(int element, float& rotX, float& rotY, float& rotZ, float& rotW, float dt);
	void AddForceAtPoint(float posX, float posY, float posZ, float forceX, float forceY, float forceZ, float element);

	int AddParticle3D(float mass, float boxHeight, float boxLength, float x, float y, float z, float rX, float rY, float rZ, float rW);
	void AddForceToParticle(float forceX, float forceY, float forceZ, int element);
	void ShowMeDaMonie(int (&no)[]);

private:

	vector<Particle3D> particles;

};