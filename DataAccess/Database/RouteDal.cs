using DataAccess.Context;
using Interface.Interface.Dal;
using Interface.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Database;

public class RouteDal(MyDbContext context) : IRouteDal
{
    public async Task<int> CreateRoute(int userId)
    {
        var userModel = await context.Users.FindAsync(userId);
        if (userModel == null)
        {
            throw new Exception("User not found");
        }

        var route = new RouteModel
        {
            UserId = userId,
            UserModel = userModel,
        };
        context.Routes.Add(route);
        await context.SaveChangesAsync();
        return route.Id;
    }

    public async Task CreateDataPoint(DataPointModel[] dataPoints)
    {
        var route = await context.Routes.FindAsync(dataPoints[0].RouteId);
        if (route == null)
        {
            throw new Exception("Route not found");
        }

        dataPoints.ToList().ForEach(x => x.RouteModel = route);
        context.DataPoints.AddRange(dataPoints);
        await context.SaveChangesAsync();
    }

    public async Task<double> GetMaxSpeed(int routeId)
    {
        var route = await context.Routes.Include(r => r.DataPoints).FirstOrDefaultAsync(r => r.Id == routeId);
        if (route == null)
        {
            throw new Exception("Route not found");
        }

        return (double)route.DataPoints.Max(dp => dp.Speed);
    }

    public async Task<double> GetMaxLean(int routeId)
    {
        var route = await context.Routes.Include(r => r.DataPoints).FirstOrDefaultAsync(r => r.Id == routeId);
        if (route == null)
        {
            throw new Exception("Route not found");
        }

        return (double)route.DataPoints.Max(dp => dp.Lean);
    }

    public async Task<double> GetMaxG(int routeId)
    {
        var route = await context.Routes.Include(r => r.DataPoints).FirstOrDefaultAsync(r => r.Id == routeId);
        if (route == null)
        {
            throw new Exception("Route not found");
        }

        var maxLateralG = (double)route.DataPoints.Max(dp => dp.LateralG);
        var maxAcceleration = (double)route.DataPoints.Max(dp => dp.Acceleration);

        return Math.Max(maxLateralG, maxAcceleration);
    }

    public async Task<RouteModel> GetRoute(int routeId)
    {
        var route = await context.Routes.Include(r => r.DataPoints).FirstOrDefaultAsync(r => r.Id == routeId);
        if (route == null)
        {
            throw new Exception("Route not found");
        }

        return route;
    }

    public async Task DeleteRoute(int routeId)
    {
        var route = await context.Routes.FindAsync(routeId);
        if (route == null)
        {
            throw new Exception("Route not found");
        }

        context.Routes.Remove(route);
        await context.SaveChangesAsync();
    }

    public async Task CalculateSpeed(int routeId)
    {
        var route = await GetRoute(routeId);
        var dataPoints = route.DataPoints;
        if (dataPoints.Count == 0)
        {
            throw new Exception("No data points found");
        }

        for (int i = 0; i < dataPoints.Count - 1; i++)
        {
            var point1 = dataPoints.ElementAt(i);
            var point2 = dataPoints.ElementAt(i + 1);
            
            double distance = Haversine(point1.Lat, point1.Lon, point2.Lat, point2.Lon);
            
            var timeDifference = (point2.Timestamp - point1.Timestamp).TotalSeconds;
            
            if (timeDifference > 0)
            {
                // Speed in kilometers per hour
                distance /= 1000; 
                double speed = (distance / timeDifference) * 3600;
                point2.Speed = speed;
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task CalculateLean(int routeId)
    {
        var route = await GetRoute(routeId);
        var dataPoints = route.DataPoints;
        if (dataPoints.Count < 3)
        {
            throw new Exception("Not enough data points found");
        }

        const double g = 9.81;

        for (int i = 1; i < dataPoints.Count - 1; i++)
        {
            var point1 = dataPoints.ElementAt(i - 1);
            var point2 = dataPoints.ElementAt(i);
            var point3 = dataPoints.ElementAt(i + 1);

            // Convert speed from km/h to m/s
            double speed = (point2.Speed ?? 0) / 3.6;

            // Calculate the distance between point1 and point2 using Haversine formula
            double distance1 = Haversine(point1.Lat, point1.Lon, point2.Lat, point2.Lon);

            // Calculate the distance between point2 and point3 using Haversine formula
            double distance2 = Haversine(point2.Lat, point2.Lon, point3.Lat, point3.Lon);

            // Calculate the heading (bearing) between point1 and point2
            double bearing1 = CalculateBearing(point1.Lat, point1.Lon, point2.Lat, point2.Lon);

            // Calculate the heading (bearing) between point2 and point3
            double bearing2 = CalculateBearing(point2.Lat, point2.Lon, point3.Lat, point3.Lon);

            // Calculate turning angle (change in heading) between the two bearings
            double turningAngle = Math.Abs(bearing2 - bearing1);

            // Prevent division by zero or invalid turning radius
            if (turningAngle == 0 || speed == 0)
            {
                point2.Lean = 0; // No lean if no turn or no speed
            }
            else
            {
                // Calculate turning radius: r = distance / turning angle (in radians)
                double turningRadius = distance1 / turningAngle;

                // Calculate the lean angle using the formula: θ = arctan(v² / (r * g))
                double leanAngle = Math.Atan((speed * speed) / (turningRadius * g));

                // Store the lean angle (in degrees) in point2
                point2.Lean = leanAngle * (180 / Math.PI);
            }
        }
        await context.SaveChangesAsync();
    }

    public async Task CalculateG(int routeId)
    {
        var route = await GetRoute(routeId);
        var dataPoints = route.DataPoints;
        if (dataPoints.Count == 0)
        {
            throw new Exception("No data points found");
        }
        const double g = 9.81;  // Gravitational constant in m/s²

        // Loop through data points
        foreach (var point in dataPoints)
        {
            // Ensure lean angle is available (you may need to calculate it if it's not)
            if (point.Lean.HasValue)
            {
                double leanAngleInRadians = ToRadians(point.Lean.Value);
                
                double lateralGForce = Math.Tan(leanAngleInRadians);

                // Store the lateral G-force in point
                point.LateralG = lateralGForce;
            }
        }
        await context.SaveChangesAsync();
    }

    public async Task CalculateAccelerationG(int routeId)
    {
        var route = await GetRoute(routeId);
        var dataPoints = route.DataPoints;
        if (dataPoints.Count < 2)  // At least 2 points needed to calculate acceleration
        {
            throw new Exception("Not enough data points to calculate acceleration");
        }

        const double g = 9.81;  // Gravitational constant in m/s²

        // Loop through data points to calculate acceleration between each pair
        for (int i = 1; i < dataPoints.Count; i++)
        {
            var point1 = dataPoints.ElementAt(i - 1);
            var point2 = dataPoints.ElementAt(i);

            // Convert speeds from km/h to m/s
            double speed1 = (point1.Speed ?? 0) / 3.6;  // Speed of point1 in m/s
            double speed2 = (point2.Speed ?? 0) / 3.6;  // Speed of point2 in m/s

            // Calculate the change in velocity (m/s)
            double deltaV = speed2 - speed1;

            // Calculate the time difference between the points in seconds
            double deltaT = (point2.Timestamp - point1.Timestamp).TotalSeconds;

            // Prevent division by zero or invalid data
            if (deltaT == 0)
            {
                point2.Acceleration = 0;  // If time difference is 0, set acceleration to 0
            }
            else
            {
                // Calculate acceleration (m/s²)
                double acceleration = deltaV / deltaT;

                // Convert acceleration to G-force (m/s² to G)
                double accelerationG = acceleration / g;

                // Store the acceleration in G-force in point2
                point2.Acceleration = accelerationG;
            }
        }
        await context.SaveChangesAsync();
    }

    public async Task CalculateDistance(int routeId)
    {
        var route = await GetRoute(routeId);
        var dataPoints = route.DataPoints;

        if (dataPoints.Count < 2)
        {
            throw new Exception("Not enough data points to calculate distance");
        }

        double totalDistance = 0;
        
        for (int i = 1; i < dataPoints.Count; i++)
        {
            var point1 = dataPoints.ElementAt(i - 1);
            var point2 = dataPoints.ElementAt(i);
            
            double distance = Haversine(point1.Lat, point1.Lon, point2.Lat, point2.Lon);

            totalDistance += distance;
        }
        
        route.Distance = totalDistance;
        await context.SaveChangesAsync();
    }

    // Haversine formula to calculate distance between two points in meters
    private double Haversine(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371.0; 
        double dLat = ToRadians(lat2 - lat1);
        double dLon = ToRadians(lon2 - lon1);

        lat1 = ToRadians(lat1);
        lat2 = ToRadians(lat2);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c * 1000;
    }

    private double ToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }
    
    private double ToDegrees(double radians)
    {
        return radians * 180 / Math.PI;
    }
    
    private double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
    {
        double dLon = ToRadians(lon2 - lon1);

        lat1 = ToRadians(lat1);
        lat2 = ToRadians(lat2);

        double y = Math.Sin(dLon) * Math.Cos(lat2);
        double x = Math.Cos(lat1) * Math.Sin(lat2) -
                   Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);

        return (ToDegrees(Math.Atan2(y, x)) + 360) % 360;  // Bearing in degrees
    }
}