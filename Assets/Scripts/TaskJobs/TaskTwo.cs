using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace TaskJobs
{
    public class TaskTwo: MonoBehaviour
    {
        [SerializeField] private int _numberOfEntities;
        [SerializeField] private float _startDistance;
        [SerializeField] private float _startVelocity;
        [SerializeField] private GameObject _bodyPrefab;
        private NativeArray<Vector3> _positions;
        private NativeArray<Vector3> _velocities;
        private NativeArray<Vector3> _finalPositions;
        private JobHandle _jobHandle;
        private JobTwo jobTwo;

        public void Start()
        {
            _positions = new NativeArray<Vector3>(_numberOfEntities, Allocator.Persistent);
            _velocities = new NativeArray<Vector3>(_numberOfEntities, Allocator.Persistent);
            Transform[] transforms = new Transform[_numberOfEntities];

            for (int i = 0; i < _numberOfEntities; i++)
            {
                _positions[i] = Random.insideUnitSphere * Random.Range(0, _startDistance);
                _velocities[i] = Random.insideUnitSphere * Random.Range(0, _startVelocity);
                
                transforms[i] = Instantiate(_bodyPrefab).transform;
            }
            
            jobTwo= new JobTwo
            {
                Positions = _positions,
                Velocities = _velocities,
                FinalPositions = _finalPositions
            };
            _jobHandle = jobTwo.Schedule(5, 0);
            _jobHandle.Complete();
        }
        
        private void OnDestroy()
        {
            _positions.Dispose();
            _velocities.Dispose();
            _finalPositions.Dispose();
        }
    }
    
    
    public struct JobTwo : IJobParallelFor
    {
        public NativeArray<Vector3> Positions;
        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> FinalPositions;
        
        public void Execute(int index)
        {
            for (int i = 0; i < Positions.Length; i++)
            {
                Vector3 sumPositions = Positions[i];
                
                for (int j = 0; j < Velocities.Length; j++)
                {
                    Vector3 sumVelocities = Velocities[j];
                }
            }
        }
    }
}