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

	Vector3 Cross(const Vector3& other) 
	{
		return Vector3();
	}

	Vector3 Subtract(const Vector3& other) 
	{
		Vector3 temp = Vector3();
		temp.Set(x - other.x, y - other.y, z - other.z);
		return other;
	}

	Vector3 Add(const Vector3 & other) 
	{
		Vector3 temp = Vector3();
		temp.x += other.x;
		temp.y += other.y;
		temp.z += other.z;
		return temp;
	}

	Vector3 Multiply(const float & other) 
	{
		Vector3 temp = Vector3();
		temp.x *= other;
		temp.y *= other;
		temp.z *= other;
		return temp;
	}

};

struct Quaternion
{
	float x;
	float y;
	float z;
	float w;

	void Set(float _x, float _y, float _z, float _w)
	{
		x = _x;
		y = _y;
		z = _z;
		w = _w;
	}

	Quaternion Dot(const Quaternion & other) 
	{
		Quaternion temp = Quaternion();
		temp.x = other.x * x;
		temp.y = other.y * y;
		temp.z = other.z * z;
		temp.w = other.w * w;
		return temp;
	}

	Quaternion Multiply(const float & other) 
	{
		Quaternion temp = Quaternion();
		temp.x = other * x;
		temp.y = other * y;
		temp.z = other * z;
		temp.w = other * w;
		return temp;
	}

	Quaternion Subtract(const Quaternion & other) 
	{
		Quaternion temp = Quaternion();
		temp.x = x - other.x;
		temp.y = y - other.y;
		temp.z = z - other.z;
		temp.w = w - other.w;
		return temp;
	}

	Quaternion Add(const Quaternion & other)
	{
		Quaternion temp = Quaternion();
		temp.x = other.x + x;
		temp.y = other.y + y;
		temp.z = other.z + z;
		temp.w = other.w + w;
		return temp;
	}

};

class Particle3D
{
public:
	Particle3D();
	Particle3D(float mass, float x, float y, float z, float rX, float rY, float rZ, float rW);
	~Particle3D();

	void UpdateParticle(float dt, float &x, float &y, float &z);
	void UpdateParticleRotation(float& rotX, float& rotY, float& rotZ, float& rotW, float dt);
	void AddForce(float x, float y, float z);
	void AddForceAtPoint(float posX, float posY, float posZ, float forceX, float forceY, float forceZ);

private:
	void UpdatePosition(float dt);
	void UpdateAcceleration(float dt);
	void UpdateAngularAcceleration();

	Vector3 force;
	Vector3 postion;
	Vector3 acceleration;
	Vector3 velocity;
	Vector3 angularVelocity;
	Vector3 angularAcceleration;
	Vector3 torque;
	Quaternion rotation;
	float mass;
	float invMass;
};