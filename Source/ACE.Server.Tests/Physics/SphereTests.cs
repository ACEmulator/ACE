using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Server.Tests.Physics
{
    [TestClass]
    public class SphereTests
    {
        [TestMethod]
        public void Sphere_Intersection()
        {
            var sphere = new Sphere(Vector3.Zero, 5.0f);
            var sphereCollide = new Sphere(new Vector3(5, 5, 5), 5.0f);
            var sphereCloseCall = new Sphere(new Vector3(6, 6, 6), 5.0f);
            var sphereTouching = new Sphere(new Vector3(10, 0, 0), 5.0f);
            var sphereNonCollide = new Sphere(new Vector3(10, 10, 10), 3.0f);
            var sphereInside = new Sphere(new Vector3(1, 1, 1), 1.0f);

            Assert.AreEqual(sphere.Intersects(sphereCollide), true);
            Assert.AreEqual(sphere.Intersects(sphereCloseCall), false);
            Assert.AreEqual(sphere.Intersects(sphereTouching), false);
            Assert.AreEqual(sphere.Intersects(sphereNonCollide), false);
            Assert.AreEqual(sphere.Intersects(sphereInside), true);
        }

        [TestMethod]
        public void Sphere_FindTimeOfCollision()
        {
            var sphere = new Sphere(Vector3.Zero, 10.0f);
            var movement = new Vector3(100, 100, 100);
            var otherSpherePosition = new Vector3(50, 50, 50);
            var radSum = 20.0f;

            var time = sphere.FindTimeOfCollision(movement, otherSpherePosition, radSum);
            Assert.IsTrue(time - 0.38452994616207481f < PhysicsGlobals.EPSILON);

            otherSpherePosition = new Vector3(50, 60, 60);
            time = sphere.FindTimeOfCollision(movement, otherSpherePosition, radSum);
            Assert.IsTrue(time - 0.46125741132772069f < PhysicsGlobals.EPSILON);

            otherSpherePosition = new Vector3(30, 42, 63);
            time = sphere.FindTimeOfCollision(movement, otherSpherePosition, radSum);
            Assert.IsTrue(time == -1.0f);
        }

        [TestMethod]
        public void Sphere_IntersectsRay()
        {
            var sphere = new Sphere(Vector3.Zero, 5.0f);
            var ray = new Ray(new Vector3(-10, -10, -10), new Vector3(20, 20, 20));
            double time = float.MinValue;
            var intersects = sphere.SphereIntersectsRay(ray, out time);
            Assert.IsTrue(intersects && time - 12.320511131409415f < PhysicsGlobals.EPSILON);

            ray = new Ray(new Vector3(20, 25, 30), new Vector3(-20, -25, -30));
            intersects = sphere.SphereIntersectsRay(ray, out time);
            Assert.IsTrue(intersects && time - 38.874800109922887f < PhysicsGlobals.EPSILON);

            // ray starting inside sphere
            // is not considered an intersection by the game
            ray = new Ray(new Vector3(1, 1, 1), new Vector3(-20, -20, -20));
            intersects = sphere.SphereIntersectsRay(ray, out time);
            Assert.IsTrue(!intersects && time == 0);

            // this is not an intersection,
            // but the function returns as a negative #
            ray = new Ray(new Vector3(50, 50, 50), new Vector3(100, 100, 100));
            intersects = sphere.SphereIntersectsRay(ray, out time);
            Assert.IsTrue(intersects && time - -81.602495098501151 < PhysicsGlobals.EPSILON);
        }

        [TestMethod]
        public void Sphere_CollideWithPoint()
        {
            var sphere = new Sphere(Vector3.Zero, 5.0f);

            var obj = new ObjectInfo();
            var path = new SpherePath();
            path.GlobalCurrCenter = new List<Vector3>() { new Vector3(20, 20, 20) };    // represents the movement path
            var collisions = new CollisionInfo();

            // the point we are checking against is represented with this sphere...
            var checkPos = new Sphere(new Vector3(10, 10, 10), 0);

            var disp = Vector3.Zero;
            var radsum = 10.0f;
            var sphereNum = 0;

            var transitionState = sphere.CollideWithPoint(obj, path, collisions, checkPos, disp, radsum, sphereNum);
            Assert.IsTrue(transitionState == TransitionState.Collided);

            obj.State |= ObjectInfoState.PerfectClip;
            transitionState = sphere.CollideWithPoint(obj, path, collisions, checkPos, disp, radsum, sphereNum);
            Assert.IsTrue(transitionState == TransitionState.Collided);

            checkPos.Center = new Vector3(30, 30, 30);
            transitionState = sphere.CollideWithPoint(obj, path, collisions, checkPos, disp, radsum, sphereNum);
            Assert.IsTrue(transitionState == TransitionState.Collided);     // should redirect to location not currently in path

            path.GlobalCurrCenter[0] = new Vector3(1, 1, 1);        // not enough distance to make it this time
            transitionState = sphere.CollideWithPoint(obj, path, collisions, checkPos, disp, radsum, sphereNum);
            Assert.IsTrue(transitionState == TransitionState.Adjusted);
        }
    }
}
