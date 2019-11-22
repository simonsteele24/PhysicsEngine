#include "Particle3D.h"

Particle3D::Particle3D()
{

}

Particle3D::Particle3D(float _mass,float x, float y, float z, float rX, float rY, float rZ, float rW)
{
	mass = _mass;
	invMass = 1.0f / mass;
	force.Set(0, 0, 0);
	postion.Set(x, y, z);
	acceleration.Set(0, 0, 0);
	velocity.Set(0, 0, 0);
	angularVelocity.Set(0, 0, 0);
	angularAcceleration.Set(0, 0, 0);
	torque.Set(0, 0, 0);
	rotation.Set(rX, rY, rZ, rW);
}

Particle3D::~Particle3D()
{
}

void Particle3D::UpdateParticle(float dt, float &x, float &y, float &z) 
{
	UpdatePosition(dt);
	UpdateAcceleration(dt);

	x = postion.x;
	y = postion.y;
	z = postion.z;
}

void Particle3D::UpdateAngularAcceleration() 
{

}

void Particle3D::AddForce(float x, float y, float z) 
{
	force.x += x;
	force.y += y;
	force.z += z;
}

void Particle3D::UpdateAcceleration(float dt) 
{
	// Convert force to acceleration
	acceleration.Set(force.x * invMass, force.y * invMass, force.z * invMass);
	force.Set(0.0f, 0.0f, 0.0f);
}

void Particle3D::UpdatePosition(float dt) 
{
	postion.Set(postion.x + (velocity.x * dt), postion.y + (velocity.y * dt), postion.z + (velocity.z * dt));
	velocity.Set(velocity.x + (acceleration.x * dt), velocity.y + (acceleration.y * dt), velocity.z + (acceleration.z * dt));
}

void Particle3D::UpdateParticleRotation(float& rotX, float& rotY, float& rotZ, float& rotW, float dt) 
{
	Quaternion newRot = Quaternion();
	newRot.Set(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0);

	Quaternion extraTemp = Quaternion();
	extraTemp.Set(0, 0, 0, dt / 2.0f);

	Quaternion temp = rotation.Dot(newRot);
	temp = temp.Dot(extraTemp);

	rotation = rotation.Add(temp);

	angularVelocity = (angularVelocity.Add(angularAcceleration)).Multiply(dt);
}

void Particle3D::AddForceAtPoint(float posX, float posY, float posZ, float forceX, float forceY, float forceZ) 
{

}