using System.Collections.Generic;
using UnityEngine;

public class VoronoiTest : MonoBehaviour
{
    public Voronoi voronoi;
    public float x_range = 10;
    public float y_range = 10;

    public float edge_size = 2;

    public int num_points = 5;

    public bool randomize = false;
    public int seed = 1;

    public bool animate = false;
    public float animation_speed = 1;

    public List<Vector2> points;

    // Use this for initialization
    void Start()
    {
        voronoi = new Voronoi();
    }

    // Update is called once per frame
    void Update()
    {

        List<BeachLineEdge> voronoiOutput;

        // Create site events
        List<VoronoiSite> siteList = new List<VoronoiSite>();
        UnityEngine.Random.InitState(seed);
        if (randomize)
        {

            // Generate points
            for (int i = 0; i < num_points; i++)
            {
                Vector2 point = new Vector2(
                    UnityEngine.Random.Range(
                        (-x_range / 2) - edge_size, (x_range / 2) + edge_size
                        ),
                    UnityEngine.Random.Range(
                        (-y_range / 2) - edge_size, (y_range / 2) + edge_size
                        ));

                // Apply animation
                if(animate)
                {
                    Vector2 direction = new Vector2(
                        UnityEngine.Random.Range(-animation_speed, animation_speed), 
                        UnityEngine.Random.Range(-animation_speed, animation_speed)
                        ) * Time.time;
                    float xAfterAnimation = Mathf.Repeat((point.x + direction.x), x_range) - x_range/2;
                    //if(Mathf.Abs(xAfterAnimation) > x_range / 2)
                    //{
                    //    xAfterAnimation = -Mathf.Sign(xAfterAnimation) * (x_range / 2);
                    //}
                    float yAfterAnimation = Mathf.Repeat((point.y + direction.y), y_range) - y_range/2;
                    //if (Mathf.Abs(yAfterAnimation) > y_range / 2)
                    //{
                    //    yAfterAnimation = -Mathf.Sign(yAfterAnimation) * (y_range / 2);
                    //}
                    point = new Vector2(xAfterAnimation, yAfterAnimation);
                }

                siteList.Add(new VoronoiSite(point));
            }

            // Apply animation
            foreach(VoronoiSite site in siteList)
            {

            }
        }
        else
        {
            foreach (Vector2 point in points)
            {
                siteList.Add(new VoronoiSite(point));
            }
        }

        voronoiOutput = voronoi.DoVoronoi(siteList);

        // Draw voronoi
        voronoi.Beachline.Draw();
        foreach (VoronoiEvent e in voronoi.Events)
        {
            e.Draw();
        }
        foreach (BeachLineEdge edge in voronoiOutput)
        {
            Debug.DrawLine(edge.Start, edge.Start + edge.Direction, Color.blue);
            //edge.Draw(sweepLine);
        }
        foreach (VoronoiSite point in siteList)
        {
            point.Draw();
        }

        Debug.DrawLine(new Vector3(-10, voronoi.Sweepline, 0), new Vector3(10, voronoi.Sweepline, 0), Color.magenta);
    }

}
