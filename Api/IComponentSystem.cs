using System;
using System.Collections.Generic;

namespace uFrame.ECS
{
    public interface IComponentSystem : IEcsSystem
    {
        bool HasAny(int entityId, params Type[] type);
        bool TryGetComponent<TComponent>(int entityId, out TComponent component) where TComponent : class, IEcsComponent;
        bool TryGetComponent<TComponent>(int[] entityIds, out TComponent[] component) where TComponent : class, IEcsComponent;
        bool TryGetComponent<TComponent>(List<int> entityIds, out TComponent[] component) where TComponent : class, IEcsComponent;
        IEnumerable<TComponent> GetAllComponents<TComponent>() where TComponent : class, IEcsComponent;
        IEcsComponentManagerOf<TComponent> RegisterComponent<TComponent>() where TComponent : class, IEcsComponent;
        IEcsComponentManager RegisterComponent(Type componentType);
        void RegisterComponentInstance(Type componentType, IEcsComponent instance);
        void DestroyComponentInstance(Type componentType, IEcsComponent instance);
        void AddComponent(int entityId, Type componentType);
        void AddComponent<TComponentType>(int entityId) where TComponentType : class, IEcsComponent;

        TGroupType RegisterGroup<TGroupType, TComponent>() where TComponent : GroupItem, new()
            where TGroupType : ReactiveGroup<TComponent>, new();

    }
}