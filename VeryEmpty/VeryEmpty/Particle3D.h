#pragma once

// This struct is meant to represent a vector 3 
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

	// Constructors
	Particle3D();
	Particle3D(float mass, float x, float y, float z, float rX, float rY, float rZ, float rW);
	
	// Destructors
	~Particle3D();

	// Mutators
	void UpdateParticle(float dt, float &x, float &y, float &z);
	void AddForce(float x, float y, float z);

private:

	// Mutators
	void UpdatePosition(float dt);
	void UpdateAcceleration(float dt);

	// Vector3's
	Vector3 force;
	Vector3 postion;
	Vector3 acceleration;
	Vector3 velocity;

	// Floats
	float mass;
	float invMass;
};