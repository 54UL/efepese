using UnityEngine;

public class IKSolver : MonoBehaviour
{
    public Transform[] chain;
    public Transform target;
    public float tolerance = 0.001f;
    public int maxIterations = 10;

    private void Update()
    {
        SolveIK();
    }

    private void SolveIK()
    {
        int iteration = 0;
        float error = float.MaxValue;

        while (error > tolerance && iteration < maxIterations)
        {
            for (int i = chain.Length - 1; i >= 0; i--)
            {
                if (i == chain.Length - 1)
                {
                    chain[i].rotation = Quaternion.LookRotation(target.position - chain[i].position);
                }
                else
                {
                    chain[i].rotation = Quaternion.LookRotation(chain[i + 1].position - chain[i].position);
                }
            }

            Vector3 currentPosition = chain[0].position;
            error = Vector3.Distance(currentPosition, target.position);
            iteration++;
        }

        // Update the position of each joint in the chain
        for (int i = 0; i < chain.Length - 1; i++)
        {
            chain[i].position = chain[i + 1].position - chain[i].forward * chain[i].localScale.z;
        }
        chain[chain.Length - 1].position = target.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, 0.1f);

        for (int i = 0; i < chain.Length; i++)
        {
            if (chain[i] == null) return;

            if (i == 0)
            {
                Gizmos.color = Color.green;
            }
            else if (i == chain.Length - 1)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.yellow;
            }

            Gizmos.DrawWireSphere(chain[i].position, 0.05f);
            if (i > 0)
            {
                Gizmos.DrawLine(chain[i - 1].position, chain[i].position);
            }
        }
    }
}