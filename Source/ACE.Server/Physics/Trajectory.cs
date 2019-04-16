
// LICENSE
//
//   This software is dual-licensed to the public domain and under the following
//   license: you are granted a perpetual, irrevocable license to copy, modify,
//   publish, and distribute this file as you see fit.
//
// VERSION 
//   0.1.0  (2016-06-01)  Initial release
//
// AUTHOR
//   Forrest Smith
//
// ADDITIONAL READING
//   https://blog.forrestthewoods.com/solving-ballistic-trajectories-b0165523348c
//
// API
//    int solve_ballistic_arc(Vector3 proj_pos, float proj_speed, Vector3 target, float gravity, out Vector3 low, out Vector3 high);
//    int solve_ballistic_arc(Vector3 proj_pos, float proj_speed, Vector3 target, Vector3 target_velocity, float gravity, out Vector3 s0, out Vector3 s1, out Vector3 s2, out Vector3 s3);
//    bool solve_ballistic_arc_lateral(Vector3 proj_pos, float lateral_speed, Vector3 target, float max_height, out float vertical_speed, out float gravity);
//    bool solve_ballistic_arc_lateral(Vector3 proj_pos, float lateral_speed, Vector3 target, Vector3 target_velocity, float max_height_offset, out Vector3 fire_velocity, out float gravity, out Vector3 impact_point);
//
//    float ballistic_range(float speed, float gravity, float initial_height);
//
//    bool IsZero(double d);
//    int SolveQuadric(double c0, double c1, double c2, out double s0, out double s1);
//    int SolveCubic(double c0, double c1, double c2, double c3, out double s0, out double s1, out double s2);
//    int SolveQuartic(double c0, double c1, double c2, double c3, double c4, out double s0, out double s1, out double s2, out double s3);


using System;
using System.Diagnostics;
using System.Numerics;

namespace ACE.Server.Physics
{
    public class Trajectory
    {

        // SolveQuadric, SolveCubic, and SolveQuartic were ported from C as written for Graphics Gems I
        // Original Author: Jochen Schwarze (schwarze@isa.de)
        // https://github.com/erich666/GraphicsGems/blob/240a34f2ad3fa577ef57be74920db6c4b00605e4/gems/Roots3And4.c

        // Utility function used by SolveQuadratic, SolveCubic, and SolveQuartic
        public static bool IsZero(double d)
        {
            const double eps = 1e-9;
            return d > -eps && d < eps;
        }

        // Solve quadratic equation: c0*x^2 + c1*x + c2. 
        // Returns number of solutions.
        public static int SolveQuadric(double c0, double c1, double c2, out double s0, out double s1)
        {
            s0 = double.NaN;
            s1 = double.NaN;

            double p, q, D;

            /* normal form: x^2 + px + q = 0 */
            p = c1 / (2 * c0);
            q = c2 / c0;

            D = p * p - q;

            if (IsZero(D))
            {
                s0 = -p;
                return 1;
            }
            else if (D < 0)
            {
                return 0;
            }
            else /* if (D > 0) */
            {
                double sqrt_D = Math.Sqrt(D);

                s0 = sqrt_D - p;
                s1 = -sqrt_D - p;
                return 2;
            }
        }

        // Solve cubic equation: c0*x^3 + c1*x^2 + c2*x + c3. 
        // Returns number of solutions.
        public static int SolveCubic(double c0, double c1, double c2, double c3, out double s0, out double s1, out double s2)
        {
            s0 = double.NaN;
            s1 = double.NaN;
            s2 = double.NaN;

            int num;
            double sub;
            double A, B, C;
            double sq_A, p, q;
            double cb_p, D;

            /* normal form: x^3 + Ax^2 + Bx + C = 0 */
            A = c1 / c0;
            B = c2 / c0;
            C = c3 / c0;

            /*  substitute x = y - A/3 to eliminate quadric term:  x^3 +px + q = 0 */
            sq_A = A * A;
            p = 1.0 / 3 * (-1.0 / 3 * sq_A + B);
            q = 1.0 / 2 * (2.0 / 27 * A * sq_A - 1.0 / 3 * A * B + C);

            /* use Cardano's formula */
            cb_p = p * p * p;
            D = q * q + cb_p;

            if (IsZero(D))
            {
                if (IsZero(q)) /* one triple solution */
                {
                    s0 = 0;
                    num = 1;
                }
                else /* one single and one double solution */
                {
                    double u = Math.Pow(-q, 1.0 / 3.0);
                    s0 = 2 * u;
                    s1 = -u;
                    num = 2;
                }
            }
            else if (D < 0) /* Casus irreducibilis: three real solutions */
            {
                double phi = 1.0 / 3 * Math.Acos(-q / Math.Sqrt(-cb_p));
                double t = 2 * Math.Sqrt(-p);

                s0 = t * Math.Cos(phi);
                s1 = -t * Math.Cos(phi + Math.PI / 3);
                s2 = -t * Math.Cos(phi - Math.PI / 3);
                num = 3;
            }
            else /* one real solution */
            {
                double sqrt_D = Math.Sqrt(D);
                double u = Math.Pow(sqrt_D - q, 1.0 / 3.0);
                double v = -Math.Pow(sqrt_D + q, 1.0 / 3.0);

                s0 = u + v;
                num = 1;
            }

            /* resubstitute */
            sub = 1.0 / 3 * A;

            if (num > 0) s0 -= sub;
            if (num > 1) s1 -= sub;
            if (num > 2) s2 -= sub;

            return num;
        }

        // Solve quartic function: c0*x^4 + c1*x^3 + c2*x^2 + c3*x + c4. 
        // Returns number of solutions.
        public static int SolveQuartic(double c0, double c1, double c2, double c3, double c4, out double s0, out double s1, out double s2, out double s3)
        {
            s0 = double.NaN;
            s1 = double.NaN;
            s2 = double.NaN;
            s3 = double.NaN;

            double[] coeffs = new double[4];
            double z, u, v, sub;
            double A, B, C, D;
            double sq_A, p, q, r;
            int num;

            /* normal form: x^4 + Ax^3 + Bx^2 + Cx + D = 0 */
            A = c1 / c0;
            B = c2 / c0;
            C = c3 / c0;
            D = c4 / c0;

            /*  substitute x = y - A/4 to eliminate cubic term: x^4 + px^2 + qx + r = 0 */
            sq_A = A * A;
            p = -3.0 / 8 * sq_A + B;
            q = 1.0 / 8 * sq_A * A - 1.0 / 2 * A * B + C;
            r = -3.0 / 256 * sq_A * sq_A + 1.0 / 16 * sq_A * B - 1.0 / 4 * A * C + D;

            if (IsZero(r))
            {
                /* no absolute term: y(y^3 + py + q) = 0 */

                coeffs[3] = q;
                coeffs[2] = p;
                coeffs[1] = 0;
                coeffs[0] = 1;

                num = SolveCubic(coeffs[0], coeffs[1], coeffs[2], coeffs[3], out s0, out s1, out s2);
            }
            else
            {
                /* solve the resolvent cubic ... */
                coeffs[3] = 1.0 / 2 * r * p - 1.0 / 8 * q * q;
                coeffs[2] = -r;
                coeffs[1] = -1.0 / 2 * p;
                coeffs[0] = 1;

                SolveCubic(coeffs[0], coeffs[1], coeffs[2], coeffs[3], out s0, out s1, out s2);

                /* ... and take the one real solution ... */
                z = s0;

                /* ... to build two quadric equations */
                u = z * z - r;
                v = 2 * z - p;

                if (IsZero(u))
                    u = 0;
                else if (u > 0)
                    u = Math.Sqrt(u);
                else
                    return 0;

                if (IsZero(v))
                    v = 0;
                else if (v > 0)
                    v = Math.Sqrt(v);
                else
                    return 0;

                coeffs[2] = z - u;
                coeffs[1] = q < 0 ? -v : v;
                coeffs[0] = 1;

                num = SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s0, out s1);

                coeffs[2] = z + u;
                coeffs[1] = q < 0 ? v : -v;
                coeffs[0] = 1;

                if (num == 0) num += SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s0, out s1);
                if (num == 1) num += SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s1, out s2);
                if (num == 2) num += SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s2, out s3);
            }

            /* resubstitute */
            sub = 1.0 / 4 * A;

            if (num > 0) s0 -= sub;
            if (num > 1) s1 -= sub;
            if (num > 2) s2 -= sub;
            if (num > 3) s3 -= sub;

            return num;
        }

        // Calculate the maximum range that a ballistic projectile can be fired on given speed and gravity.
        //
        // speed (float): projectile velocity
        // gravity (float): force of gravity, positive is down
        // initial_height (float): distance above flat terrain
        //
        // return (float): maximum range
        public static float ballistic_range(float speed, float gravity, float initial_height)
        {
            // Handling these cases is up to your project's coding standards
            //Debug.Assert(speed > 0 && gravity > 0 && initial_height >= 0, "fts.ballistic_range called with invalid data");
            if (speed <= 0 || gravity <= 0 || initial_height < 0)
                return 0.0f;

            // Derivation
            //   (1) x = speed * time * cos O
            //   (2) z = initial_height + (speed * time * sin O) - (.5 * gravity*time*time)
            //   (3) via quadratic: t = (speed*sin O)/gravity + sqrt(speed*speed*sin O + 2*gravity*initial_height)/gravity    [ignore smaller root]
            //   (4) solution: range = x = (speed*cos O)/gravity * sqrt(speed*speed*sin O + 2*gravity*initial_height)    [plug t back into x=speed*time*cos O]
            var angle = 45 * 0.0174533; // no air resistence, so 45 degrees provides maximum range
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);

            var range = (speed * cos / gravity) * (speed * sin + Math.Sqrt(speed * speed * sin * sin + 2 * gravity * initial_height));
            return (float)range;
        }

        // Solve firing angles for a ballistic projectile with speed and gravity to hit a fixed position.
        //
        // proj_pos (Vector3): point projectile will fire from
        // proj_speed (float): scalar speed of projectile
        // target (Vector3): point projectile is trying to hit
        // gravity (float): force of gravity, positive down
        //
        // s0 (out Vector3): firing solution (low angle) 
        // s1 (out Vector3): firing solution (high angle)
        //
        // return (int): number of unique solutions found: 0, 1, or 2.
        public static int solve_ballistic_arc(Vector3 proj_pos, float proj_speed, Vector3 target, float gravity, out Vector3 s0, out Vector3 s1, out float t0, out float t1)
        {
            // Handling these cases is up to your project's coding standards
            //Debug.Assert(proj_pos != target && proj_speed > 0 && gravity > 0, "fts.solve_ballistic_arc called with invalid data");

            // C# requires out variables be set
            s0 = Vector3.Zero;
            s1 = Vector3.Zero;
            t0 = float.PositiveInfinity;
            t1 = float.PositiveInfinity;

            if (proj_pos == target || proj_speed <= 0 || gravity <= 0)
                return 0;

            // Derivation
            //   (1) x = v*t*cos O
            //   (2) z = v*t*sin O - .5*g*t^2
            // 
            //   (3) t = x/(cos O*v)                                        [solve t from (1)]
            //   (4) z = v*x*sin O/(cos O * v) - .5*g*x^2/(cos^2 O*v^2)     [plug t into z=...]
            //   (5) z = x*tan O - g*x^2/(2*v^2*cos^2 O)                    [reduce; cos/sin = tan]
            //   (6) z = x*tan O - (g*x^2/(2*v^2))*(1+tan^2 O)              [reduce; 1+tan O = 1/cos^2 O]
            //   (7) 0 = ((-g*x^2)/(2*v^2))*tan^2 O + x*tan O - (g*x^2)/(2*v^2) - z    [re-arrange]
            //   Quadratic! a*p^2 + b*p + c where p = tan O
            //
            //   (8) let gxv = -g*x*x/(2*v*v)
            //   (9) p = (-x +- sqrt(x*x - 4gxv*(gxv - z)))/2*gxv           [quadratic formula]
            //   (10) p = (v^2 +- sqrt(v^4 - g(g*x^2 + 2*z*v^2)))/gx        [multiply top/bottom by -2*v*v/x; move 4*v^4/x^2 into root]
            //   (11) O = atan(p)

            Vector3 diff = target - proj_pos;
            Vector3 diffXY = new Vector3(diff.X, diff.Y, 0);
            float groundDist = diffXY.Length();

            float speed2 = proj_speed * proj_speed;
            float speed4 = proj_speed * proj_speed * proj_speed * proj_speed;
            float z = diff.Z;
            float x = groundDist;
            float gx = gravity * x;

            float root = speed4 - gravity * (gravity * x * x + 2 * z * speed2);

            // No solution
            if (root < 0)
                return 0;

            root = (float)Math.Sqrt(root);

            var lowAng = Math.Atan2(speed2 - root, gx);
            var highAng = Math.Atan2(speed2 + root, gx);
            int numSolutions = lowAng != highAng ? 2 : 1;

            Vector3 groundDir = Vector3.Normalize(diffXY);
            s0 = groundDir * (float)Math.Cos(lowAng) * proj_speed + Vector3.UnitZ * (float)Math.Sin(lowAng) * proj_speed;
            if (numSolutions > 1)
                s1 = groundDir * (float)Math.Cos(highAng) * proj_speed + Vector3.UnitZ * (float)Math.Sin(highAng) * proj_speed;

            t0 = x / ((float)Math.Cos(lowAng) * proj_speed);
            t1 = x / ((float)Math.Cos(highAng) * proj_speed);

            return numSolutions;
        }

        // Solve firing angles for a ballistic projectile with speed and gravity to hit a target moving with constant, linear velocity.
        //
        // proj_pos (Vector3): point projectile will fire from
        // proj_speed (float): scalar speed of projectile
        // target (Vector3): point projectile is trying to hit
        // target_velocity (Vector3): velocity of target
        // gravity (float): force of gravity, positive down
        //
        // s0 (out Vector3): firing solution (fastest time impact) 
        // s1 (out Vector3): firing solution (next impact)
        // s2 (out Vector3): firing solution (next impact)
        // s3 (out Vector3): firing solution (next impact)
        //
        // return (int): number of unique solutions found: 0, 1, 2, 3, or 4.
        public static int solve_ballistic_arc(Vector3 proj_pos, float proj_speed, Vector3 target_pos, Vector3 target_velocity, float gravity, out Vector3 s0, out Vector3 s1, out float time)
        {
            // Initialize output parameters
            s0 = Vector3.Zero;
            s1 = Vector3.Zero;

            // Derivation 
            //
            //  For full derivation see: blog.forrestthewoods.com
            //  Here is an abbreviated version.
            //
            //  Four equations, four unknowns (solution.x, solution.y, solution.z, time):
            //
            //  (1) proj_pos.x + solution.x*time = target_pos.x + target_vel.x*time
            //  (2) proj_pos.y + solution.y*time = target_pos.y + target_vel.y*time
            //  (3) proj_pos.z + solution.z*time + .5*G*t = target_pos.z + target_vel.z*time
            //  (4) proj_speed^2 = solution.x^2 + solution.y^2 + solution.z^2
            //
            //  (5) Solve for solution.x and solution.y in equations (1) and (2)
            //  (6) Square solution.x and solution.y from (5)
            //  (7) Solve solution.z^2 by plugging (6) into (4)
            //  (8) Solve solution.z by rearranging (2)
            //  (9) Square (8)
            //  (10) Set (8) = (7). All solution.xyz terms should be gone. Only time remains.
            //  (11) Rearrange 10. It will be of the form a*^4 + b*t^3 + c*t^2 + d*t * e. This is a quartic.
            //  (12) Solve the quartic using SolveQuartic.
            //  (13) If there are no positive, real roots there is no solution.
            //  (14) Each positive, real root is one valid solution
            //  (15) Plug each time value into (1) (2) and (3) to calculate solution.xyz
            //  (16) The end.

            double G = gravity;

            double A = proj_pos.X;
            double B = proj_pos.Z;
            double C = proj_pos.Y;
            double M = target_pos.X;
            double N = target_pos.Z;
            double O = target_pos.Y;
            double P = target_velocity.X;
            double Q = target_velocity.Z;
            double R = target_velocity.Y;
            double S = proj_speed;

            double H = M - A;
            double J = O - C;
            double K = N - B;
            double L = -.5f * G;

            // Quartic Coeffecients
            double c0 = L * L;
            double c1 = 2 * Q * L;
            double c2 = Q * Q + 2 * K * L - S * S + P * P + R * R;
            double c3 = 2 * K * Q + 2 * H * P + 2 * J * R;
            double c4 = K * K + H * H + J * J;

            // Solve quartic
            double[] times = new double[4];
            int numTimes = SolveQuartic(c0, c1, c2, c3, c4, out times[0], out times[1], out times[2], out times[3]);

            // Sort so faster collision is found first
            Array.Sort(times);

            // Plug quartic solutions into base equations
            // There should never be more than 2 positive, real roots.
            Vector3[] solutions = new Vector3[2];
            int numSolutions = 0;

            time = 0.0f;
            for (int i = 0; i < numTimes && numSolutions < 2; ++i)
            {
                double t = times[i];
                if (t <= 0)
                    continue;

                if (numSolutions == 0)
                    time = (float)t;

                solutions[numSolutions].X = (float)((H + P * t) / t);
                solutions[numSolutions].Y = (float)((J + R * t) / t);
                solutions[numSolutions].Z = (float)((K + Q * t - L * t * t) / t);
                ++numSolutions;
            }

            // Write out solutions
            if (numSolutions > 0) s0 = solutions[0];
            if (numSolutions > 1) s1 = solutions[1];

            return numSolutions;
        }

        /// <summary>
        /// Solve for a firing arc with a fixed gravity. Method was adapted for use by ACE by gmriggs from the original
        /// </summary>
        /// <param name="projectilePosition"></param>
        /// <param name="lateralSpeed"></param>
        /// <param name="targetPosition"></param>
        /// <param name="velocityVector"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool SolveBallisticArc(Vector3 projectilePosition, float lateralSpeed, Vector3 targetPosition, out Vector3 velocityVector, out float time)
        {
            // Handling these cases is up to your project's coding standards
            //Debug.Assert(projectilePosition != targetPosition && lateralSpeed > 0, "fts.solve_ballistic_arc called with invalid data");
            velocityVector = Vector3.Zero;
            time = float.NaN;

            if (projectilePosition == targetPosition || lateralSpeed <= 0)
                return false;

            Vector3 diff = targetPosition - projectilePosition;
            Vector3 diffXY = new Vector3(diff.X, diff.Y, 0f);
            float lateralDist = diffXY.Length();

            if (lateralDist == 0)
                return false;

            time = lateralDist / lateralSpeed;

            velocityVector = Vector3.Normalize(diffXY) * lateralSpeed;

            // System of equations. Hit max_height at t=.5*time. Hit target at t=time.
            //
            // peak = z0 + vertical_speed*halfTime + .5*gravity*halfTime^2
            // end = z0 + vertical_speed*time + .5*gravity*time^s
            // Wolfram Alpha: solve b = a + .5*v*t + .5*g*(.5*t)^2, c = a + vt + .5*g*t^2 for g, v
            float a = projectilePosition.Z; // initial
            float c = targetPosition.Z;     // final

            // Gravity value pulled from ACE property
            var g = PhysicsGlobals.Gravity;
            var b = (4 * a + 4 * c - g * time * time) / 8;

            velocityVector.Z = (2 * a - 2 * c + g * time * time) / (time * 2) * -1;

            return true;
        }

        // Solve the firing arc with a fixed lateral speed. Vertical speed and gravity varies. 
        // This enables a visually pleasing arc.
        //
        // proj_pos (Vector3): point projectile will fire from
        // lateral_speed (float): scalar speed of projectile along XY plane
        // target_pos (Vector3): point projectile is trying to hit
        // max_height (float): height above Max(proj_pos, impact_pos) for projectile to peak at
        //
        // fire_velocity (out Vector3): firing velocity
        // gravity (out float): gravity necessary to projectile to hit precisely max_height
        //
        // return (bool): true if a valid solution was found
        public static bool solve_ballistic_arc_lateral(Vector3 proj_pos, float lateral_speed, Vector3 target_pos, float max_height, out Vector3 fire_velocity, out float gravity)
        {
            // Handling these cases is up to your project's coding standards
            Debug.Assert(proj_pos != target_pos && lateral_speed > 0 && max_height > proj_pos.Z, "fts.solve_ballistic_arc called with invalid data");

            fire_velocity = Vector3.Zero;
            gravity = float.NaN;

            if (proj_pos == target_pos || lateral_speed <= 0 || max_height <= proj_pos.Z)
                return false;

            Vector3 diff = target_pos - proj_pos;
            Vector3 diffXY = new Vector3(diff.X, diff.Y, 0f);
            float lateralDist = diffXY.Length();

            if (lateralDist == 0)
                return false;

            float time = lateralDist / lateral_speed;

            fire_velocity = Vector3.Normalize(diffXY) * lateral_speed;

            // System of equations. Hit max_height at t=.5*time. Hit target at t=time.
            //
            // peak = z0 + vertical_speed*halfTime + .5*gravity*halfTime^2
            // end = z0 + vertical_speed*time + .5*gravity*time^s
            // Wolfram Alpha: solve b = a + .5*v*t + .5*g*(.5*t)^2, c = a + vt + .5*g*t^2 for g, v
            float a = proj_pos.Z;       // initial
            float b = max_height;       // peak
            float c = target_pos.Z;     // final

            gravity = -4 * (a - 2 * b + c) / (time * time);
            fire_velocity.Z = -(3 * a - 4 * b + c) / time;

            return true;
        }

        // Solve the firing arc with a fixed lateral speed. Vertical speed and gravity varies. 
        // This enables a visually pleasing arc.
        //
        // proj_pos (Vector3): point projectile will fire from
        // lateral_speed (float): scalar speed of projectile along XY plane
        // target_pos (Vector3): point projectile is trying to hit
        // max_height (float): height above Max(proj_pos, impact_pos) for projectile to peak at
        //
        // fire_velocity (out Vector3): firing velocity
        // gravity (out float): gravity necessary to projectile to hit precisely max_height
        // impact_point (out Vector3): point where moving target will be hit
        //
        // return (bool): true if a valid solution was found
        public static bool solve_ballistic_arc_lateral(Vector3 proj_pos, float lateral_speed, Vector3 target, Vector3 target_velocity, float gravity, out Vector3 fire_velocity, out float time, out Vector3 impact_point)
        {
            // Handling these cases is up to your project's coding standards
            //Debug.Assert(proj_pos != target && lateral_speed > 0, "fts.solve_ballistic_arc_lateral called with invalid data");

            // Initialize output variables
            fire_velocity = Vector3.Zero;
            time = 0.0f;
            impact_point = Vector3.Zero;

            if (proj_pos == target || lateral_speed <= 0)
                return false;

            // Ground plane terms
            Vector3 targetVelXY = new Vector3(target_velocity.X, target_velocity.Y, 0f);
            Vector3 diffXY = target - proj_pos;
            diffXY.Z = 0;

            // Derivation
            //   (1) Base formula: |P + V*t| = S*t
            //   (2) Substitute variables: |diffXY + targetVelXY*t| = S*t
            //   (3) Square both sides: Dot(diffXY,diffXY) + 2*Dot(diffXY, targetVelXY)*t + Dot(targetVelXY, targetVelXY)*t^2 = S^2 * t^2
            //   (4) Quadratic: (Dot(targetVelXY,targetVelXY) - S^2)t^2 + (2*Dot(diffXY, targetVelXY))*t + Dot(diffXY, diffXY) = 0
            float c0 = Vector3.Dot(targetVelXY, targetVelXY) - lateral_speed * lateral_speed;
            float c1 = 2f * Vector3.Dot(diffXY, targetVelXY);
            float c2 = Vector3.Dot(diffXY, diffXY);
            double t0, t1;
            int n = SolveQuadric(c0, c1, c2, out t0, out t1);

            // pick smallest, positive time
            bool valid0 = n > 0 && t0 > 0;
            bool valid1 = n > 1 && t1 > 0;

            float t;
            if (!valid0 && !valid1)
                return false;
            else if (valid0 && valid1)
                t = Math.Min((float)t0, (float)t1);
            else
                t = valid0 ? (float)t0 : (float)t1;

            // Calculate impact point
            impact_point = target + (target_velocity * t);

            // Calculate fire velocity along XZ plane
            Vector3 dir = impact_point - proj_pos;
            fire_velocity = Vector3.Normalize(new Vector3(dir.X, dir.Y, 0f)) * lateral_speed;

            // Solve system of equations. Hit max_height at t=.5*time. Hit target at t=time.
            //
            // peak = z0 + vertical_speed*halfTime + .5*gravity*halfTime^2
            // end = z0 + vertical_speed*time + .5*gravity*time^s
            // Wolfram Alpha: solve b = a + .5*v*t + .5*g*(.5*t)^2, c = a + vt + .5*g*t^2 for g, v
            float a = proj_pos.Z;       // initial
            //float b = Math.Max(proj_pos.Z, impact_point.Z) + max_height_offset;  // peak
            float c = impact_point.Z;   // final

            //gravity = -4 * (a - 2 * b + c) / (t * t);
            //fire_velocity.Z = -(3 * a - 4 * b + c) / t;

            var g = gravity;
            var b = (4 * a + 4 * c - g * t * t) / 8;

            fire_velocity.Z = (2 * a - 2 * c + g * t * t) / (t * 2) * -1;

            time = t;

            return true;
        }
    }
}
