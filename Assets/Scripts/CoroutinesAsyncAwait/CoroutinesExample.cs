using System;
using System.Collections;
using UnityEngine;

public class CoroutinesExample : MonoBehaviour
{
   [SerializeField] private float _time = 3f;
   [SerializeField] private float _hight = 10f;
   [SerializeField] Animator _animator;
   private string _dance = "Dance";
   private string _sleep = "Sleep";

   private Coroutine _coroutine; 
   
   private void Start()
   {
      _coroutine = StartCoroutine(PrintMessage());
      StopCoroutine(_coroutine);
   }

   #region PrintMessage
   private IEnumerator PrintMessage()
   {
      while (true)
      {
         Debug.Log("Message");
         yield return null;
      }
   }
   #endregion
   
   #region PlayAnimation
   //StartCoroutine(PlayAnimation(_dance, 2f));
   //StartCoroutine(PlayAnimation(_sleep, 1f));
   //StartCoroutine(StopAnimation(0.5f));
   
   private IEnumerator StopAnimation(float delay)
   {
      yield return new WaitForSecondsRealtime(delay);
      _animator.StopPlayback();
      Debug.Log($"Animation stopped");
   }

   private IEnumerator PlayAnimation(string animation, float delay)
   {
      yield return new WaitForSecondsRealtime(delay);
      _animator.Play(animation);
      Debug.Log($"Animation playing {animation}");
   }
   #endregion

   #region PrintAndDestroy
   private IEnumerator PrintAndDestroy()
   {
      while (true)
      {
         Debug.Log($"{_hight} seconds left");
         _hight--;
         if (_hight == 3)
            this.enabled = false;

         if (_hight == 0)
            Destroy(this.gameObject);
         yield return new WaitForSecondsRealtime(1);
      }
   }
   #endregion

   #region MoveAround
   private IEnumerator MoveAround()
   {
      transform.position += Vector3.up;
      yield return new WaitForSecondsRealtime(_time);
      transform.position += Vector3.left;
      yield return new WaitForSecondsRealtime(_time);
      transform.position += Vector3.down;
      yield return new WaitForSecondsRealtime(_time);
      transform.position += Vector3.right;
      yield return new WaitForSecondsRealtime(_time);
   }
   #endregion

   #region MoveUp
   private IEnumerator MoveUp1(float time, Vector3 direction)
   {
      while (transform.position.y <_hight)
      {
         yield return new WaitForSecondsRealtime(time);
         transform.position += direction;
      }
   }

   private IEnumerator MoveUp(float time, Vector3 direction)
   {
      yield return new WaitForSecondsRealtime(time);
      transform.position += direction;
   }
   #endregion
   
   #region PrintOverTime
   private IEnumerator PrintOverTime()
   {
      Debug.Log($"Before yield");
      yield return new WaitForSecondsRealtime(_time);
      Debug.Log($"After yield in {_time} seconds");
   }
   #endregion
}
