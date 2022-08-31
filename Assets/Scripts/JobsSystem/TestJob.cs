using System.Collections;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;

namespace JobsSystem
{
    public class TestJob : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private int _count;
        [SerializeField] private float _spawnRadius;

        private TransformAccessArray _array;
        private NativeArray<Color> _colorA;
        private NativeArray<Color> _output;
        private NativeArray<int> _angle;
        private Material[] _materials;


        private IEnumerator ChangeDirection()
        {
            while (true)
            {
                Shuffle job = new Shuffle()
                {
                    A = _colorA,
                    Seed = (uint)(UnityEngine.Random.value * 10000)
                };
                
                JobHandle jobHandle = job.Schedule();
                jobHandle.Complete();

                yield return new WaitForSecondsRealtime(1f);
            }
        }

        private void Start()
        {
            _materials = new Material[_count];
            _colorA = new NativeArray<Color>(_count, Allocator.Persistent);
            _output = new NativeArray<Color>(_count, Allocator.Persistent);
            _angle = new NativeArray<int>(_count, Allocator.Persistent);
            _array = new TransformAccessArray(SpawnObject(_prefab, _count, _spawnRadius));
            
            for (int i = 0; i < _count; i++)
            {
                _colorA[i] = Random.ColorHSV();
                _output[i] = Random.ColorHSV();
                _angle[i] = Random.Range(0, 180);
            }

            StartCoroutine(ChangeDirection());
        }

        private void Update()
        {
            MyJobParTransform myJobParTransform = new MyJobParTransform()
            {
                Direction = _direction,
                DeltaTime = Time.deltaTime,
                Random = Random.Range(-1f, 1f),
                A = _colorA,
                Output = _output,
                Angles = _angle
            };

            JobHandle jobHandle = myJobParTransform.Schedule(_array);
            jobHandle.Complete();

            for (int i = 0; i < -_count; i++)
            {
                _materials[i].color = _output[i];
            }
        }

        Transform[] SpawnObject(GameObject prefab, int count, float spawnRadius)
        {
            Transform[] objects = new Transform[count];
            for (int i = 0; i < count; i++)
            {
                objects[i] = Instantiate(prefab).transform;
                objects[i].position = Random.insideUnitSphere * spawnRadius;
            }

            return objects;
        }

        private void OnDestroy()
        {
            _array.Dispose();
            _colorA.Dispose();
            _output.Dispose();
            _angle.Dispose();
        }

        public NativeArray<Color> Shuffle(NativeArray<Color> colors)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                Color temp;
                int rnd = Random.Range(0, colors.Length);
                temp = colors[rnd];
                colors[rnd] = colors[i];
                colors[i] = temp;
            }
            return colors;
        }
    }

    [BurstCompile]
    public struct Shuffle : IJob
    {
        public NativeArray<Color> A;
        public uint Seed;
        
        public void Execute()
        {
            Unity.Mathematics.Random random = new Unity.Mathematics.Random(Seed);

            for (int i = 0; i < A.Length; i++)
            {
                Color temp;
                int rnd = random.NextInt(0, A.Length);
                temp = A[rnd];
                A[rnd] = A[i];
                A[i] = temp;
            }
        }
    }

    [BurstCompile]
    public struct MyJobParTransform : IJobParallelForTransform
    {
        public Vector3 Direction;
        public float DeltaTime;
        public float Random;
        public NativeArray<Color> A;
        public NativeArray<Color> Output;
        public NativeArray<int> Angles;

        public void Execute(int index, TransformAccess transform)
        {
            transform.position += Direction * DeltaTime;
            transform.localRotation = Quaternion.AngleAxis(Angles[index], Vector3.up);
            Angles[index] = Angles[index] == 180 ? 0 : Angles[index] + 1;
            Output[index] = Color.Lerp(Output[index], A[index], DeltaTime);
        }
    }


    #region TestJob1

    /*public class TestJob : MonoBehaviour
    {
        void Start()
        {
            MyJob myJob = new MyJob()
            {
                first = Vector3.up,
                second = Vector3.left
            };

            JobHandle jobHandle = myJob.Schedule();
            jobHandle.Complete();
        }
    }

    public struct MyJob : IJob
    {
        public Vector3 first;
        public Vector3 second;
        
        public void Execute()
        {
            var result = first + second;
            Debug.Log(result);
        }
    }*/

    #endregion

    #region TestJob2

    /*public NativeArray<int> NatArr;
    void Start()
    {
        NatArr = new NativeArray<int>(new []{1, 7, 10, 11, 14},Allocator.Persistent);
        MyJob myJob = new MyJob()
        {
            nat = NatArr
        };

        JobHandle jobHandle = myJob.Schedule();
        jobHandle.Complete();
        for (int i = 0; i < NatArr.Length; i++)
        {
            Debug.Log($"NativeArray start {NatArr[i]}");
            if (NatArr[i] > 10)
            NatArr[i] = 0;
            Debug.Log($"NativeArray end{NatArr[i]}");
        }
        }

        private void OnDestroy()
        {
        NatArr.Dispose();
        }
    }

    public struct MyJob : IJob
    {
        public NativeArray<int> nat;
            
        public void Execute()
        {
            for (int i = 0; i < nat.Length; i++)
            {
                nat[i] = nat[i] * 2;
            }
        }
    }*/

    #endregion

    #region TestJob3

    /*public class TestJob : MonoBehaviour
    {
        public NativeArray<int> NatArr;
        void Start()
        {
            NatArr = new NativeArray<int>(new []{1, 23, 4, 5}, Allocator.Persistent);
            MyJob myJob = new MyJob()
            {
                nat = NatArr
            };

            JobHandle jobHandle = myJob.Schedule(NatArr.Length, 0);
            jobHandle.Complete();
            for (int i = 0; i < NatArr.Length; i++)
            {
                Debug.Log($"NativeArray {NatArr[i]}");
            }
        }

        private void OnDestroy()
        {
            NatArr.Dispose();
        }
    }

    public struct MyJob : IJobParallelFor
    {
        public NativeArray<int> nat;
        
        public void Execute(int index)
        {
            nat[index] = nat[index] * 2;
        }
    }*/

    #endregion

    #region TestJob4

    /*public class TestJob : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private int _count;
        [SerializeField] private float _spawnRadius;

        private TransformAccessArray _array;

        private void Start()
        {
            _array = new TransformAccessArray(SpawnObject(_prefab, _count, _spawnRadius));
        }

        private void Update()
        {
            MyJob myJob = new MyJob()
            {
                Direction = _direction,
                DeltaTime = Time.deltaTime
            };

            JobHandle jobHandle = myJob.Schedule(_array);
            jobHandle.Complete();
        }

        Transform[] SpawnObject(GameObject prefab, int count, float spawnRadius)
        {
            Transform[] objects = new Transform[count];

            for (int i = 0; i < count; i++)
            {
                objects[i] = Instantiate(prefab).transform;
                objects[i].position = Random.insideUnitSphere * spawnRadius;
            }

            return objects;
        }

        private void OnDestroy()
        {
            _array.Dispose();
        }
    }

    public struct MyJob : IJobParallelForTransform
    {
        public Vector3 Direction;
        public float DeltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            transform.position += Direction * DeltaTime;
        }
    }*/

    #endregion
}
