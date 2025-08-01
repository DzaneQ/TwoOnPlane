using TwoOnPlane.Players;
using Unity.Entities;
using Unity.Transforms;
using UnityEditor.Animations;
using UnityEngine;

public class LoadModelAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject _firstModel;
    [SerializeField] private GameObject _secondModel;
    [SerializeField] private AnimatorController _firstModelAnimatorController;

    class Baker : Baker<LoadModelAuthoring>
    {
        public override void Bake(LoadModelAuthoring authoring)
        {
            authoring.AddModel();
        }
    }

    private void AddModel()
    {
        Transform player = GetFirstPlayerOrNull();
        if (player == null || player.childCount == 0 || player.GetChild(0).name == _secondModel.name)
        {
            Instantiate(_firstModel, transform);
            AddAnimatorControllerToModel();
        }
        else Instantiate(_secondModel, transform);
        AddAnimatorControllerToModel();
    }

    private Transform GetFirstPlayerOrNull()
    {
        Transform entitySubscene = transform.parent;
        for (int i = 0; i < entitySubscene.childCount; i++)
        {
            if (entitySubscene.GetChild(i).name == "Player") return entitySubscene.GetChild(i);
        }
        return null;
    }

    private void AddAnimatorControllerToModel()
    {
        transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = _firstModelAnimatorController;
    }
}
