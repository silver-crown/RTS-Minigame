using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {

    public Vector3 m_target;
    public GameObject collisionExplosion;
    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float step = speed * Time.deltaTime;

        if (m_target != null)
        {
            if(transform.position == m_target)
            {
                explode();
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, m_target, step);
        }
	}
    
    public void setTarget(Vector3 target)
    {
        m_target = target;
    }

    void explode()
    {
        if(collisionExplosion != null)
        {
            GameObject exploision = (GameObject)Instantiate(
                collisionExplosion, transform.position, transform.rotation);

            Destroy(gameObject);
            Destroy(exploision, 1f);
        }
    }
}
