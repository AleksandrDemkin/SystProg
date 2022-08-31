using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace TaskJobs
{
    public class TaskOne : MonoBehaviour
    {
        public NativeArray<int> NatArr;
        void Start()
        {
            NatArr = new NativeArray<int>(new []{1, 7, 10, 11, 14},Allocator.Persistent);
            MyJob myJob = new MyJob()
            {
                Nat = NatArr
            };

            JobHandle jobHandle = myJob.Schedule();
            jobHandle.Complete();
            for (int i = 0; i < NatArr.Length; i++)
            {
                if (NatArr[i] > 10)
                    NatArr[i] = 0;
                Debug.Log($"NativeArray end {NatArr[i]}");
            }
        }

        private void OnDestroy()
        {
            NatArr.Dispose();
        }
    }

    public struct MyJob : IJob
    {
        public NativeArray<int> Nat;
            
        public void Execute()
        {
            for (int i = 0; i < Nat.Length; i++)
            {
                Nat[i]++;
            }
        }
    }
}