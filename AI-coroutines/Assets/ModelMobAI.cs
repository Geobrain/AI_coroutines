using System.Collections;
using System.Collections.Generic;
using Pixeye.Actors;
using Pixeye.Source;
using UnityEngine;
//using MoreLinq;

/* сейчас максимум 10 поведений!!! в массиве поведений*/
sealed partial class ModelAI {
	
    public static void MobAI(in ent entity)
    {
		var cAI0 = entity.Set<ComponentAI>();
		
		#region AI_AAA
		
		ref var beh0 = ref cAI0.AddBehaviour(Tag.AI_AAA, (entWith, entAnother, behName) => AI_AAA(entWith, entAnother, behName));
		beh0.predicateTrigger = ent => ent.EnableBehaviour(Tag.AI_AAA);
		beh0.Kill = ent =>
		{
			// Kill только если нужны дополнительные действия внутри этого поведения перед тем, как поведение будет уничтожено другим с более высоким приоритетом
			Debug.Log("поведение AI_AAA прекратило работу");
			ent.KillBehaviour();
			//if (ent.Get(out ComponentMoveRbImpulse cMoveRbImpulse))
			//{
			//	ent.Remove<ComponentMoveRbImpulse>();
			//	ent.ComponentPhys().rb2D.velocity = Vector3.zero;
			//}
			return true;
		};
		
		
		IEnumerator AI_AAA(ent ent, ent entAnother = default, int behName = default)
		{
			var cMob = ent.ComponentMob();
			do
			{
	            Debug.Log(cMob.aaa);
	            yield return routines.waitFrame;
			} while (ent.exist);
		}
		
		#endregion
		
		
		#region AI_QQQ
		
		ref var beh1 = ref cAI0.AddBehaviour(Tag.AI_QQQ, (entWith, entAnother, behName) => AI_QQQ(entWith, entAnother, behName));
		beh1.enumeratorTrigger = (ent, behName) => Trigger_AI_QQQ(ent);
		beh1.Kill = ent =>
		{
			Debug.Log("поведение AI_QQQ прекратило работу");
			ent.KillBehaviour();
			return true;
		};
		
		IEnumerator Trigger_AI_QQQ(ent ent, ent entAnother = default, int behName = default)
		{
			var check = false;
			while (ent.exist && !check)
			{
				if (Input.GetKeyDown(KeyCode.Q))
				{
					ent.EnableBehaviour(Tag.AI_QQQ);
				}
				yield return routines.waitFrame;
			}
		}
		
		IEnumerator AI_QQQ(ent ent, ent entAnother = default, int behName = default)
		{
			var cMob = ent.ComponentMob();
			while (ent.exist)
			{
				Debug.Log(cMob.qqq);
				yield return routines.waitFrame;
			}
		}
		
		#endregion
		
		
		#region AI_WWW
		
		ref var beh2 = ref cAI0.AddBehaviour(Tag.AI_WWW, (entWith, entAnother, behName) => AI_WWW(entWith, entAnother, behName));
		beh2.enumeratorTrigger = (ent, behName) => Trigger_AI_WWW(ent);
		beh2.Kill = ent =>
		{
			ent.KillBehaviour();
			return true;
		};
		
		IEnumerator Trigger_AI_WWW(ent ent, ent entAnother = default, int behName = default)
		{
			var check = false;
			while (ent.exist && !check)
			{
				if (Input.GetKeyDown(KeyCode.W))
				{
					ent.EnableBehaviour(Tag.AI_WWW);
				}
				yield return routines.waitFrame;
			}
		}
		
		IEnumerator AI_WWW(ent ent, ent entAnother = default, int behName = default)
		{
			var cMob = ent.ComponentMob();
			var stop = false;
			Timer.Add(5f, () => { stop = true; });
			
			do
			{
				Debug.Log(cMob.www);
				yield return routines.waitFrame;
			} while (ent.exist && !stop);
		}
		
		#endregion
		
		

	}

}