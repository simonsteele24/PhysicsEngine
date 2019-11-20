#pragma once

struct Vector3
{
	float x;
	float y;
	float z;

	void Set(float _x, float _y, float _z) 
	{
		x = _x;
		y = _y;
		z = _z;
	}
};

class Particle3D
{
public:
	Particle3D();
	Particle3D(float mass, float x, float y, float z);
	~Particle3D();

	void UpdateParticle(float dt, float &x, float &y, float &z);
	void AddForce(float x, float y, float z);

private:
	void UpdatePosition(float dt);
	void UpdateAcceleration(float dt);

	Vector3 force;
	Vector3 postion;
	Vector3 acceleration;
	Vector3 velocity;
	float mass;
	float invMass;
};