#include "Particle3D.h"


// This function is the default constructor for this class
Particle3D::Particle3D()
{

}





// This constructor helps initialize all essential variables for the class
Particle3D::Particle3D(float _mass,float x, float y, float z, float rX, float rY, float rZ, float rW)
{
	mass = _mass;
	invMass = 1.0f / mass;
	force.Set(0, 0, 0);
	postion.Set(x, y, z);
	acceleration.Set(0, 0, 0);
	velocity.Set(0, 0, 0);
}





// This function is the destructor for this class
Particle3D::~Particle3D()
{
}





// This function updates a given particle 
void Particle3D::UpdateParticle(float dt, float &x, float &y, float &z) 
{
	UpdatePosition(dt);
	UpdateAcceleration(dt);

	x = postion.x;
	y = postion.y;
	z = postion.z;
}





// This function adds a force to the particle
void Particle3D::AddForce(float x, float y, float z) 
{
	force.x += x;
	force.y += y;
	force.z += z;
}





// This function updates the acceleration of the particle
void Particle3D::UpdateAcceleration(float dt) 
{
	// Convert force to acceleration
	acceleration.Set(force.x * invMass, force.y * invMass, force.z * invMass);
	force.Set(0.0f, 0.0f, 0.0f);
}





// This function updates the position of the particle
void Particle3D::UpdatePosition(float dt) 
{
	postion.Set(postion.x + (velocity.x * dt), postion.y + (velocity.y * dt), postion.z + (velocity.z * dt));
	velocity.Set(velocity.x + (acceleration.x * dt), velocity.y + (acceleration.y * dt), velocity.z + (acceleration.z * dt));
}