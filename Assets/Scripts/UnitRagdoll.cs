using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void Setup(Transform originaRootBone)
    {
        MatchAllChildTransforms(originaRootBone, ragdollRootBone);
        ApplyExplosionToRagdoll(ragdollRootBone,explosionForce: 300f,transform.position,explosionRange: 10f);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                MatchAllChildTransforms(child,cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root,float explosionForce,Vector3 explosionPosition,float explosionRange)
    {
        foreach (Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce,explosionPosition,explosionRange);
            }
            ApplyExplosionToRagdoll(child,explosionForce,explosionPosition,explosionRange);
        }
    }
}