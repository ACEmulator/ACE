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

            // represents the movement path
            var transition = new Transition();
            transition.SpherePath.GlobalCurrCenter.Add(new Sphere(new Vector3(20, 20, 20), 5.0f));    

            // the point we are checking against is represented with this sphere...
            var checkPos = new Sphere(new Vector3(10, 10, 10), 0);

            var disp = Vector3.Zero;
            var radsum = 10.0f;
            var sphereNum = 0;

            var transitionState = sphere.CollideWithPoint(transition, checkPos, disp, radsum, sphereNum);
            Assert.IsTrue(transitionState == TransitionState.Collided);

            transition.ObjectInfo.State |= ObjectInfoState.PerfectClip;
            transitionState = sphere.CollideWithPoint(transition, checkPos, disp, radsum, sphereNum);
            Assert.IsTrue(transitionState == TransitionState.Collided);

            // should redirect to location not currently in path
            checkPos.Center = new Vector3(30, 30, 30);
            transitionState = sphere.CollideWithPoint(transition, checkPos, disp, radsum, sphereNum);
            Assert.IsTrue(transitionState == TransitionState.Collided);

            // not enough distance to make it this time
            transition.SpherePath.GlobalCurrCenter[0] = new Sphere(new Vector3(1, 1, 1), 5.0f);
            transitionState = sphere.CollideWithPoint(transition, checkPos, disp, radsum, sphereNum);
            Assert.IsTrue(transitionState == TransitionState.Collided);
        }

        [TestMethod]
        public void Sphere_IntersectsSphere()
        {
            var sphere = new Sphere(Vector3.Zero, 5.0f);
            var sphereCollide = new Sphere(new Vector3(1, 1, 1), 5.0f);
            var sphereNonCollide = new Sphere(new Vector3(10, 10, 10), 5.0f);

            // represents the movement path
            var transition = new Transition();
            transition.SpherePath.NumSphere = 1;
            transition.SpherePath.InsertType = InsertType.Placement;
            transition.SpherePath.GlobalSphere.Add(sphereNonCollide);
            transition.SpherePath.GlobalSphere.Add(null);
            transition.SpherePath.GlobalCurrCenter.AddRange(new List<Sphere>() { new Sphere(Vector3.Zero, 0), new Sphere(Vector3.Zero, 0) });

            Sphere_IntersectsSphere_Inner(sphere, transition, sphereCollide, sphereNonCollide);

            // test walkable paths
            transition.SpherePath.CheckWalkable = true;
            transition.SpherePath.InsertType = InsertType.Transition;
            Sphere_IntersectsSphere_Inner(sphere, transition, sphereCollide, sphereNonCollide);

            // test clipped paths
            transition.ObjectInfo.State = ObjectInfoState.PathClipped;
            Sphere_IntersectsSphere_Inner(sphere, transition, sphereCollide, sphereNonCollide, true, false);

            // test step up
            transition.SpherePath.StepUp = true;
            transition.SpherePath.CheckWalkable = false;
            Sphere_IntersectsSphere_Inner(sphere, transition, sphereCollide, sphereNonCollide, true, false);

            // test interpolation
            transition.SpherePath.StepUp = false;
            Sphere_IntersectsSphere_Inner(sphere, transition, sphereCollide, sphereNonCollide, false, true, true);
        }

        public void Sphere_IntersectsSphere_Inner(Sphere sphere, Transition transition, Sphere sphereCollide, Sphere sphereNonCollide, bool firstTest = true, bool secondTest = true, bool interp = false)
        {
            if (firstTest)
            {
                // test non-collision
                transition.SpherePath.NumSphere = 1;
                transition.SpherePath.GlobalSphere[0] = sphereNonCollide;
                SetCenter(transition);
                var transitionState = sphere.IntersectsSphere(transition, false);
                Assert.AreEqual(transitionState, TransitionState.OK);

                // test collision
                transition.SpherePath.GlobalSphere[0] = sphereCollide;
                SetCenter(transition);
                transitionState = sphere.IntersectsSphere(transition, false);
                Assert.AreEqual(transitionState, interp ? TransitionState.OK : TransitionState.Collided);
            }

            if (secondTest)
            {
                // test collision with another sphere
                transition.SpherePath.GlobalSphere[0] = sphereNonCollide;
                transition.SpherePath.GlobalSphere[1] = sphereCollide;
                transition.SpherePath.NumSphere = 2;
                SetCenter(transition);
                var transitionState = sphere.IntersectsSphere(transition, false);
                Assert.AreEqual(transitionState, interp ? TransitionState.OK : TransitionState.Collided);
            }
        }

        public void SetCenter(Transition transition)
        {
            // refactor this mapping
            transition.SpherePath.GlobalCurrCenter = transition.SpherePath.GlobalSphere;
        }

        [TestMethod]
        public void Sphere_LandOnSphere()
        {
            var sphere = new Sphere(Vector3.Zero, 5.0f);
            var transition = new Transition();

            // defines the collision normal
            transition.SpherePath.GlobalCurrCenter.Add(new Sphere(new Vector3(0, 0, -1), 5.0f));

            // test collision
            var transitionState = sphere.LandOnSphere(transition, new Sphere(), Vector3.Zero, sphere.Radius * 2);
            Assert.AreEqual(transitionState, TransitionState.Adjusted);

            // test adjusted
            transition.SpherePath.GlobalCurrCenter[0] = new Sphere(new Vector3(0, 0, 0.0001f), 5.0f);
            transitionState = sphere.LandOnSphere(transition, new Sphere(), Vector3.Zero, sphere.Radius * 2);
            Assert.AreEqual(transitionState, TransitionState.Collided);
            Assert.AreEqual(transition.SpherePath.Collide, true);
        }

        //[TestMethod]
        public void Sphere_StepSphereUp()
        {
            var sphere = new Sphere(Vector3.Zero, 5.0f);
            var transition = new Transition();

            // the location being stepped up to
            var disp = new Vector3(0, 0, 1);
            var checkPos = new Sphere();

            var transitionState = sphere.StepSphereUp(transition, checkPos, disp, sphere.Radius * 2.0f);
            // TODO: implement SpherePath.StepUpSlide()
        }

        [TestMethod]
        public void Sphere_StepSphereDown()
        {
            var sphere = new Sphere(Vector3.Zero, 5.0f);

            var transition = new Transition();
            transition.SpherePath.StepDownAmt = -10.0f;     // the amount being stepped down this frame
            transition.SpherePath.WalkInterp = 10.0f;

            // the location being stepped down to
            var disp = new Vector3(0, 0, -10);
            var checkPos = new Sphere();

            var transitionState = sphere.StepSphereDown(transition, checkPos, ref disp, sphere.Radius * 2.0f);
            Assert.AreEqual(transitionState, TransitionState.Adjusted);
        }

        [TestMethod]
        public void Sphere_SlideSphere()
        {
            var sphere = new Sphere(Vector3.Zero, 5.0f);
            var collisionNormal = new Vector3(0, 0, -1);

            var transition = new Transition();

            var transitionState = sphere.SlideSphere(transition, ref collisionNormal, Vector3.Zero);
            Assert.AreEqual(transitionState, TransitionState.Slid);

            transition.CollisionInfo.LastKnownContactPlaneValid = true;
            transition.CollisionInfo.LastKnownContactPlane = new Plane(new Vector3(0, 0, 1), 0);

            transitionState = sphere.SlideSphere(transition, ref collisionNormal, Vector3.Zero);
            Assert.AreEqual(transitionState, TransitionState.OK);
        }
    }
}
