#pragma once

class PhysicsWorld
{
public:
	PhysicsWorld();
	~PhysicsWorld();

	void Update(float dt);

	void AddParticle3D(float mass, float boxHeight, float boxLength);

private:

};