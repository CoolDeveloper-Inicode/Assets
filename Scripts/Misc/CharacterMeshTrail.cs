using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMeshTrail : MonoBehaviour
{
    public float activeTime;

    [Header("Mesh Properties")]
    public float meshRefreshRate;
    public float destroyMeshTime;
    public Material mat;
    public Transform meshTrailTransform;

    [HideInInspector]
    public bool isTrailActive;

    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    public IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.position = meshTrailTransform.position;
                gObj.transform.rotation = meshTrailTransform.rotation;

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                Destroy(gObj, destroyMeshTime);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }
}
